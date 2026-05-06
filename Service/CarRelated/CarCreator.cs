using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.CarRelated.Interface;

namespace Service.CarRelated
{
    public class CarCreator : ICarCreator
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public CarCreator(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<IntranetCarReadDto> CreateAsync(IntranetCarUpsertDto dto)
        {
            var entity = _mapper.Map<Car>(dto);
            _carRepository.Add(entity);
            await _carRepository.SaveAsync();

            var created = await _carRepository.GetAsync(c => c.Id == entity.Id, false, c => c.Client);
            return _mapper.Map<IntranetCarReadDto>(created!);
        }
    }
}
