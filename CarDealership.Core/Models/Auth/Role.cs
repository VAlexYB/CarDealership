using CSharpFunctionalExtensions;

namespace CarDealership.Core.Models.Auth
{
    public class Role
    {
        public int Id { get;  }
        public string Value { get; }
        private readonly List<User> users = new List<User>();
        public IReadOnlyCollection<User> Users => users.AsReadOnly();

        private Role(int id, string value)
        {
            Id = id;
            Value = value;
        }

        public static Result<Role> Create(int id, string value)
        {
            var role = new Role(id, value);
            return Result.Success(role);
        }

        public void AddUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            users.Add(user);
        }
    }
}
