using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.DomainModel;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class ClientDeleter : IClientDeleter
    {
        private readonly IClientProfileRepository _clientProfileRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICarRepository _carRepository;

        public ClientDeleter(IClientProfileRepository clientProfileRepo, UserManager<ApplicationUser> userManager, ICarRepository carRepository)
        {
            _clientProfileRepo = clientProfileRepo;
            _userManager = userManager;
            _carRepository = carRepository;
        }

        public async Task DeleteAsync(int clientProfileId)
        {
            var profile = await _clientProfileRepo.GetAsync(cp => cp.Id == clientProfileId, true, cp => cp.ApplicationUser);

            if (profile == null)
                throw new Exception($"Client with id {clientProfileId} not found.");

            var clientCars = await _carRepository.GetAllAsync(c => c.ClientId == profile.Id);
            if (clientCars != null && clientCars.Any())
                _carRepository.RemoveRange(clientCars);

            _clientProfileRepo.Remove(profile);
            await _clientProfileRepo.SaveAsync();

            var user = await _userManager.FindByIdAsync(profile.ApplicationUserId!);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }
    }
}
