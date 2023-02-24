using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public Game Game { get; set; } = null!;

        [Required]
        public int PlayerId { get; set; }

        [Required]
        public Player Player { get; set; } = null!;

        [Required]
        public int ScoredGoals { get; set; }

        [Required]
        public int Assists { get; set; }
        [Required]
        public int MinutesPlayed { get; set; }
    }
}
