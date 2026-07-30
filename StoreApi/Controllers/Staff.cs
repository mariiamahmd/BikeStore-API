using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data;
using StoreApi.Dtos;
using StoreApi.Models;

namespace StoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly BikestoreContext _context;

        public StaffController(BikestoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var staff = await _context.Staffs.ToListAsync();

            return Ok(staff);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var staff = await _context.Staffs
                .Where(s => s.StaffId == id)
                .Select(s => new StaffDto
                {
                    StaffId = s.StaffId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Phone = s.Phone,
                    Email = s.Email,
                    Active = s.Active,
                    ManagerId = s.ManagerId,
                    StoreId = s.StoreId
                })
                .SingleOrDefaultAsync();

            if (staff == null)
                return NotFound();

            return Ok(staff);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaff(StaffDto dto)
        {
            var validStore = await _context.Stores
                .AnyAsync(s => s.StoreId == dto.StoreId);

            if (!validStore)
                return NotFound("Store not found");

            var staff = new Staff
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email,
                Active = dto.Active,
                ManagerId = dto.ManagerId,
                StoreId = dto.StoreId
            };

            await _context.Staffs.AddAsync(staff);
            await _context.SaveChangesAsync();

            return Ok(staff);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(int id, StaffDto dto)
        {
            var staff = await _context.Staffs.FindAsync(id);

            if (staff == null)
                return NotFound();

            var validStore = await _context.Stores
                .AnyAsync(s => s.StoreId == dto.StoreId);

            if (!validStore)
                return NotFound("Store not found");

            staff.FirstName = dto.FirstName;
            staff.LastName = dto.LastName;
            staff.Phone = dto.Phone;
            staff.Email = dto.Email;
            staff.Active = dto.Active;
            staff.ManagerId = dto.ManagerId;
            staff.StoreId = dto.StoreId;

            await _context.SaveChangesAsync();

            return Ok(staff);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var staff = await _context.Staffs.FindAsync(id);

            if (staff == null)
                return NotFound();

            _context.Staffs.Remove(staff);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}