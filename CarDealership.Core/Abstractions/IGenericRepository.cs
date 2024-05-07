using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions
{
    public interface IGenericRepository<M, F>
        where M : BaseModel
        where F : BaseFilter
    {

        Task<List<M>> GetAllAsync();

        Task<List<M>> GetFilteredAsync(F filter);
            
        Task<M> GetByIdAsync(Guid entityId);

        Task InsertAsync(M entity);

        Task UpdateAsync(M entity);

        Task DeleteAsync(Guid entityId);

        Task<bool> ExistsAsync(Guid entityId);
    }
}
