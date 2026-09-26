using System.ComponentModel.DataAnnotations;

namespace Student_Management.DTOs
{
    public class StudentDTOs
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Course is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Course must be between 1 and 100 characters")]
        public string Course { get; set; }
    }
}
