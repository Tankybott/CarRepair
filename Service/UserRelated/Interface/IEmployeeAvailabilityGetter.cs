using Model.DTOs.IntranetDto;

namespace Service.UserRelated.Interface
{
    public interface IEmployeeAvailabilityGetter
    {
        Task<IEnumerable<IntranetAvailableEmployeeDto>> GetAvailableAsync(int serviceTypeId, DateTime start, DateTime end, int? excludeTaskId = null);
    }
}
