namespace Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [
            Required(ErrorMessage = "El nombre es requerido."),
            MaxLength(40, ErrorMessage = "El nombre puede tener máximo 40 caracteres.")
        ]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; } = 0m;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
