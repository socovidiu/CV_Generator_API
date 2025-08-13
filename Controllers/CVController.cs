using System.Security.Claims;
using CVGeneratorAPI.Dtos;
using CVGeneratorAPI.Mappers;
using CVGeneratorAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CVGeneratorAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/cvs")]
[Tags("CVs")]
public class CVsController : ControllerBase
{
    private readonly CVService _cvService;
    public CVsController(CVService cvService) => _cvService = cvService;

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    // GET /api/cvs
    [HttpGet]
    public async Task<ActionResult<List<CVResponse>>> GetAll()
    {
        var cvs = await _cvService.GetAllByUserAsync(UserId);
        return Ok(cvs.Select(c => c.ToResponse()).ToList());
    }

    // GET /api/cvs/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<CVResponse>> GetById(string id)
    {
        var cv = await _cvService.GetByIdForUserAsync(id, UserId);
        if (cv == null) return NotFound("CV not found.");
        return Ok(cv.ToResponse());
    }

    // POST /api/cvs
    [HttpPost]
    public async Task<ActionResult<CVResponse>> Create([FromBody] CreateCVRequest newCv)
    {
        var model = newCv.ToModel(UserId);
        await _cvService.CreateCvAsync(model);
        var response = model.ToResponse();
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    // PUT /api/cvs/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<CVResponse>> Update(string id, [FromBody] UpdateCVRequest updatedCv)
    {
        var existing = await _cvService.GetByIdForUserAsync(id, UserId);
        if (existing == null) return NotFound("CV not found.");

        updatedCv.ApplyToModel(existing);
        existing.Id = id;
        existing.UserId = UserId;

        await _cvService.UpdateForUserAsync(id, UserId, existing);
        return Ok(existing.ToResponse());
    }

    // DELETE /api/cvs/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var existing = await _cvService.GetByIdForUserAsync(id, UserId);
        if (existing == null) return NotFound("CV not found.");

        await _cvService.DeleteForUserAsync(id, UserId);
        return NoContent();
    }
}

