using CVGeneratorAPI.Dtos;
using CVGeneratorAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CVGeneratorAPI.Controllers;

[ApiController]
[Route("api/sessions")]
[Tags("Sessions")]
public class SessionsController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public SessionsController(UserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    // POST /api/sessions  (login)
    [AllowAnonymous]
    [HttpPost]  
    public async Task<ActionResult<AuthResponse>> Create([FromBody] LoginRequest request)
    {
        var user = await _userService.GetByUsernameAsync(request.Username);
        if (user == null) return Unauthorized(new AuthResponse { Message = "Invalid credentials." });

        // Assuming you're still using SHA256 hashing as before
        if (user.PasswordHash != UserControllerHash(request.Password))
            return Unauthorized(new AuthResponse { Message = "Invalid credentials." });

        var token = _tokenService.Create(user);

        return Ok(new AuthResponse
        {
            Message = "Login successful.",
            Token = token,
            User = new UserResponse { Id = user.Id!, Username = user.Username, Email = user.Email }
        });
    }

    // DELETE /api/sessions  (logout)
    // Stateless JWT: clients just discard the token. This is a 204 placeholder.
    [Authorize]
    [HttpDelete]
    public IActionResult Delete()
    {
        return NoContent();
    }

    // Reuse the same hashing logic as before (mirror of UsersController private method)
    private static string UserControllerHash(string password)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
