using CarDealership.Core.Models.Auth;

namespace CarDealership.DataAccess.Entities.Auth
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public virtual List<UserEntity> Users { get; set; } = [];
    }
}
