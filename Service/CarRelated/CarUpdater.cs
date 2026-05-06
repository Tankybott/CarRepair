using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Service.CarRelated.Interface;

namespace Service.CarRelated
{
    public class CarUpdater : ICarUpdater
    {
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;

        public CarUpdater(ICarRepository carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task<IntranetCarReadDto> UpdateAsync(IntranetCarUpsertDto dto)
        {
            var entity = await _carRepository.GetAsync(c => c.Id == dto.Id, true, c => c.Client);

            if (entity == null)
                throw new Exception($"Car with id {dto.Id} not found.");

            _mapper.Map(dto, entity);

            _carRepository.Update(entity);
            await _carRepository.SaveAsync();

            var updated = await _carRepository.GetAsync(c => c.Id == entity.Id, false, c => c.Client);
            return _mapper.Map<IntranetCarReadDto>(updated!);
        }
    }
}
