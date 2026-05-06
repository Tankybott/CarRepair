using Model.DTOs.IntranetDto;

namespace Service.RepairRelated.Interface
{
    public interface ICostEstimationCreator
    {
        Task CreateAsync(IntranetEstimateCostsDto dto);
    }
}
