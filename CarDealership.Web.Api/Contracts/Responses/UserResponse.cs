namespace CarDealership.Web.Api.Contracts.Responses
{
    public class UserResponse : BaseResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CardDigits { get; set; }


        public UserResponse(Guid id) : base(id)
        {
        }
    }
}

