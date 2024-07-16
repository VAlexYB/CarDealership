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
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<M>> GetFilteredAsync(F filter)
        {
            try
            {
                return await _repository.GetFilteredAsync(filter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<M> GetByIdAsync(Guid entityId)
        {
            try
            {
                return await _repository.GetByIdAsync(entityId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<Guid> CreateOrEditAsync(M model)
        {
            try
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
            catch
            {
                throw;
            }
        }

        public async Task<Guid> DeleteAsync(Guid entityId)
        {
            try
            {
                return await _repository.DeleteAsync(entityId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
