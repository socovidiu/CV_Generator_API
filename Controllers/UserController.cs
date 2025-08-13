using CVGeneratorAPI.Models;
using CVGeneratorAPI.Services;
using CVGeneratorAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CVGeneratorAPI.Controllers;

[ApiController]
[Route("api/users")]
[Tags("Users")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly TokenService _tokenService;

    public UsersController(UserService userService, TokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    // POST /api/users  (signup)
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<AuthResponse>> Create([FromBody] SignUpRequest request)
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

        var token = _tokenService.Create(user);

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, new AuthResponse
        {
            Message = "User created successfully.",
            Token = token,
            User = new UserResponse { Id = user.Id!, Username = user.Username, Email = user.Email }
        });
    }

    // GET /api/users/{id}  (self only)
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetById(string id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != id) return Forbid();

        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound("User not found.");

        return Ok(new UserResponse { Id = user.Id!, Username = user.Username, Email = user.Email });
    }

    // PUT /api/users/{id}  (self only)
    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponse>> Update(string id, [FromBody] UpdateUserRequest request)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != id) return Forbid();

        var existingByUsername = await _userService.GetByUsernameAsync(request.Username);
        if (existingByUsername != null && existingByUsername.Id != id)
            return BadRequest("Username already taken.");

        var existingUser = await _userService.GetByIdAsync(id);
        if (existingUser == null) return NotFound("User not found.");

        var passwordHash = string.IsNullOrWhiteSpace(request.Password)
            ? existingUser.PasswordHash
            : HashPassword(request.Password);

        var updated = new UserModel
        {
            Id = id,
            Username = request.Username,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        await _userService.UpdateUserAsync(id, updated);

        return Ok(new UserResponse { Id = updated.Id!, Username = updated.Username, Email = updated.Email });
    }

    // DELETE /api/users/{id}  (self only)
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId != id) return Forbid();

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
