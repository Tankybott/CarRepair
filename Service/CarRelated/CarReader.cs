using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Model.DTOs.PortalDto;
using Service.CarRelated.Interface;

namespace Service.CarRelated
{
    public class CarReadService : ICarReader
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public CarReadService(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IntranetCarReadDto>> GetAllForIndex()
        {
            var cars = await _carRepository.GetAllAsync(null, false, c => c.Client);
            return _mapper.Map<IEnumerable<Car>, IEnumerable<IntranetCarReadDto>>(cars);
        }

        public async Task<IEnumerable<PortalCarDto>> GetCarsForUser(string userId)
        {
            var cars = await _carRepository.GetAllAsync(c => c.Client.ApplicationUserId == userId);
            return _mapper.Map<IEnumerable<Car>, IEnumerable<PortalCarDto>>(cars);
        }

        public async Task<IEnumerable<CarAndRepairCarDto>> GetCarsDetailForUser(string userId)
        {
            var cars = await _carRepository.GetAllAsync(c => c.Client.ApplicationUserId == userId);
            return _mapper.Map<IEnumerable<Car>, IEnumerable<CarAndRepairCarDto>>(cars);
        }

        public async Task<CarAndRepairCarDto?> GetCarForUser(int carId, string userId)
        {
            var car = await _carRepository.GetAsync(c => c.Id == carId && c.Client.ApplicationUserId == userId);
            return car is null ? null : _mapper.Map<CarAndRepairCarDto>(car);
        }
    }
}
