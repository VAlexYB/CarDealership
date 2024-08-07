﻿using CarDealership.Core.Filters;
using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Services
{
    public interface IAutoConfigsService : IGenericService<AutoConfiguration, ConfigurationsFilter>
    {
    }
}
