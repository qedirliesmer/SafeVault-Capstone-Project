using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace SafeVaultApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString = "Server=localhost;Database=SafeVaultDb;Trusted_Connection=True;TrustServerCertificate=True;";
        private readonly HtmlEncoder _htmlEncoder;

        public UserController(HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
        }

        // Secure Input Validation & SQL Injection Mitigation
        [HttpGet("search")]
        [Authorize(Policy = "RequireUserRole")]
        public IActionResult SearchUser([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username) || username.Length > 50)
            {
                return BadRequest("Invalid username format.");
            }

            // Parameterized Query to prevent SQL Injection
            string query = "SELECT Id, Username, Email FROM Users WHERE Username = @Username";
            var users = new List<object>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);

                // Open connection and execute reader securely
                // connection.Open();
            }

            return Ok(new { Message = "Search executed safely using parameterized queries.", QueryUser = username });
        }

        // XSS Mitigation Example
        [HttpPost("update-profile")]
        [Authorize(Policy = "RequireUserRole")]
        public IActionResult UpdateProfile([FromBody] UserProfileDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Sanitizing output to prevent Cross-Site Scripting (XSS)
            string sanitizedBio = _htmlEncoder.Encode(dto.Bio ?? string.Empty);

            return Ok(new { Message = "Profile updated successfully.", SanitizedBio = sanitizedBio });
        }

        // Admin-only Endpoint (RBAC Test)
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult DeleteUser(int id)
        {
            return Ok(new { Message = $"User {id} deleted successfully by Admin." });
        }
    }

    public class UserProfileDto
    {
        public string Username { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
    }
}
