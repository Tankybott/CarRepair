using DataAccess.Repository.Interfaces;
using Service.CarRelated.Interface;
using Service.RepairRelated.Interface;

namespace Service.CarRelated
{
    public class CarDeleter : ICarDeleter
    {
        private readonly ICarRepository _carRepository;
        private readonly IRepairRepository _repairRepository;
        private readonly IRepairDeleter _repairDeleter;

        public CarDeleter(ICarRepository carRepository, IRepairRepository repairRepository, IRepairDeleter repairDeleter)
        {
            _carRepository = carRepository;
            _repairRepository = repairRepository;
            _repairDeleter = repairDeleter;
        }

        public async Task DeleteAsync(int id)
        {
            var repairs = await _repairRepository.GetAllAsync(r => r.CarId == id && r.DeletedAt == null);
            foreach (var repair in repairs)
                await _repairDeleter.DeleteAsync(repair.Id);

            var car = await _carRepository.GetAsync(c => c.Id == id, tracked: true);
            if (car == null)
                throw new Exception($"Car with id {id} not found.");

            car.DeletedAt = DateTime.UtcNow;
            await _carRepository.SaveAsync();
        }
    }
}
