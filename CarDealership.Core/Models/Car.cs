
using CSharpFunctionalExtensions;

namespace CarDealership.Core.Models
{
    public class Car : BaseModel
    {
        public const int VIN_REQUIRED_LENGTH = 17;
        public string VIN { get; }
        public Guid AutoConfigurationId { get; }
        public AutoConfiguration? AutoConfiguration { get; }
        private Car(Guid id, string VIN, Guid autoConfigurationId, bool isDeleted, AutoConfiguration? autoConfiguration) : base(id)
        {
            this.VIN = VIN;
            AutoConfigurationId = autoConfigurationId;
            IsDeleted = isDeleted;
            AutoConfiguration = autoConfiguration;
        }

        public static Result<Car> Create(Guid id, string VIN, Guid autoConfigurationId, bool isDeleted, AutoConfiguration? autoConfiguration = null)
        {
            var error = string.Empty;

            if(string.IsNullOrWhiteSpace(VIN) || VIN.Length != VIN_REQUIRED_LENGTH)
            {
                error += $"Длина VIN должна быть {VIN_REQUIRED_LENGTH} символов. ";
            }

            if (autoConfigurationId == Guid.Empty)
            {
                error += $"AutoConfigurationId не должен быть пустым. ";
            }

            if (!string.IsNullOrEmpty(error))
            {
                return Result.Failure<Car>(error.Trim());
            }

            var car = new Car(id, VIN, autoConfigurationId, isDeleted, autoConfiguration);
            return Result.Success(car);
        }
    }
}
