using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Service.WorkTaskRelated.Interface;

namespace Service.WorkTaskRelated
{
    public class WorkTaskDeleter : IWorkTaskDeleter
    {
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IEmployeeBookingRepository _employeeBookingRepository;

        public WorkTaskDeleter(IWorkTaskRepository workTaskRepository, IEmployeeBookingRepository employeeBookingRepository)
        {
            _workTaskRepository = workTaskRepository;
            _employeeBookingRepository = employeeBookingRepository;
        }

        public async Task DeleteAsync(int id)
        {
            var bookings = await _employeeBookingRepository.GetAllAsync(b => b.TaskId == id, tracked: true);
            foreach (var b in bookings)
                _employeeBookingRepository.Remove(b);

            var task = await _workTaskRepository.GetAsync(t => t.Id == id, tracked: true);
            if (task != null)
                _workTaskRepository.Remove(task);

            await _workTaskRepository.SaveAsync();
        }

        public async Task CancelAsync(int id)
        {
            var task = await _workTaskRepository.GetAsync(t => t.Id == id, tracked: true);
            if (task == null) return;
            task.Status = Model.DomainModel.TaskStatus.Cancelled;
            await _workTaskRepository.SaveAsync();
        }
    }
}
