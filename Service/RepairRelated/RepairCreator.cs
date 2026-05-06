using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class RepairCreator : IRepairCreator
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public RepairCreator(IRepairRepository repairRepository, IServiceRepository serviceRepository, IMapper mapper)
        {
            _repairRepository = repairRepository;
            _serviceRepository = serviceRepository;
            _mapper = mapper;
        }

        public async Task<IntranetRepairReadDto> CreateAsync(IntranetRepairCreateDto dto)
        {
            var repair = new Repair
            {
                CarId = dto.CarId,
                ClientDescription = dto.ClientDescription,
                Status = RepairStatus.Pending
            };

            if (dto.ServiceIds.Any())
            {
                var services = await _serviceRepository.GetAllAsync(s => dto.ServiceIds.Contains(s.Id), true);
                foreach (var service in services)
                    repair.ServicesSelected.Add(service);
            }

            _repairRepository.Add(repair);
            await _repairRepository.SaveAsync();

            var created = await _repairRepository.GetWithDetailsAsync(repair.Id);
            return _mapper.Map<IntranetRepairReadDto>(created!);
        }
    }
}
