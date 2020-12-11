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
    public class BranchItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BranchItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BranchItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchItem>>> GetBranchItem()
        {
            return await _context.BranchItem.ToListAsync();
        }

        // GET: api/BranchItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BranchItem>> GetBranchItem(int id)
        {
            var branchItem = await _context.BranchItem.FindAsync(id);

            if (branchItem == null)
            {
                return NotFound();
            }

            return branchItem;
        }

        // PUT: api/BranchItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBranchItem(int id, BranchItem branchItem)
        {
            if (id != branchItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(branchItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchItemExists(id))
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

        // POST: api/BranchItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BranchItem>> PostBranchItem(BranchItem branchItem)
        {
            _context.BranchItem.Add(branchItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBranchItem", new { id = branchItem.Id }, branchItem);
        }

        // DELETE: api/BranchItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranchItem(int id)
        {
            var branchItem = await _context.BranchItem.FindAsync(id);
            if (branchItem == null)
            {
                return NotFound();
            }

            _context.BranchItem.Remove(branchItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BranchItemExists(int id)
        {
            return _context.BranchItem.Any(e => e.Id == id);
        }
    }
}
