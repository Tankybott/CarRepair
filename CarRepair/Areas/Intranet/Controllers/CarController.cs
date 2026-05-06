using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.CarRelated.Interface;
using Service.UserRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    public class CarController : Controller
    {
        private readonly ICarCreator _carCreator;
        private readonly ICarUpdater _carUpdater;
        private readonly ICarDeleter _carDeleter;
        private readonly ICarReader _carReader;
        private readonly IClientReader _clientReader;

        public CarController(ICarCreator carCreator, ICarUpdater carUpdater, ICarDeleter carDeleter, ICarReader carReader, IClientReader clientReader)
        {
            _carCreator = carCreator;
            _carUpdater = carUpdater;
            _carDeleter = carDeleter;
            _carReader = carReader;
            _clientReader = clientReader;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var cars = await _carReader.GetAllForIndex();
                var clients = await _clientReader.GetAllForIndex();

                return View(new IntranetCarViewModel
                {
                    Cars = cars,
                    Clients = clients
                });
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        [HttpPost]
        [Route("api/car/upsert")]
        public async Task<IActionResult> Upsert([FromBody] IntranetCarUpsertDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var saved = dto.Id == 0
                ? await _carCreator.CreateAsync(dto)
                : await _carUpdater.UpdateAsync(dto);

            return Ok(new
            {
                success = true,
                message = dto.Id == 0 ? "Created successfully." : "Updated successfully.",
                data = saved
            });
        }

        [HttpDelete]
        [Route("api/car/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _carDeleter.DeleteAsync(id);
            return Ok(new { success = true, message = "Deleted successfully." });
        }
    }
}
