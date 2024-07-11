using CarDealership.Core.Enums;
using CarDealership.Core.Models.Auth;
using CSharpFunctionalExtensions;
using System.Text;
using System.Text.Json.Serialization;

namespace CarDealership.Core.Models
{
    public class Order : BaseModel
    {
        public DateTime OrderDate { get; }
        public DateTime CompleteDate { get; }
        public OrderStatus Status { get; private set; }
        public decimal Price { get; }
        public Guid AutoConfigurationId { get; }
        public virtual AutoConfiguration AutoConfiguration { get; }

        public Guid? ManagerId { get; private set; }
        public virtual User Manager { get; }

        public Guid CustomerId { get; set; }
        public virtual User Customer { get; set; }

        [JsonConstructor]
        private Order(Guid id, DateTime orderDate, DateTime completeDate, OrderStatus status, decimal price, Guid autoConfigurationId, Guid? managerId, Guid customerId, 
            bool isDeleted, AutoConfiguration configuration, User manager, User customer) : base(id)
        {
            OrderDate = orderDate;
            CompleteDate = completeDate;
            Status = status;
            Price = price;
            AutoConfigurationId = autoConfigurationId;
            ManagerId = managerId;
            CustomerId = customerId;
            IsDeleted = isDeleted;
            AutoConfiguration = configuration;
            Manager = manager;
            Customer = customer;
        }

        public static Result<Order> Create(Guid id, DateTime orderDate, DateTime completeDate, OrderStatus status, decimal price, Guid autoConfigurationId, Guid? managerId, Guid customerId,
            bool isDeleted = false, AutoConfiguration configuration = null, User manager = null, User customer = null)
        {
            var errorBuilder = new StringBuilder();

            if (autoConfigurationId == Guid.Empty)
            {
                errorBuilder.Append("AutoConfigurationId не должен быть пустым. ");
            }

            if (price <= 0)
            {
                errorBuilder.Append("Стоимость заказа не может быть меньше или равно 0. ");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<Order>(errorBuilder.ToString().Trim());
            }

            var order = new Order(id, orderDate, completeDate, status, price, autoConfigurationId, managerId, customerId, 
                isDeleted, configuration, manager, customer);

            return Result.Success<Order>(order);
        }

        public void ChangeStatus(OrderStatus newStatus)
        {
            Status = newStatus;
        }

        public void SetAsManager(Guid managerId)
        {
            ManagerId = managerId;
        }

        public void RemoveManager()
        {
            ManagerId = null;
        }
    }
}
