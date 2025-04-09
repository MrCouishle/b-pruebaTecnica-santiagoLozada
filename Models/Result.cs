using Helpers;

namespace Models
{
    public class Result
    {
        [Key]
        public Guid Id { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BetValue { get; set; } = 0m;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Profit { get; set; } = 0m;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RemainingBalance { get; set; } = 0m;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CurrentBalance { get; set; } = 0m;
        public bool Winner { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User? User { get; set; }

        public int RouletteNumber { get; set; }

        [NotMapped]
        public ColorEnums Color => RouletteHelper.GetColor(RouletteNumber);

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
