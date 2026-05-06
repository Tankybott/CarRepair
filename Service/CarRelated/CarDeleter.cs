using DataAccess.Repository.Interfaces;
using Service.CarRelated.Interface;

namespace Service.CarRelated
{
    public class CarDeleter : ICarDeleter
    {
        private readonly ICarRepository _carRepository;

        public CarDeleter(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _carRepository.GetAsync(c => c.Id == id);

            if (entity == null)
                throw new Exception($"Car with id {id} not found.");

            _carRepository.Remove(entity);
            await _carRepository.SaveAsync();
        }
    }
}
