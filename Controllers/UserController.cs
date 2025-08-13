using CVGeneratorAPI.Models;
using CVGeneratorAPI.Services;
using CVGeneratorAPI.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace CVGeneratorAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    // POST: api/user/signup
    [HttpPost("signup")]
    public async Task<ActionResult<AuthResponse>> SignUp([FromBody] SignUpRequest request)
    {
        var existing = await _userService.GetByUsernameAsync(request.Username);
        if (existing != null)
            return BadRequest(new AuthResponse { Message = "Username already exists." });

        var user = new UserModel
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password)
        };

        await _userService.CreateUserAsync(user);

        var response = new AuthResponse
        {
            Message = "User created successfully.",
            User = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            }
        };

        return Ok(response);
    }

    // POST: api/user/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var user = await _userService.GetByUsernameAsync(request.Username);
        if (user == null || user.PasswordHash != HashPassword(request.Password))
            return Unauthorized(new AuthResponse { Message = "Invalid credentials." });

        return Ok(new AuthResponse
        {
            Message = "Login successful.",
            User = new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            }
        });
    }

    // PUT: api/user/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponse>> Update(string id, [FromBody] UpdateUserRequest request)
    {
        var userInDb = await _userService.GetByUsernameAsync(request.Username);
        if (userInDb != null && userInDb.Id != id)
            return BadRequest("Username already taken.");

        var existingUser = await _userService.GetByUsernameAsync(userInDb?.Username ?? "");
        if (existingUser == null)
            return NotFound("User not found.");

        var passwordHash = string.IsNullOrWhiteSpace(request.Password)
            ? existingUser.PasswordHash
            : HashPassword(request.Password);

        var updatedUser = new UserModel
        {
            Id = id,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        await _userService.UpdateUserAsync(id, updatedUser);

        return Ok(new UserResponse
        {
            Id = updatedUser.Id,
            Username = updatedUser.Username,
            Email = updatedUser.Email
        });
    }

    // GET: api/user/{username}
    [HttpGet("{username}")]
    public async Task<ActionResult<UserResponse>> GetByUsername(string username)
    {
        var user = await _userService.GetByUsernameAsync(username);
        if (user == null)
            return NotFound("User not found.");

        return Ok(new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        });
    }

    // DELETE: api/user/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
