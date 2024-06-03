using CSharpFunctionalExtensions;

namespace CarDealership.Core.Models.Auth
{
    public class User
    {
        public Guid Id { get; }
        public bool IsDeleted { get; }
        public string UserName { get; } 
        public string Email { get; }
        public string PasswordHash { get; }
        public string? FirstName { get; }
        public string? MiddleName { get; }
        public string? LastName { get; }
        public string? PhoneNumber { get; }
        public string? FirstCardDigits { get; }
        public string? LastCardDigits { get; }
        public bool? HasLinkedCard { get; }


        private readonly List<Role> roles = new List<Role>();

        private readonly List<Order> managedOrders = new List<Order>();
        private readonly List<Deal> managedDeals = new List<Deal>();
        private readonly List<Order> customerOrders = new List<Order>();
        private readonly List<Deal> customerDeals = new List<Deal>();

        public IReadOnlyCollection<Role> Roles => roles.AsReadOnly();
        public IReadOnlyCollection<Order> ManagedOrders => managedOrders.AsReadOnly();
        public IReadOnlyCollection<Deal> ManagedDeals => managedDeals.AsReadOnly();
        public IReadOnlyCollection<Order> CustomerOrders => customerOrders.AsReadOnly();
        public IReadOnlyCollection<Deal> CustomerDeals => customerDeals.AsReadOnly();

        private User(Guid id, string userName, string email, string passwordHash, string firstName, string middleName, string lastName,
            string phoneNumber, string firstCardDigits, string lastCardDigits, bool isDeleted)
        {
            Id = id;
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            FirstCardDigits = firstCardDigits;
            LastCardDigits = lastCardDigits;
            HasLinkedCard = firstCardDigits != null && lastCardDigits != null;
            IsDeleted = isDeleted;
        }

        public static Result<User> Create(Guid id, string userName, string email, string passwordHash, string firstName, string middleName, string lastName,
            string phoneNumber, string firstCardDigits, string lastCardDigits, bool isDeleted = false)
        {
            var user = new User(id, userName, email, passwordHash, firstName, middleName, lastName, phoneNumber, firstCardDigits, lastCardDigits, isDeleted);
            return Result.Success(user);
        }

        public void AddRole(Role role)
        {
            if (role == null) throw new ArgumentNullException(nameof(role));
            roles.Add(role);
        }

        public void AddManagedOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            managedOrders.Add(order);
        }

        public void AddManagedDeal(Deal deal)
        {
            if (deal == null) throw new ArgumentNullException(nameof(deal));
            managedDeals.Add(deal);
        }

        public void AddCustomerOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            customerOrders.Add(order);
        }

        public void AddCustomerDeal(Deal deal)
        {
            if (deal == null) throw new ArgumentNullException(nameof(deal));
            customerDeals.Add(deal);
        }
    }
}
