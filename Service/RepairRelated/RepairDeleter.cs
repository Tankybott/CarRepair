using DataAccess.Repository.Interfaces;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class RepairDeleter : IRepairDeleter
    {
        private readonly IRepairRepository _repairRepository;
        private readonly IPartRepository _partRepository;
        private readonly ICostEstimationItemRepository _costEstimationItemRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IEmployeeBookingRepository _employeeBookingRepository;
        private readonly IRepairBookingRepository _repairBookingRepository;

        public RepairDeleter(
            IRepairRepository repairRepository,
            IPartRepository partRepository,
            ICostEstimationItemRepository costEstimationItemRepository,
            IWorkTaskRepository workTaskRepository,
            IEmployeeBookingRepository employeeBookingRepository,
            IRepairBookingRepository repairBookingRepository)
        {
            _repairRepository = repairRepository;
            _partRepository = partRepository;
            _costEstimationItemRepository = costEstimationItemRepository;
            _workTaskRepository = workTaskRepository;
            _employeeBookingRepository = employeeBookingRepository;
            _repairBookingRepository = repairBookingRepository;
        }

        public async Task DeleteAsync(int repairId)
        {
            var now = DateTime.UtcNow;

            var parts = await _partRepository.GetAllAsync(p => p.RepairId == repairId && p.DeletedAt == null, tracked: true);
            foreach (var p in parts)
                p.DeletedAt = now;

            var costItems = await _costEstimationItemRepository.GetAllAsync(c => c.RepairId == repairId && c.DeletedAt == null, tracked: true);
            foreach (var c in costItems)
                c.DeletedAt = now;

            var tasks = await _workTaskRepository.GetAllAsync(t => t.RepairId == repairId && t.DeletedAt == null, tracked: true);
            foreach (var task in tasks)
            {
                var bookings = await _employeeBookingRepository.GetAllAsync(b => b.TaskId == task.Id && b.DeletedAt == null, tracked: true);
                foreach (var b in bookings)
                    b.DeletedAt = now;

                task.DeletedAt = now;
            }

            var repairBooking = await _repairBookingRepository.GetAsync(rb => rb.RepairId == repairId && rb.DeletedAt == null, tracked: true);
            if (repairBooking != null)
                repairBooking.DeletedAt = now;

            var repair = await _repairRepository.GetAsync(r => r.Id == repairId && r.DeletedAt == null, tracked: true);
            if (repair != null)
                repair.DeletedAt = now;

            await _repairRepository.SaveAsync();
        }
    }
}
