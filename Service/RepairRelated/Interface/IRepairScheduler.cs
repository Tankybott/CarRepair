using Model.DTOs.IntranetDto;

namespace Service.RepairRelated.Interface
{
    public interface IRepairScheduler
    {
        Task ScheduleAsync(IntranetRepairScheduleDto dto);
    }
}
