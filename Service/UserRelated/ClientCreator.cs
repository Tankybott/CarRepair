using AutoMapper;
using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class ClientCreator : IClientCreator
    {
        private readonly IClientProfileRepository _clientProfileRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ClientCreator> _logger;

        public ClientCreator(IClientProfileRepository clientProfileRepo, UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<ClientCreator> logger)
        {
            _clientProfileRepo = clientProfileRepo;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IntranetClientReadDto> CreateAsync(IntranetClientCreateDto dto)
        {
            var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user for client: {Errors}", errors);
                throw new InvalidOperationException(errors);
            }

            var profile = new ClientProfile
            {
                ApplicationUserId = user.Id,
                Name = dto.Name,
                Surname = dto.Surname,
                PhoneNumber = dto.PhoneNumber
            };

            _clientProfileRepo.Add(profile);
            await _clientProfileRepo.SaveAsync();

            var created = await _clientProfileRepo.GetAsync(cp => cp.Id == profile.Id, false, cp => cp.ApplicationUser);
            return _mapper.Map<IntranetClientReadDto>(created!);
        }
    }
}
