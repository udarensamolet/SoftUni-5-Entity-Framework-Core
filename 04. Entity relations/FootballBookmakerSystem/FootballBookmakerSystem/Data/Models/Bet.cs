using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P02_FootballBetting.Data.Models
{
    public class Bet
    {
        [Key]
        public int BetId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string Prediction { get; set; } = null!;

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateTime { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public User User { get; set; } = null!;

        [Required]
        [ForeignKey("Game")]
        public int GameId { get; set; }

        [Required]
        public Game Game { get; set; } = null!;
    }
}
