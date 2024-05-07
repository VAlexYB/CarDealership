namespace CarDealership.DataAccess.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
