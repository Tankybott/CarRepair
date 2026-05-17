using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.DomainModel;
using Service.CarRelated.Interface;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class ClientDeleter : IClientDeleter
    {
        private readonly IClientProfileRepository _clientProfileRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICarRepository _carRepository;
        private readonly ICarDeleter _carDeleter;

        public ClientDeleter(
            IClientProfileRepository clientProfileRepo,
            UserManager<ApplicationUser> userManager,
            ICarRepository carRepository,
            ICarDeleter carDeleter)
        {
            _clientProfileRepo = clientProfileRepo;
            _userManager = userManager;
            _carRepository = carRepository;
            _carDeleter = carDeleter;
        }

        public async Task DeleteAsync(int clientProfileId)
        {
            var profile = await _clientProfileRepo.GetAsync(cp => cp.Id == clientProfileId, tracked: true, cp => cp.ApplicationUser);

            if (profile == null)
                throw new Exception($"Client with id {clientProfileId} not found.");

            var cars = await _carRepository.GetAllAsync(c => c.ClientId == profile.Id && c.DeletedAt == null);
            foreach (var car in cars)
                await _carDeleter.DeleteAsync(car.Id);

            profile.DeletedAt = DateTime.UtcNow;
            await _clientProfileRepo.SaveAsync();

            var user = await _userManager.FindByIdAsync(profile.ApplicationUserId!);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }
    }
}
