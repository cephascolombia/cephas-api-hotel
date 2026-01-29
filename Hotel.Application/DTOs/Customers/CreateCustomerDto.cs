using System.ComponentModel.DataAnnotations;

namespace Hotel.Application.DTOs.Customers
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150, MinimumLength = 3, 
            ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(150, ErrorMessage = "El correo no puede exceder los 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        public string? Phone { get; set; }

        [StringLength(30, ErrorMessage = "El documento de identidad no puede exceder los 30 caracteres.")]
        public string? IdentityDocument { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        public string? Address { get; set; }

        [StringLength(500, ErrorMessage = "Las notas no pueden exceder los 500 caracteres.")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "El campo 'Creado por' es obligatorio.")]
        public string CreatedBy { get; set; } = string.Empty;
    }
}
