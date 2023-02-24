using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Game
    {
        public Game()
        {
            Bets = new HashSet<Bet>();
            PlayersStatistics = new HashSet<PlayerStatistic>();
        }

        [Key]
        public int GameId { get; set; }

        [Required]
        [ForeignKey("HomeTeam")]
        public int HomeTeamId { get; set; }

        [Required]
        public Team HomeTeam { get; set; } = null!;

        [Required]
        [ForeignKey("AwayTeam")]
        public int AwayTeamId { get; set; }

        [Required]
        public Team AwayTeam { get; set; } = null!;

        [Required]
        public int HomeTeamGoals { get; set; }

        [Required]
        public int AwayTeamGoals { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal HomeTeamBetRate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal AwayTeamBetRate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DrawBetRate { get; set; }

        [Required]
        public int Result { get; set; }

        [Required]
        public virtual ICollection<Bet> Bets { get; set; } = null!;

        [Required]
        public virtual ICollection<PlayerStatistic> PlayersStatistics { get; set; } = null!;
    }
}
