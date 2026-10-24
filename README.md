# SafeVault - Secure Application Capstone Project

## Project Overview
SafeVault is a secure Web API built with .NET 8 designed to demonstrate industry best practices for data protection, authentication, authorization, and vulnerability mitigation.

## Security Features Implemented
1. **Input Validation & SQL Injection Prevention**:
   - Implemented strict input length and character validation.
   - Utilized parameterized SQL queries (`SqlCommand.Parameters.AddWithValue`) to completely prevent SQL Injection attacks.

2. **XSS Mitigation**:
   - Sanitized user inputs and encoded outputs using `HtmlEncoder` to neutralize Cross-Site Scripting (XSS) payload risks.

3. **Authentication & Role-Based Access Control (RBAC)**:
   - Integrated JWT (JSON Web Token) authentication.
   - Enforced fine-grained RBAC policies (`RequireAdminRole`, `RequireUserRole`) to adhere to the Principle of Least Privilege.

4. **Copilot Assistance & Debugging**:
   - Microsoft Copilot assisted in identifying vulnerable unparameterized SQL queries and suggested secure encoding practices.
   - Generated unit testing scenarios to verify endpoint authorization and output sanitization behavior.

## How to Run Tests
Execute the security unit test suite using the CLI:
```bash
dotnet test
