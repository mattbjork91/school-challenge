using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolChallenge.Models
{
    public partial class Teacher
    {
        [Key][Required]
        public int TeacherId { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Use numbers only please")]
        public int NumberOfStudents { get; set; }
    }
}
