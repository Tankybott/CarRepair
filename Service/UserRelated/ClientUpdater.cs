using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DTOs.IntranetDto;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class ClientUpdater : IClientUpdater
    {
        private readonly IClientProfileRepository _clientProfileRepo;
        private readonly IMapper _mapper;

        public ClientUpdater(IClientProfileRepository clientProfileRepo, IMapper mapper)
        {
            _clientProfileRepo = clientProfileRepo;
            _mapper = mapper;
        }

        public async Task<IntranetClientReadDto> UpdateAsync(IntranetClientUpdateDto dto)
        {
            var profile = await _clientProfileRepo.GetAsync(cp => cp.Id == dto.Id, true, cp => cp.ApplicationUser);

            if (profile == null)
                throw new Exception($"Client with id {dto.Id} not found.");

            profile.Name = dto.Name;
            profile.Surname = dto.Surname;
            profile.PhoneNumber = dto.PhoneNumber;

            _clientProfileRepo.Update(profile);
            await _clientProfileRepo.SaveAsync();

            return _mapper.Map<IntranetClientReadDto>(profile);
        }
    }
}
