using Model.DTOs.IntranetDto;

namespace Service.CarRelated.Interface
{
    public interface ICarUpdater
    {
        Task<IntranetCarReadDto> UpdateAsync(IntranetCarUpsertDto dto);
    }
}
