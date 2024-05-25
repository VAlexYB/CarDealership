﻿namespace CarDealership.DataAccess.Entities
{
    public class BodyTypeEntity : BaseEntity
    {
        public string Value { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public List<AutoConfigurationEntity> Configurations { get; set; } = [];
    }
}
