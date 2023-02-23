using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Player
    {
        public Player()
        {
            PlayersStatistics = new HashSet<PlayerStatistic>();
        }

        [Key]
        public int PlayerId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int SquadNumber { get; set; }

        [Required]
        public int Assists { get; set; }

        [Required]
        public int TownId { get; set; }

        [Required]
        public Town Town { get; set; } = null!;

        [Required]
        [ForeignKey("Position")]
        public int PositionId { get; set; }

        [Required]
        public Position Position { get; set; } = null!;

        [Required]
        public bool IsInjured { get; set; }

        [Required]
        public int TeamId { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }

        [Required]
        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;
    }
}
