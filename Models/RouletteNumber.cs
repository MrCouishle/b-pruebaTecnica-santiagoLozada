namespace Models
{
    [Index(nameof(Id), IsUnique = true)]
    public class RouletteNumber
    {
        [Key]
        public int Id { get; set; }

        [
            Required(ErrorMessage = "El color es requerido."),
            Range(0, 2, ErrorMessage = "Color inválido.")
        ]
        public ColorEnums Color { get; set; }
    }
}
