using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AMM_Project.Models;

namespace AMM_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuisnessesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BuisnessesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Buisnesses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Buisness>>> GetBuisness()
        {
            return await _context.Buisness.ToListAsync();
        }

        // GET: api/Buisnesses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Buisness>> GetBuisness(int id)
        {
            var buisness = await _context.Buisness.FindAsync(id);

            if (buisness == null)
            {
                return NotFound();
            }

            return buisness;
        }

        // PUT: api/Buisnesses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBuisness(int id, Buisness buisness)
        {
            if (id != buisness.Id)
            {
                return BadRequest();
            }

            _context.Entry(buisness).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuisnessExists(id))
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

        // POST: api/Buisnesses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Buisness>> PostBuisness(Buisness buisness)
        {
            _context.Buisness.Add(buisness);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBuisness", new { id = buisness.Id }, buisness);
        }

        // DELETE: api/Buisnesses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBuisness(int id)
        {
            var buisness = await _context.Buisness.FindAsync(id);
            if (buisness == null)
            {
                return NotFound();
            }

            _context.Buisness.Remove(buisness);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BuisnessExists(int id)
        {
            return _context.Buisness.Any(e => e.Id == id);
        }
    }
}
