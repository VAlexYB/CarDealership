﻿using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IOrdersService : IGenericService<Order, BaseFilter>
    {
        Task<Guid> ChangeStatus(Guid id, int status);
    }
}
