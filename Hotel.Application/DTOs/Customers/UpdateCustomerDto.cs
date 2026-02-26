using System.ComponentModel.DataAnnotations;

namespace Hotel.Application.DTOs.Customers
{
    public class UpdateCustomerDto
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(150, MinimumLength = 3, 
            ErrorMessage = "El nombre debe tener entre 3 y 150 caracteres.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string? Phone { get; set; }

        public string? IdentityDocument { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }

        public bool? IsActive { get; set; }
    }
}
