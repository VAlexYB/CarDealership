using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;

namespace CarDealership.Application.Services
{
    public class BaseService<M, F> : IGenericService<M, F>
        where M : BaseModel
        where F : BaseFilter
    {

        protected readonly IGenericRepository<M, F> _repository;
        public BaseService(IGenericRepository<M, F> repository) {
            _repository = repository;
        }

        public async Task<List<M>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<M>> GetFilteredAsync(F filter)
        {
            return await _repository.GetFilteredAsync(filter);
        }

        public async Task<M> GetByIdAsync(Guid entityId)
        {
            return await _repository.GetByIdAsync(entityId);
        }

        public virtual async Task<Guid> CreateOrEditAsync(M model)
        {
            bool _exist = await _repository.ExistsAsync(model.Id);
            Guid id = Guid.Empty;
            if (_exist)
            {
                id = await _repository.UpdateAsync(model);
            }
            else
            {
                id = await _repository.InsertAsync(model);
            }
            return id;
        }

        public async Task<Guid> DeleteAsync(Guid entityId)
        {
            return await _repository.DeleteAsync(entityId);
        }
    }
}
