using CVGeneratorAPI.Models;
using CVGeneratorAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CVGeneratorAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVController : ControllerBase
    {
        private readonly CVService _cvService;

        public CVController(CVService cvService)
        {
            _cvService = cvService;
        }

        // GET: api/cv (Get all CVs)
        [HttpGet]
        public async Task<ActionResult<List<CVModel>>> GetAll()
        {
            var cvs = await _cvService.GetAllCvsAsync();
            return Ok(cvs);
        }

        // GET: api/cv/{id} (Get a single CV by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<CVModel>> GetById(string id)
        {
            var cv = await _cvService.GetCvByIdAsync(id);
            if (cv == null)
                return NotFound("CV not found.");
            
            return Ok(cv);
        }

        // POST: api/cv (Create a new CV)
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CVModel newCv)
        {
            await _cvService.CreateCvAsync(newCv);
            return CreatedAtAction(nameof(GetById), new { id = newCv.Id }, newCv);
        }

        // PUT: api/cv/{id} (Update an existing CV)
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] CVModel updatedCv)
        {
            var existingCv = await _cvService.GetCvByIdAsync(id);
            if (existingCv == null)
                return NotFound("CV not found.");

            updatedCv.Id = id; // Ensure the ID remains the same
            await _cvService.UpdateCvAsync(id, updatedCv);
            return NoContent();
        }

        // DELETE: api/cv/{id} (Delete a CV)
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var cv = await _cvService.GetCvByIdAsync(id);
            if (cv == null)
                return NotFound("CV not found.");

            await _cvService.DeleteCvAsync(id);
            return NoContent();
        }
    }
}