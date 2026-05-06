using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
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
    }
}
