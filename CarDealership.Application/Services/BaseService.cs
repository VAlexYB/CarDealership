using CarDealership.Core.Abstractions;
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

        public async Task CreateOrEditAsync(M model)
        {
            bool _exist = await _repository.ExistsAsync(model.Id);
            if (_exist)
            {
                await _repository.UpdateAsync(model);
            }
            else
            {
                await _repository.InsertAsync(model);
            }
        }

        public async Task DeleteAsync(Guid entityId)
        {
            await _repository.DeleteAsync(entityId);
        }
    }
}
