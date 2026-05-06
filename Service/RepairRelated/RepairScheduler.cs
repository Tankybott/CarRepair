using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class RepairScheduler : IRepairScheduler
    {
        private readonly IRepairBookingRepository _repairBookingRepository;
        private readonly IRepairStatusUpdater _repairStatusUpdater;
        private readonly IWorkTaskRepository _workTaskRepository;

        public RepairScheduler(
            IRepairBookingRepository repairBookingRepository,
            IRepairStatusUpdater repairStatusUpdater,
            IWorkTaskRepository workTaskRepository)
        {
            _repairBookingRepository = repairBookingRepository;
            _repairStatusUpdater = repairStatusUpdater;
            _workTaskRepository = workTaskRepository;
        }

        public async Task ScheduleAsync(IntranetRepairScheduleDto dto)
        {
            var tasks = (await _workTaskRepository.GetAllAsync(
                t => t.RepairId == dto.RepairId
                  && t.DeletedAt == null
                  && t.Status != Model.DomainModel.TaskStatus.Cancelled)).ToList();

            if (tasks.Any())
            {
                var minStart = tasks.Min(t => t.PredictedStart);
                var maxEnd = tasks.Max(t => t.PredicetedEnd);

                if (dto.CarDeliveryDateTime >= minStart)
                    throw new InvalidOperationException(
                        $"Car delivery must be before the earliest task start ({minStart:dd.MM.yyyy HH:mm}).");

                if (dto.CarPickupDateTime <= maxEnd)
                    throw new InvalidOperationException(
                        $"Car pickup must be after the latest task end ({maxEnd:dd.MM.yyyy HH:mm}).");
            }

            var existing = await _repairBookingRepository.GetAsync(
                b => b.RepairId == dto.RepairId && b.DeletedAt == null, tracked: true);

            if (existing != null)
            {
                existing.StartDateTime = dto.CarDeliveryDateTime;
                existing.EndDateTime = dto.CarPickupDateTime;
            }
            else
            {
                _repairBookingRepository.Add(new RepairBooking
                {
                    RepairId = dto.RepairId,
                    StartDateTime = dto.CarDeliveryDateTime,
                    EndDateTime = dto.CarPickupDateTime
                });
            }

            await _repairBookingRepository.SaveAsync();

            await _repairStatusUpdater.UpdateStatusAsync(dto.RepairId, RepairStatus.Scheduled);
        }
    }
}
