using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions
{
    public interface IGenericService<M, F>
        where M : BaseModel
        where F : BaseFilter
    {

        Task<List<M>> GetAllAsync();

        Task<List<M>> GetFilteredAsync(F filter);

        Task<M> GetByIdAsync(Guid entityId);

        Task CreateOrEditAsync(M entity);

        Task DeleteAsync(Guid entityId);
    }
}
