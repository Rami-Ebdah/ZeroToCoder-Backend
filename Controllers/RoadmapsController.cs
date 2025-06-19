
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZeroToCoder.Dto;
using ZeroToCoder.Models;
using ZeroToCoder.Data;

namespace ZeroToCoder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoadmapsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoadmapsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoadmapReadDto>>> GetAll()
        {
            var roadmaps = await _context.Roadmaps.ToListAsync();
            return roadmaps.Select(r => new RoadmapReadDto
            {
                RoadmapID = r.RoadmapID,
                ImagePath = r.ImagePath,
                Specialization = r.Specialization
            }).ToList();
        }

        [HttpPost]
        public async Task<ActionResult<RoadmapReadDto>> Create(RoadmapCreateDto dto)
        {
            var roadmap = new Roadmap
            {
                ImagePath = dto.ImagePath,
                Specialization = dto.Specialization
            };

            _context.Roadmaps.Add(roadmap);
            await _context.SaveChangesAsync();

            var result = new RoadmapReadDto
            {
                RoadmapID = roadmap.RoadmapID,
                ImagePath = roadmap.ImagePath,
                Specialization = roadmap.Specialization
            };

            return CreatedAtAction(nameof(GetById), new { id = roadmap.RoadmapID }, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoadmapReadDto>> GetById(int id)
        {
            var roadmap = await _context.Roadmaps.FindAsync(id);

            if (roadmap == null)
                return NotFound();

            return new RoadmapReadDto
            {
                RoadmapID = roadmap.RoadmapID,
                ImagePath = roadmap.ImagePath,
                Specialization = roadmap.Specialization
            };
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, RoadmapUpdateDto dto)
        {
            var roadmap = await _context.Roadmaps.FindAsync(id);

            if (roadmap == null)
                return NotFound();

            roadmap.ImagePath = dto.ImagePath;
            roadmap.Specialization = dto.Specialization;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var roadmap = await _context.Roadmaps.FindAsync(id);

            if (roadmap == null)
                return NotFound();

            _context.Roadmaps.Remove(roadmap);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}