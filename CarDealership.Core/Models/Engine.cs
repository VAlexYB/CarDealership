using CSharpFunctionalExtensions;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CarDealership.Core.Models
{
    public class Engine : BaseModel
    {
        public int Power { get; } //лошадиные силы
        public int Consumption { get; } //эффективная мощность
        public decimal Price { get; }

        public Guid EngineTypeId { get; }
        public EngineType? EngineType { get; }

        public Guid TransmissionTypeId { get; }
        public TransmissionType? TransmissionType { get; }

        private readonly List<AutoConfiguration> configurations = new List<AutoConfiguration>();
        public IReadOnlyCollection<AutoConfiguration> Configurations => configurations.AsReadOnly();

        [JsonConstructor]
        private Engine(Guid id, int power, int consumption, decimal price, Guid engineTypeId, 
             Guid transmissionTypeId, bool isDeleted, EngineType? engineType, TransmissionType? transmissionType) : base(id)
        {
            Power = power;
            Consumption = consumption;
            Price = price;
            EngineTypeId = engineTypeId;
            EngineType = engineType;
            TransmissionTypeId = transmissionTypeId;
            TransmissionType = transmissionType;
            IsDeleted = isDeleted;
        }

        public void AddConfiguration(AutoConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configurations.Add(configuration);
        }

        public static Result<Engine> Create(Guid id, int power, int consumption, decimal price, Guid engineTypeId,
             Guid transmissionTypeId, bool isDeleted = false, EngineType? engineType = null, TransmissionType? transmissionType = null)
        {
            var errorBuilder = new StringBuilder();

            if (power < 0 || power > 5000)
            {
                errorBuilder.Append("Мощность может быть в пределах от 0 до 5000 лошадиных сил. ");
            }

            //if (consumption < )
            //{

            //}
            if (price < 0)
            {
                errorBuilder.Append("Цена двигателя не может быть отрицательной. ");
            }

            if (engineTypeId == Guid.Empty)
            {
                errorBuilder.Append("EngineTypeId не должен быть пустым. ");
            }

            if (transmissionTypeId == Guid.Empty)
            {
                errorBuilder.Append("TransmissionTypeId не должен быть пустым. ");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<Engine>(errorBuilder.ToString().Trim());
            }

            var engine = new Engine(id, power, consumption, price, engineTypeId, transmissionTypeId, isDeleted, engineType, transmissionType);
            return Result.Success(engine);
        }

    }
}
