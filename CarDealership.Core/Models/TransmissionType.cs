
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;

namespace CarDealership.Core.Models
{
    public class TransmissionType : BaseModel
    {
        public const int MAX_VALUE_LENGTH = 25;
        public string Value { get; }

        private readonly List<Engine> engines = new List<Engine>();

        public IReadOnlyCollection<Engine> Engines => engines.AsReadOnly();
        
        [JsonConstructor]
        public TransmissionType(Guid id, string value, bool isDeleted) : base(id)
        {
            Value = value;
            IsDeleted = isDeleted;
        }

        public void AddEngine(Engine engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));
            engines.Add(engine);
        }

        public static Result<TransmissionType> Create(Guid id, string value, bool isDeleted = false)
        {
            var error = string.Empty;

            if(string.IsNullOrWhiteSpace(value) || value.Length > MAX_VALUE_LENGTH)
            {
                error += $"Тип трансмиссии не может быть пустым или иметь более {MAX_VALUE_LENGTH} символов. ";
            }
            var transmissionType = new TransmissionType(id, value, isDeleted);
            return Result.Success(transmissionType);
        }
    }
}
