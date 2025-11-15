using System.ComponentModel.DataAnnotations;

namespace SpendSmart.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Profession is required.")]
        public string? Profession { get; set; }

        public ICollection<Expense>? Expenses { get; set; }
    }
}
