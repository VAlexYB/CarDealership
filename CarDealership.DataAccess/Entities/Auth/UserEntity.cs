namespace CarDealership.DataAccess.Entities.Auth
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? FirstName { get; set; } 
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstCardDigits { get; set; } 
        public string? LastCardDigits { get; set; }
        public bool? HasLinkedCard { get; set; }
        public virtual List<RoleEntity> Roles { get; set; } = [];

        public virtual List<OrderEntity> ManagedOrders { get; set; } = [];
        public virtual List<DealEntity> ManagedDeals { get; set; } = [];
        public virtual List<OrderEntity> CustomerOrders { get; set; } = [];
        public virtual List<DealEntity> CustomerDeals { get; set; } = [];

    }
}
