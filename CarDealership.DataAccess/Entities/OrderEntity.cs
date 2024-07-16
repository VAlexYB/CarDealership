using CarDealership.Core.Enums;
using CarDealership.DataAccess.Entities.Auth;

namespace CarDealership.DataAccess.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Price { get; set; }
        public Guid AutoConfigurationId { get; set; }
        public virtual AutoConfigurationEntity AutoConfiguration { get; set; }

        public Guid? ManagerId { get; set; }
        public virtual UserEntity Manager { get; set; }

        public Guid CustomerId {  get; set; }
        public virtual UserEntity Customer { get; set; }
    }
}
