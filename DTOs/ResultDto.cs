namespace DTOs
{
    public class ResultCreateDto
    {
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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class BetValidationDto
    {
        public Guid UserId { get; set; }
        public decimal BetValue { get; set; } // Valor apostado
        public int? ResultNumber { get; set; } // Resultado de la ruleta
        public int? BetNumber { get; set; } // Número específico
        public string? BetColor { get; set; } // red, black, green
        public string? EvenOdd { get; set; } // even, odd
    }
}
