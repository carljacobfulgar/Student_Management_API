using System.ComponentModel.DataAnnotations;

namespace StudentManagementApi.DTOs
{
    public class UpdateStudentDto
    {
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        public string? Address { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string? Birthplace { get; set; }
    }
}