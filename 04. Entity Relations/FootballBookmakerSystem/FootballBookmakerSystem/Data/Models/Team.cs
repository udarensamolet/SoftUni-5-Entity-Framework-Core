using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            Players = new HashSet<Player>();
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
        }

        [Key]
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string LogoUrl { get; set; } = null!;

        [Required]
        [MaxLength(3)]
        public string Initials { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Budget { get; set; }

        [Required]
        [ForeignKey("PrimaryKitColor")]
        public int PrimaryKitColorId { get; set; }

        [Required]
        public Color PrimaryKitColor { get; set; } = null!;


        [Required]
        [ForeignKey("SecondaryKitColor")]
        public int SecondaryKitColorId { get; set; }

        [Required]
        public Color SecondaryKitColor { get; set; } = null!;

        [Required]
        [ForeignKey("Town")]
        public int TownId { get; set; }

        [Required]
        public Town Town { get; set; } = null!;

        public virtual ICollection<Player> Players { get; set; } = null!;

        [Required]
        [InverseProperty("HomeTeam")]
        public virtual ICollection<Game> HomeGames { get; set; } = null!;

        [Required]
        [InverseProperty("AwayTeam")]
        public virtual ICollection<Game> AwayGames { get; set; } = null!;
    }
}
