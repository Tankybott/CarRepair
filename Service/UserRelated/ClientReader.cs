using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class ClientReadService : IClientReader
    {
        private readonly IClientProfileRepository _clientProfileRepo;
        private readonly IMapper _mapper;

        public ClientReadService(IClientProfileRepository clientProfileRepo, IMapper mapper)
        {
            _clientProfileRepo = clientProfileRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IntranetClientReadDto>> GetAllForIndex()
        {
            var profiles = await _clientProfileRepo.GetAllAsync(null, false, cp => cp.ApplicationUser);
            return _mapper.Map<IEnumerable<ClientProfile>, IEnumerable<IntranetClientReadDto>>(profiles);
        }
    }
}
