using DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using Model.DomainModel;
using Service.UserRelated.Interface;

namespace Service.UserRelated
{
    public class EmployeeDeleter : IEmployeeDeleter
    {
        private readonly IEmployeeProfileRepository _employeeRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeDeleter(IEmployeeProfileRepository employeeRepo, UserManager<ApplicationUser> userManager)
        {
            _employeeRepo = employeeRepo;
            _userManager = userManager;
        }

        public async Task DeleteAsync(int id)
        {
            var profile = await _employeeRepo.GetAsync(e => e.Id == id, true, e => e.ApplicationUser, e => e.Specializations);

            if (profile == null)
                throw new Exception($"Employee with id {id} not found.");

            var user = await _userManager.FindByIdAsync(profile.ApplicationUserId);
            if (user != null)
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }

            profile.Specializations.Clear();
            _employeeRepo.Remove(profile);
            await _employeeRepo.SaveAsync();
        }
    }
}
