using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Server.Model;
using Microsoft.AspNetCore.Authorization;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PotsController : ControllerBase
    {
        private readonly NyyttiDbContext _context;

        public PotsController(NyyttiDbContext context)
        {
            _context = context;
        }

        // GET: api/Pots
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pot>>> GetPots()
        {
            return await _context.Pots.ToListAsync();
        }

        [HttpGet("getpots/{guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Pot>>> GetPots(string guid)
        {
            return await _context.Pots.Include(p=>p.Potluck).Where(p=>p.Potluck.Guid==guid).Include(p=>p.Tags).ToListAsync();
        }

        // GET: api/Pots/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Pot>> GetPot(int id)
        {
            var pot = await _context.Pots.FindAsync(id);

            if (pot == null)
            {
                return NotFound();
            }

            return pot;
        }

        // PUT: api/Pots/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{guid}/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> PutPot(string guid, int id, PotDTO potDTO)
        {
            Potluck pl = _context.Potlucks.Where(p => p.Guid == guid).First();
            if (pl == null) return NotFound();

            Pot pot = _context.Pots.Include(p=>p.Tags).ThenInclude(t=>t.Pots).First(p=>p.Pot_Id==id);

            

            if (pot == null || pot.Potluck != pl)
            {
                return BadRequest();
            }

            pot.Course_Id = potDTO.Course_Id;
            pot.Creator = potDTO.Creator;
            pot.DishName = potDTO.DishName;
            pot.Potluck = pl;
            pot.Tags = new List<Tag>();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PotExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            foreach (int i in potDTO.tag_ids)
            {
                Tag tag = await _context.Tags.FindAsync(i);

                if (tag != null && pot.Tags.Where(t=>t.Tag_Id==tag.Tag_Id).Count() == 0)
                {
                    tag.Pots.Add(pot);
                    _context.Entry(tag).State = EntityState.Modified;
                    pot.Tags.Add(tag);
                }
                else if (tag != null && pot.Tags.Where(t => t.Tag_Id == tag.Tag_Id).Count() > 0)
                {
                    pot.Tags.Remove(tag);
                    tag.Pots.Remove(pot);
                    _context.Entry(tag).State = EntityState.Modified;
                }
            }

            _context.Entry(pot).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PotExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPot", new { id = pot.Pot_Id }, pot);
        }

        // POST: api/Pots
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<Pot>> PostPot(string guid, PotDTO potDTO)
        {
            Potluck pl = _context.Potlucks.Where(p => p.Guid == guid).First();
            if (pl == null) return NotFound();

            Pot pot = new Pot() { Course_Id = potDTO.Course_Id,
                                  Creator = potDTO.Creator,
                                  Created = DateTime.Now,
                                  DishName = potDTO.DishName,
                                  Description = potDTO.Description,
                                  Potluck = pl,
                                  Tags = new List<Tag>()
                                  };
            
            foreach(int i in potDTO.tag_ids)
            {
                Tag tag = await _context.Tags.FindAsync(i);
                if(tag != null)
                {
                    pot.Tags.Add(tag);
                }
            }

            _context.Pots.Add(pot);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPot", new { id = pot.Pot_Id }, pot);
        }

        // DELETE: api/Pots/5
        [HttpDelete("{guid}/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeletePot(string guid,int id)
        {
            Potluck pl = _context.Potlucks.Where(p => p.Guid == guid).First();

            var pot = await _context.Pots.FindAsync(id);
            if (pot == null || pl == null || pl.Pots.Count(p=>p.Pot_Id == id) == 0)
            {
                return NotFound();
            }

            _context.Pots.Remove(pot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PotExists(int id)
        {
            return _context.Pots.Any(e => e.Pot_Id == id);
        }
    }
}
