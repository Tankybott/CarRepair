using Model.DTOs.IntranetDto;

namespace Service.CarRelated.Interface
{
    public interface ICarCreator
    {
        Task<IntranetCarReadDto> CreateAsync(IntranetCarUpsertDto dto);
    }
}
