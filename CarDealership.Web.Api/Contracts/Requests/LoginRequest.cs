using System.ComponentModel.DataAnnotations;

namespace CarDealership.Web.Api.Contracts.Requests
{
    public record LoginRequest
    (
        [Required] string Identifier,
        [Required] string Password
    );
}
