namespace CarDealership.Core.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; }
        public bool IsDeleted { get; set; }

        public BaseModel(Guid id)
        {
            Id = id;
        }
    }
}
