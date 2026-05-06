using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.PartRelated.Interface;
using Service.RepairRelated.Interface;

namespace Service.RepairRelated
{
    public class CostEstimationCreator : ICostEstimationCreator
    {
        private readonly IRepairRepository _repairRepository;
        private readonly ICostEstimationItemRepository _costEstimationItemRepository;
        private readonly IPartCreator _partCreator;

        public CostEstimationCreator(
            IRepairRepository repairRepository,
            ICostEstimationItemRepository costEstimationItemRepository,
            IPartCreator partCreator)
        {
            _repairRepository = repairRepository;
            _costEstimationItemRepository = costEstimationItemRepository;
            _partCreator = partCreator;
        }

        public async Task CreateAsync(IntranetEstimateCostsDto dto)
        {
            foreach (var svc in dto.ServiceCosts)
            {
                _costEstimationItemRepository.Add(new CostEstimationItem
                {
                    RepairId = dto.RepairId,
                    Type = CostEstimationItemType.Service,
                    Name = svc.ServiceName,
                    Description = string.Empty,
                    Cost = svc.Cost
                });
            }

            foreach (var part in dto.Parts)
            {
                await _partCreator.CreateAsync(new IntranetPartUpsertDto
                {
                    RepairId = dto.RepairId,
                    Name = part.Name,
                    Manufacturer = part.Manufacturer,
                    SerialNumber = part.SerialNumber,
                    Description = part.Description,
                    UnitPrice = part.UnitPrice,
                    Quantity = part.Quantity
                });

                _costEstimationItemRepository.Add(new CostEstimationItem
                {
                    RepairId = dto.RepairId,
                    Type = CostEstimationItemType.Part,
                    Name = part.Name,
                    Description = part.Description ?? string.Empty,
                    Cost = part.UnitPrice * part.Quantity
                });
            }

            foreach (var other in dto.OtherCosts)
            {
                _costEstimationItemRepository.Add(new CostEstimationItem
                {
                    RepairId = dto.RepairId,
                    Type = CostEstimationItemType.Other,
                    Name = other.Name,
                    Description = other.Description,
                    Cost = other.Cost
                });
            }

            var repair = await _repairRepository.GetAsync(r => r.Id == dto.RepairId, tracked: true);
            if (repair != null)
            {
                repair.PredictedPrice = dto.ServiceCosts.Sum(s => s.Cost)
                                      + dto.Parts.Sum(p => p.UnitPrice * p.Quantity)
                                      + dto.OtherCosts.Sum(o => o.Cost);
                repair.Status = RepairStatus.ManagerReviewed;
            }

            await _costEstimationItemRepository.SaveAsync();
        }
    }
}
