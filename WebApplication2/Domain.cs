using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementApi.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^STU-\d{4}-\d{4}$", ErrorMessage = "Student number must follow format STU-YYYY-XXXX (e.g., STU-2026-0001)")]
        public string StudentNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string Gender { get; set; } = string.Empty;

        public string? Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        public string? Birthplace { get; set; }
    }
}