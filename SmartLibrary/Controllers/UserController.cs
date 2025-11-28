using Microsoft.AspNetCore.Mvc;
using SmartLibrary.Interfaces;
using SmartLibrary.Models;

namespace SmartLibrary.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _users;

        public UserController(IUserService users)
        {
            _users = users;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _users.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _users.GetAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserDto dto)
        {
            // Create the correct type based on Role
            User user = dto.Role?.ToLower() switch
            {
                "student" => new Student
                {
                    Username = dto.Username,
                    Password = dto.Password,
                    Name = dto.Name,
                    Email = dto.Email,
                    Role = "Student",
                    Program = dto.Program
                },
                "faculty" => new Faculty
                {
                    Username = dto.Username,
                    Password = dto.Password,
                    Name = dto.Name,
                    Email = dto.Email,
                    Role = "Faculty",
                    Department = dto.Department
                },
                "staff" => new Staff
                {
                    Username = dto.Username,
                    Password = dto.Password,
                    Name = dto.Name,
                    Email = dto.Email,
                    Role = "Staff"
                },
                _ => throw new ArgumentException("Invalid role. Must be Student, Faculty, or Staff.")
            };

            await _users.AddAsync(user);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
        {
            var existingUser = await _users.GetAsync(id);
            if (existingUser == null) return NotFound();

            // Update properties
            existingUser.Username = dto.Username;
            existingUser.Password = dto.Password;
            existingUser.Name = dto.Name;
            existingUser.Email = dto.Email;

            // Update specific properties
            if (existingUser is Student student && dto.Program != null)
                student.Program = dto.Program;
            if (existingUser is Faculty faculty && dto.Department != null)
                faculty.Department = dto.Department;

            await _users.UpdateAsync(existingUser);
            return Ok(existingUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _users.DeleteAsync(id);
            return Ok(new { message = "User deleted" });
        }
    }

    // DTO class - INSIDE the namespace but OUTSIDE the controller class
    public class UserDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public string? Program { get; set; }      // For Student
        public string? Department { get; set; }   // For Faculty
    }
}