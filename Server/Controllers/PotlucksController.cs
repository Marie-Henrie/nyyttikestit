using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Model;
using Microsoft.AspNetCore.Authorization;


namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PotlucksController : ControllerBase
    {
        private readonly NyyttiDbContext _context;

        public PotlucksController(NyyttiDbContext context)
        {
            _context = context;
        }

        // GET: api/Potlucks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Potluck>>> GetPotlucks()
        {
            return await _context.Potlucks.Include(p=>p.Pots).ThenInclude(p=>p.Tags).ToListAsync();
        }

        // GET: api/Potlucks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Potluck>> GetPotluck(int id)
        {
            var potluck = await _context.Potlucks.FindAsync(id);

            if (potluck == null)
            {
                return NotFound();
            }

            return potluck;
        }
        //[HttpGet("getpots/{guid}")]
        //public async Task<ActionResult<IEnumerable<Pot>>> GetPots(string guid)
        //{
        //    return await _context.Pots.Include(p => p.Potluck).Where(p => p.Potluck.Guid == guid).Include(p => p.Tags).ToListAsync();
        //}

        // GET: api/Potlucks/5
        [HttpGet("getpotluck/{guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Potluck>> GetPotluckByGuid(string guid)
        {
            var potluck = await _context.Potlucks.Where(p => p.Guid == guid).SingleAsync();

            if (potluck == null)
            {
                return NotFound();
            }

            return potluck;
        }

        // PUT: api/Potlucks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutPotluck(int id, PotluckDTO potluckDTO)
        {
            Potluck potluck = await _context.Potlucks.FindAsync(id);
            potluck.Description = potluckDTO.Description;
            potluck.Location = potluckDTO.Location;
            potluck.Date = potluckDTO.Date;

            if (potluck == null)
            {
                return BadRequest();
            }

            _context.Entry(potluck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PotluckExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Potlucks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Potluck>> PostPotluck(PotluckDTO potluckTDO)
        {
            string guid = "";
            while (true){
                guid = Guid.NewGuid().ToString().Substring(0, 8);
                if (!_context.Potlucks.Where(p => p.Guid == guid).Any()) break;
            }

            Potluck potluck = new Potluck() {Name=potluckTDO.Name,
                                             Guid= guid,
                                             Created = DateTime.Now,
                                             HashedPassword= potluckTDO.HashedPassword,
                                             Description= potluckTDO.Description,
                                             Date = potluckTDO.Date,
                                             Location= potluckTDO.Location
                                             };

            _context.Potlucks.Add(potluck);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPotluck", new { id = potluck.Potluck_Id }, potluck);
        }

        // DELETE: api/Potlucks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePotluck(int id)
        {
            var potluck = await _context.Potlucks.FindAsync(id);
            if (potluck == null)
            {
                return NotFound();
            }

            _context.Potlucks.Remove(potluck);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PotluckExists(int id)
        {
            return _context.Potlucks.Any(e => e.Potluck_Id == id);
        }
    }
}
