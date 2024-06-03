using CarDealership.Core.Enums;
using CarDealership.DataAccess.Entities.Auth;

namespace CarDealership.DataAccess.Entities
{
    public class DealEntity : BaseEntity
    {
        public DateTime DealDate { get; set; }
        public DealStatus Status { get; set; }
        public Guid CarId { get; set; }
        public virtual CarEntity Car { get; set; }

        public Guid? ManagerId { get; set; }
        public virtual UserEntity Manager { get; set; }

        public Guid CustomerId { get; set; }
        public virtual UserEntity Customer { get; set; }
    }
}
