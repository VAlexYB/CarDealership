using CSharpFunctionalExtensions;
using Newtonsoft.Json;

namespace CarDealership.Core.Models
{
    public class EngineType : BaseModel
    {
        public const int MAX_VALUE_LENGTH = 30;
        public string Value { get; }

        private readonly List<Engine> engines = new List<Engine>();

        public IReadOnlyCollection<Engine> Engines => engines.AsReadOnly();
        
        [JsonConstructor]
        public EngineType(Guid id, string value, bool isDeleted) : base(id)
        {
            Value = value;
            IsDeleted = isDeleted;
        }

        public void AddEngine(Engine engine)
        {
            if(engine == null) throw new ArgumentNullException(nameof(engine));
            engines.Add(engine);
        }
        
        public static Result<EngineType> Create(Guid id, string value, bool isDeleted = false)
        {
            string error = string.Empty;

            if(string.IsNullOrWhiteSpace(value) || value.Length > MAX_VALUE_LENGTH)
            {
                error += $"Тип двигателя не может быть пустым или иметь более {MAX_VALUE_LENGTH} символов. ";
            }

            if(!string.IsNullOrEmpty(error))
            {
                return Result.Failure<EngineType>(error.Trim());
            }

            var engineType = new EngineType(id, value, isDeleted);
            return Result.Success(engineType);
        }
    }
}
