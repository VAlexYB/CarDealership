﻿using CarDealership.Core.Enums;
using CarDealership.Core.Models.Auth;
using CSharpFunctionalExtensions;
using System.Text;

namespace CarDealership.Core.Models
{
    public class Deal : BaseModel
    {
        public DateTime DealDate { get; }
        public DealStatus Status { get; }
        public Guid CarId { get; }
        public virtual Car Car { get; }

        public Guid? ManagerId { get; }
        public virtual User Manager { get; }

        public Guid CustomerId { get; set; }
        public virtual User Customer { get; set; }

        private Deal(Guid id, DateTime dealDate, DealStatus status, Guid carId, Guid? managerId, Guid customerId,
            bool isDeleted, Car car, User manager, User customer) : base(id)
        {
            DealDate = dealDate;
            Status = status;
            CarId = carId;
            ManagerId = managerId;
            CustomerId = customerId;
            IsDeleted = isDeleted;
            Car = car;
            Manager = manager;
            Customer = customer;
        }

        public static Result<Deal> Create(Guid id, DateTime dealDate, DealStatus status, Guid carId, Guid? managerId, Guid customerId,
            bool isDeleted = false, Car car = null, User manager = null, User customer = null)
        {
            var errorBuilder = new StringBuilder();

            if (carId == Guid.Empty)
            {
                errorBuilder.Append("CarId не должен быть пустым. ");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<Deal>(errorBuilder.ToString().Trim());
            }

            var deal = new Deal(id, dealDate, status, carId, managerId, customerId,
                isDeleted, car, manager, customer);

            return Result.Success<Deal>(deal);
        }
    }
}