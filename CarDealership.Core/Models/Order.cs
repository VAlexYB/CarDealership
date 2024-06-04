using CarDealership.Core.Enums;
using CarDealership.Core.Models.Auth;
using CSharpFunctionalExtensions;
using System.Text;

namespace CarDealership.Core.Models
{
    public class Order : BaseModel
    {
        public DateTime OrderDate { get; }
        public DateTime CompleteDate { get; }
        public OrderStatus Status { get; }
        public decimal Price { get; }
        public Guid CarId { get; }
        public virtual Car Car { get; }

        public Guid? ManagerId { get; }
        public virtual User Manager { get; }

        public Guid CustomerId { get; set; }
        public virtual User Customer { get; set; }

        private Order(Guid id, DateTime orderDate, DateTime completeDate, OrderStatus status, decimal price, Guid carId, Guid? managerId, Guid customerId, 
            bool isDeleted, Car car, User manager, User customer) : base(id)
        {
            OrderDate = orderDate;
            CompleteDate = completeDate;
            Status = status;
            Price = price;
            CarId = carId;
            ManagerId = managerId;
            CustomerId = customerId;
            IsDeleted = isDeleted;
            Car = car;
            Manager = manager;
            Customer = customer;
        }

        public static Result<Order> Create(Guid id, DateTime orderDate, DateTime completeDate, OrderStatus status, decimal price, Guid carId, Guid? managerId, Guid customerId,
            bool isDeleted = false, Car car = null, User manager = null, User customer = null)
        {
            var errorBuilder = new StringBuilder();

            if (carId == Guid.Empty)
            {
                errorBuilder.Append("CarId не должен быть пустым. ");
            }

            if (price <= 0)
            {
                errorBuilder.Append("Стоимость заказа не может быть меньше или равно 0. ");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<Order>(errorBuilder.ToString().Trim());
            }

            var order = new Order(id, orderDate, completeDate, status, price, carId, managerId, customerId, 
                isDeleted, car, manager, customer);

            return Result.Success<Order>(order);
        }
    }
}
