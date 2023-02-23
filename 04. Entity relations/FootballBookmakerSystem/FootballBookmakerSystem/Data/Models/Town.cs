using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {
        public Town()
        {
            Teams = new HashSet<Team>();
        }

        [Key]
        public int TownId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        [Required]
        public virtual Country Country { get; set; } = null!;

        [Required]
        public virtual ICollection<Team> Teams { get; set; }
    }
}
