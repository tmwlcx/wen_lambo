using AttorneyScheduler.DAL;
using AttorneyScheduler.DAL.Tables;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttorneyScheduler.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AttorneyController : ControllerBase
    {
        private readonly AttorneySchedulerDbContext _context;

        public AttorneyController(AttorneySchedulerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attorney>>> GetAttorneys()
        {
            return await _context.Attorney.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Attorney>> GetAttorney(int id)
        {
            var attorney = await _context.Attorney.FindAsync(id);

            if (attorney == null)
            {
                return NotFound();
            }

            return attorney;
        }

        [HttpPost]
        public async Task<ActionResult<Attorney>> PostAttorney(Attorney attorney)
        {
            _context.Attorney.Add(attorney);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttorney", new { id = attorney.AttorneyId }, attorney);
        }

        [HttpGet("AttorneyType")]
        public async Task<ActionResult<IEnumerable<AttorneyType>>> GetAttorneyTypes()
        {
            return await _context.AttorneyType.ToListAsync();
        }

    }

}
