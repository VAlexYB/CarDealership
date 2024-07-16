using CarDealership.Core.Models;

namespace CarDealership.Core.Abstractions.Repositories
{
    public interface IGenericRepository<M, F>
        where M : BaseModel
        where F : BaseFilter
    {

        Task<List<M>> GetAllAsync();

        Task<List<M>> GetFilteredAsync(F filter);

        Task<M> GetByIdAsync(Guid entityId);

        Task<Guid> InsertAsync(M entity);

        Task<Guid> UpdateAsync(M entity);

        Task<Guid> DeleteAsync(Guid entityId);

        Task<bool> ExistsAsync(Guid entityId);
    }
}
