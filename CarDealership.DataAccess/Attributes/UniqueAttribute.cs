namespace CarDealership.DataAccess.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class UniqueAttribute : Attribute
    {
        public string[] PropertyNames { get; }

        public UniqueAttribute(params string[] propertyNames)
        {
            PropertyNames = propertyNames;
        }
    }
}
