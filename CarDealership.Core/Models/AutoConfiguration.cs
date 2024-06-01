
using CSharpFunctionalExtensions;
using System.Text;

namespace CarDealership.Core.Models
{
    public class AutoConfiguration : BaseModel
    {
        public decimal Price { get; }

        public Guid AutoModelId { get; }
        public AutoModel? AutoModel { get; }

        public Guid BodyTypeId { get; }
        public BodyType? BodyType { get; }

        public Guid DriveTypeId { get; }
        public DriveType? DriveType { get; }

        public Guid EngineId { get; }
        public Engine? Engine { get; }

        public Guid ColorId { get; }
        public Color? Color { get; }

        private readonly List<Car> cars = new List<Car>();
        public IReadOnlyCollection<Car> Cars => cars.AsReadOnly();

        private AutoConfiguration(Guid id, decimal price, Guid autoModelId,  Guid bodyTypeId,  Guid driveTypeId, Guid engineId, Guid colorId,
            bool isDeleted, AutoModel? autoModel, BodyType? bodyType, DriveType? driveType, Engine? engine, Color? color) : base(id)
        {
            Price = price;
            AutoModelId = autoModelId;
            BodyTypeId = bodyTypeId;
            DriveTypeId = driveTypeId;
            EngineId = engineId;
            ColorId = colorId;
            IsDeleted = isDeleted;
            AutoModel = autoModel;
            BodyType = bodyType;
            DriveType = driveType;
            Engine = engine;
            Color = color;
        }

        public void AddCar(Car car)
        {
            if(car == null) throw new ArgumentNullException(nameof(car));
            cars.Add(car);
        }

        public static Result<AutoConfiguration> Create(Guid id, decimal price, Guid autoModelId, Guid bodyTypeId, Guid driveTypeId, Guid engineId, Guid colorId,
            bool isDeleted = false, AutoModel? autoModel = null, BodyType? bodyType = null, DriveType? driveType = null, Engine? engine = null, Color? color = null)
        {
            var errorBuilder = new StringBuilder();

            if(price < 0)
            {
                errorBuilder.Append("Цена конфигурации не может быть отрицательной. ");
            }

            if(autoModelId == Guid.Empty)
            {
                errorBuilder.Append("AutoModelId не должен быть пустым. ");
            }

            if(bodyTypeId == Guid.Empty)
            {
                errorBuilder.Append("BodyTypeId не должен быть пустым. ");
            }

            if(driveTypeId == Guid.Empty)
            {
                errorBuilder.Append("DriveTypeId не должен быть пустым. ");
            }

            if(engineId == Guid.Empty)
            {
                errorBuilder.Append("EngineId не должен быть пустым. ");
            }

            if(colorId == Guid.Empty)
            {
                errorBuilder.Append("ColorId не должен быть пустым. ");
            }

            if (errorBuilder.Length > 0)
            {
                return Result.Failure<AutoConfiguration>(errorBuilder.ToString().Trim());
            }

            var autoConfiguration = new AutoConfiguration(id, price, autoModelId, bodyTypeId, driveTypeId, engineId, colorId,
             isDeleted, autoModel, bodyType, driveType, engine, color);

            return Result.Success(autoConfiguration);
        }
    }
}
