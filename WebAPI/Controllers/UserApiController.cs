using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserApiController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/UserApi
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            var list = await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Role = u.Role.Name
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET: api/UserApi/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var u = await _context.Users
                                  .Include(u => u.Role)
                                  .FirstOrDefaultAsync(u => u.Id == id);

            if (u == null)
                return NotFound();

            return Ok(new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                Role = u.Role.Name
            });
        }

        // POST: api/UserApi
        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody] UserDto dto)
        {
            if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Bu e-posta zaten kayıtlı.");

            var newUser = new Models.User
            {
                Email = dto.Email,
                RoleId = await GetRoleId(dto.Role),
                SessionId = System.Guid.NewGuid()
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            dto.Id = newUser.Id;
            return CreatedAtAction(nameof(Get), new { id = dto.Id }, dto);
        }

        // PUT: api/UserApi/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID uyuşmuyor.");

            var userToUpdate = await _context.Users.FindAsync(id);
            if (userToUpdate == null)
                return NotFound();

            userToUpdate.Email = dto.Email;
            userToUpdate.RoleId = await GetRoleId(dto.Role);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/UserApi/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete == null)
                return NotFound();

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Yardımcı: rol adından ID döner
        private async Task<int> GetRoleId(string roleName)
        {
            var role = await _context.Roles
                                     .FirstOrDefaultAsync(r => r.Name == roleName);
            if (role == null)
                throw new KeyNotFoundException($"Role '{roleName}' bulunamadı.");

            return role.Id;
        }
    }
}