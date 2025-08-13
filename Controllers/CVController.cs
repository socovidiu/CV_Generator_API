using System.Security.Claims;
using CVGeneratorAPI.Dtos;
using CVGeneratorAPI.Mappers;
using CVGeneratorAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CVGeneratorAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CVController : ControllerBase
    {
        private readonly CVService _cvService;

        public CVController(CVService cvService)
        {
            _cvService = cvService;
        }

        // Helper to read the user id (sub) from JWT
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        // GET: api/cv  -> only current user's CVs
        [HttpGet]
        public async Task<ActionResult<List<CVResponse>>> GetAll()
        {
            var cvs = await _cvService.GetAllByUserAsync(UserId);
            return Ok(cvs.Select(c => c.ToResponse()).ToList());
        }

        // GET: api/cv/{id}  -> only if owned by current user
        [HttpGet("{id}")]
        public async Task<ActionResult<CVResponse>> GetById(string id)
        {
            var cv = await _cvService.GetByIdForUserAsync(id, UserId);
            if (cv == null) return NotFound("CV not found.");
            return Ok(cv.ToResponse());
        }

        // POST: api/cv  -> create CV owned by current user
        [HttpPost]
       public async Task<ActionResult<CVResponse>> Create([FromBody] CreateCVRequest newCv)
        {
            var model = newCv.ToModel(UserId); // <-- pass current userId from JWT
            await _cvService.CreateCvAsync(model);

            var response = model.ToResponse();
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        // PUT: api/cv/{id}  -> update only if owned by current user
        [HttpPut("{id}")]
        public async Task<ActionResult<CVResponse>> Update(string id, [FromBody] UpdateCVRequest updatedCv)
        {
            var existing = await _cvService.GetByIdForUserAsync(id, UserId);
            if (existing == null) return NotFound("CV not found.");

            updatedCv.ApplyToModel(existing);
            existing.Id = id;            // keep stable id
            existing.UserId = UserId;    // never allow ownership change

            await _cvService.UpdateForUserAsync(id, UserId, existing);
            return Ok(existing.ToResponse());
        }

        // DELETE: api/cv/{id}  -> delete only if owned by current user
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var existing = await _cvService.GetByIdForUserAsync(id, UserId);
            if (existing == null) return NotFound("CV not found.");

            await _cvService.DeleteForUserAsync(id, UserId);
            return NoContent();
        }
    }
}
