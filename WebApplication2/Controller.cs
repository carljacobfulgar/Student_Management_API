using Microsoft.AspNetCore.Mvc;
using StudentManagementApi.DTOs;
using StudentManagementApi.Mappings;
using StudentManagementApi.Models;

namespace StudentManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class StudentsController : ControllerBase
    {
        // Thread-safe in-memory store simulation
        private static readonly List<Student> _students = new();
        private static int _nextId = 1;
        private static readonly object _lock = new();

        // 1. GET ALL: api/students
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            lock (_lock)
            {
                var dtos = _students.Select(s => s.ToDto());
                return Ok(dtos);
            }
        }

        // 2. GET BY ID: api/students/1
        [HttpGet("{id:int:min(1)}", Name = "GetStudentById")]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById([FromRoute] int id)
        {
            lock (_lock)
            {
                var student = _students.FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return NotFound(new { Message = $"Student with ID {id} was not found." });
                }

                return Ok(student.ToDto());
            }
        }

        // 3. CREATE: api/students
        [HttpPost]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult Create([FromBody] CreateStudentDto? createDto)
        {
            if (createDto == null)
            {
                return BadRequest(new { Message = "Student data must not be null." });
            }

            lock (_lock)
            {
                if (_students.Any(s => s.StudentNumber.Equals(createDto.StudentNumber, StringComparison.OrdinalIgnoreCase)))
                {
                    return Conflict(new { Message = $"Student number '{createDto.StudentNumber}' already exists." });
                }

                var student = createDto.ToEntity();
                student.Id = _nextId++;
                _students.Add(student);

                var resultDto = student.ToDto();
                return CreatedAtRoute("GetStudentById", new { id = student.Id }, resultDto);
            }
        }

        // 4. UPDATE: api/students/1
        [HttpPut("{id:int:min(1)}")]
        [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStudentDto? updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest(new { Message = "Update data must not be null." });
            }

            lock (_lock)
            {
                var existingStudent = _students.FirstOrDefault(s => s.Id == id);
                if (existingStudent == null)
                {
                    return NotFound(new { Message = $"Student with ID {id} was not found." });
                }

                updateDto.UpdateEntity(existingStudent);
                return Ok(existingStudent.ToDto());
            }
        }

        // 5. DELETE: api/students/1
        [HttpDelete("{id:int:min(1)}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete([FromRoute] int id)
        {
            lock (_lock)
            {
                var student = _students.FirstOrDefault(s => s.Id == id);
                if (student == null)
                {
                    return NotFound(new { Message = $"Student with ID {id} was not found." });
                }

                _students.Remove(student);
                return NoContent();
            }
        }

        // 6. SEARCH QUERY: api/students/search?lastName=Doe&firstName=John
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<StudentDto>), StatusCodes.Status200OK)]
        public IActionResult Search([FromQuery] string? lastName, [FromQuery] string? firstName)
        {
            lock (_lock)
            {
                var query = _students.AsQueryable();

                if (!string.IsNullOrWhiteSpace(lastName))
                {
                    query = query.Where(s => s.LastName.Contains(lastName.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrWhiteSpace(firstName))
                {
                    query = query.Where(s => s.FirstName.Contains(firstName.Trim(), StringComparison.OrdinalIgnoreCase));
                }

                var results = query.Select(s => s.ToDto()).ToList();
                return Ok(results);
            }
        }
    }
}