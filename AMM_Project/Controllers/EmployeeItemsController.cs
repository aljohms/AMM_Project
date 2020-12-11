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
    public class EmployeeItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeeItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeItem>>> GetEmployeeItem()
        {
            return await _context.EmployeeItem.ToListAsync();
        }

        // GET: api/EmployeeItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeItem>> GetEmployeeItem(int id)
        {
            var employeeItem = await _context.EmployeeItem.FindAsync(id);

            if (employeeItem == null)
            {
                return NotFound();
            }

            return employeeItem;
        }

        // PUT: api/EmployeeItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeItem(int id, EmployeeItem employeeItem)
        {
            if (id != employeeItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(employeeItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeItemExists(id))
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

        // POST: api/EmployeeItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeItem>> PostEmployeeItem(EmployeeItem employeeItem)
        {
            _context.EmployeeItem.Add(employeeItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeItem", new { id = employeeItem.Id }, employeeItem);
        }

        // DELETE: api/EmployeeItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeItem(int id)
        {
            var employeeItem = await _context.EmployeeItem.FindAsync(id);
            if (employeeItem == null)
            {
                return NotFound();
            }

            _context.EmployeeItem.Remove(employeeItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeItemExists(int id)
        {
            return _context.EmployeeItem.Any(e => e.Id == id);
        }
    }
}
