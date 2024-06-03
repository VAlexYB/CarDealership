using System.ComponentModel.DataAnnotations;

namespace CarDealership.Web.Api.Contracts.Requests
{
    public record RegistrationRequest(
        Guid Id,
        [Required] string UserName,
        [Required] string Email,
        [Required] string Password,
        string? FirstName,
        string? MiddleName,
        string? LastName,
        string? PhoneNumber,
        string? FirstCardDigits,
        string? LastCardDigits
    );
}
