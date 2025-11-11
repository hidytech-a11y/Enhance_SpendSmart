using System.ComponentModel.DataAnnotations;


namespace SpendSmart.Models
{
    public class User
    {
        [Key]

        public int userId { get; set; }

        [Required]
        public required string Name { get; set; }

        public int Age { get; set; }

        public string? Address { get; set; }

        public string? Profession { get; set; }

    }
}
