namespace CarDealership.Core.Models
{
    public class BaseFilter
    {
        public DateTime? Start {  get; set; }
        public DateTime? End { get; set; }
        public HashSet<Guid>? Guids { get; set; }
    }
}
