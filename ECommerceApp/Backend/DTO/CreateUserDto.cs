using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class CreateUserDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
            ErrorMessage = "Password must contain at least 1 uppercase, 1 lowercase, 1 digit, and 1 special character.")]
        public string Password { get; set; }

        public List<AddressDto> AddressList { get; set; }
    }

    public class AddressDto
    {
        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Pincode { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
