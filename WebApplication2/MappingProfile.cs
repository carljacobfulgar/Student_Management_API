using StudentManagementApi.DTOs;
using StudentManagementApi.Models;

namespace StudentManagementApi.Mappings
{
    public static class StudentMappingExtensions
    {
        public static StudentDto ToDto(this Student student)
        {
            if (student == null) return null!;

            return new StudentDto
            {
                Id = student.Id,
                StudentNumber = student.StudentNumber,
                LastName = student.LastName,
                FirstName = student.FirstName,
                Gender = student.Gender,
                Address = student.Address,
                Birthday = student.Birthday,
                Birthplace = student.Birthplace
            };
        }

        public static Student ToEntity(this CreateStudentDto dto)
        {
            if (dto == null) return null!;

            return new Student
            {
                StudentNumber = dto.StudentNumber,
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                Gender = dto.Gender,
                Address = dto.Address,
                Birthday = dto.Birthday,
                Birthplace = dto.Birthplace
            };
        }

        public static void UpdateEntity(this UpdateStudentDto dto, Student student)
        {
            if (dto == null || student == null) return;

            student.LastName = dto.LastName;
            student.FirstName = dto.FirstName;
            student.Gender = dto.Gender;
            student.Address = dto.Address;
            student.Birthday = dto.Birthday;
            student.Birthplace = dto.Birthplace;
        }
    }
}