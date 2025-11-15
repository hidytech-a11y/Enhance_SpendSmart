using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpendSmart.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }
        public decimal Value { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please select a user.")]

        public int UserId { get; set; }

        [ForeignKey("UserId")]

        public User? User { get; set; }

    }
    


}
