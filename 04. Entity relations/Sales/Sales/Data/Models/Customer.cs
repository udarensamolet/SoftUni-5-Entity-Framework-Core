using System.ComponentModel.DataAnnotations;

namespace P03_SalesDatabase.Data.Models
{
    public class Customer
    {
        public Customer()
        {
            Sales = new HashSet<Sale>();
        }
        [Key]
        public int CustomerId { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(80)]
        [Required]
        public string Email { get; set; }

        [MaxLength(50)]
        [Required]
        public string CreditCardNumber { get; set; }

        public ICollection<Sale> Sales { get; set; }
    }
}
