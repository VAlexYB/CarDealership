﻿using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Models;
using CarDealership.DataAccess.Entities;
using CarDealership.DataAccess.Factories;
using Microsoft.Extensions.Caching.Distributed;

namespace CarDealership.DataAccess.Repositories
{
    public class TransmissionTypesRepository : BaseRepository<TransmissionType, TransmissionTypeEntity, BaseFilter>, ITransmissionTypesRepository
    {
        public TransmissionTypesRepository(CarDealershipDbContext context, IEntityModelFactory<TransmissionType, TransmissionTypeEntity> factory,  IDistributedCache cache) : base(context, factory, cache)
        {
        }
    }
}
