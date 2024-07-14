using CarDealership.Core.Enums;
using CarDealership.Core.Models.Auth;
using CSharpFunctionalExtensions;
using System.Text;
using System.Text.Json.Serialization;

namespace CarDealership.Core.Models
{
    public class Deal : BaseModel
    {
        public DateTime DealDate { get; }
        public DealStatus Status { get; private set; }

        public decimal Price { get; }
        public Guid CarId { get; }
        public virtual Car Car { get; }

        public Guid? ManagerId { get; private set; }
        public virtual User Manager { get; }

        public Guid CustomerId { get; set; }
        public virtual User Customer { get; set; }

        [JsonConstructor]
        private Deal(Guid id, DateTime dealDate, DealStatus status, decimal price,  Guid carId, Guid? managerId, Guid customerId,
            bool isDeleted, Car car, User manager, User customer) : base(id)
        {
            DealDate = dealDate;
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

        public static Result<Deal> Create(Guid id, DateTime dealDate, DealStatus status, decimal price, Guid carId, Guid? managerId, Guid customerId,
            bool isDeleted = false, Car car = null, User manager = null, User customer = null)
        {
            var errorBuilder = new StringBuilder();

            if (carId == Guid.Empty)
            {
                errorBuilder.Append("CarId не должен быть пустым. ");
            }

            if(price <= 0)
            {
                errorBuilder.Append("Стоимость сделки не может быть меньше или равно 0. ");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<Deal>(errorBuilder.ToString().Trim());
            }

            var deal = new Deal(id, dealDate, status, price, carId, managerId, customerId,
                isDeleted, car, manager, customer);

            return Result.Success<Deal>(deal);
        }

        public void ChangeStatus(DealStatus status)
        {
            Status = status;
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
