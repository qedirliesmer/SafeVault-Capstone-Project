using System.Net;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;
using SafeVaultApp.Controllers;
using Xunit;

namespace SafeVaultApp.Tests
{
    public class SecurityTests
    {
        [Fact]
        public void UpdateProfile_ShouldEncodeHtml_ToPreventXSS()
        {
            // Arrange
            var encoder = HtmlEncoder.Default;
            var controller = new UserController(encoder);
            var dto = new UserProfileDto
            {
                Username = "testuser",
                Bio = "<script>alert('xss')</script>"
            };

            // Act
            var result = controller.UpdateProfile(dto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Contains("&lt;script&gt;alert(&#39;xss&#39;)&lt;/script&gt;", result.Value.ToString());
        }

        [Fact]
        public void SearchUser_ShouldRejectInvalidLengthInputs()
        {
            // Arrange
            var encoder = HtmlEncoder.Default;
            var controller = new UserController(encoder);
            string longInput = new string('A', 100);

            // Act
            var result = controller.SearchUser(longInput);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
