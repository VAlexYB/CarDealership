using CarDealership.Core.Abstractions.Repositories;
using CarDealership.Core.Abstractions.Services;
using CarDealership.Core.Models;
using Microsoft.EntityFrameworkCore;


namespace CarDealership.Application.Services
{
    public class CarsService : BaseService<Car, BaseFilter>, ICarsService
    {
        private readonly ICarsRepository _carsRepository;
        private readonly IDealsRepository _dealsRepository;
        public CarsService(ICarsRepository carsRepository, IDealsRepository dealsRepository) : base(carsRepository)
        {
            _carsRepository = carsRepository;
            _dealsRepository = dealsRepository;
        }

        public async Task<List<Car>> GetFreeCars()
        {
            HashSet<Guid> reservedCars = (await _dealsRepository.GetAllAsync())
                .Select(d => d.CarId)
                .ToHashSet();

            return (await _carsRepository.GetAllAsync())
                .Where(c => !reservedCars.Contains(c.Id))
                .ToList();
        }
    }
}
