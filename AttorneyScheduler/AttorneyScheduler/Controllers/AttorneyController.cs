﻿using AttorneyScheduler.DAL;
using AttorneyScheduler.DAL.Tables;
using AttorneyScheduler.DTO;
using AttorneyScheduler.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttorneyScheduler.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class AttorneyController : BaseController
    {
        private readonly IAttorneyService attorneyService;

        public AttorneyController(IAttorneyService attorneyService, AttorneySchedulerDbContext context) : base(context)
        {
            this.attorneyService = attorneyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attorney>>> GetAttorneys()
        {
            var attorneys = await attorneyService.GetAttorneys();
            if (attorneys == null) 
            { 
                return NotFound(); 
            }
            return Ok(attorneys);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Attorney>> GetAttorney(int id)
        {
            var attorney = await attorneyService.GetAttorney(id);

            if (attorney == null)
            {
                return NotFound();
            }

            return attorney;
        }

        [HttpPost]
        public async Task<ActionResult<Attorney>> PostAttorney(Attorney attorney)
        {
            var createdAttorney = await attorneyService.CreateAttorney(attorney);
            if (createdAttorney == null)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetAttorney), new { id = attorney.AttorneyId }, attorney);
        }

        [HttpGet("AttorneyType")]
        public async Task<ActionResult<IEnumerable<AttorneyType>>> GetAttorneyTypes()
        {
            var types = await attorneyService.GetAttorneyTypes();
            if (types == null)
            {
                return NotFound();
            }
            return Ok(types);
        }

        [HttpGet("TimeOff/{id}")]
        public async Task<ActionResult<AttorneyTimeOff>> GetAttorneyTimeOff(int id)
        {
            var timeOff = await attorneyService.GetAttorneyTimeOff(id);
            if (timeOff == null)
            {
                return NotFound();
            }
            return timeOff;
        }

        [HttpPost("TimeOff")]
        public async Task<ActionResult<AttorneyTimeOff>> CreateAttorneyTimeOff([FromBody] AttorneyTimeOffDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var attorneyTimeOff = await attorneyService.CreateAttorneyTimeOff(request.AttoneryId, request.TimeOffDateFrom, request.TimeOffDateTo);

            return CreatedAtAction(nameof(GetAttorneyTimeOff), new { id = attorneyTimeOff.AttorneyTimeOffId }, attorneyTimeOff);
        }

        [HttpPut("TimeOff/{id}")]
        public async Task<IActionResult> PutAttorneyTimeOff(int id, AttorneyTimeOff attorneyTimeOff)
        {
            if (id != attorneyTimeOff.AttorneyTimeOffId)
            {
                return BadRequest();
            }

            var updatedTimeOff = await attorneyService.UpdateAttorneyTimeOff(id, attorneyTimeOff);
            if (updatedTimeOff == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }

}
