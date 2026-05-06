using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.UserRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    public class ClientController : Controller
    {
        private readonly IClientCreator _clientCreator;
        private readonly IClientUpdater _clientUpdater;
        private readonly IClientDeleter _clientDeleter;
        private readonly IClientReader _clientReader;

        public ClientController(IClientCreator clientCreator, IClientUpdater clientUpdater, IClientDeleter clientDeleter, IClientReader clientReader)
        {
            _clientCreator = clientCreator;
            _clientUpdater = clientUpdater;
            _clientDeleter = clientDeleter;
            _clientReader = clientReader;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var clients = await _clientReader.GetAllForIndex();
                return View(new IntranetClientViewModel { Clients = clients });
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        [HttpPost]
        [Route("api/client/create")]
        public async Task<IActionResult> Create([FromBody] IntranetClientCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            try
            {
                var saved = await _clientCreator.CreateAsync(dto);
                return Ok(new { success = true, message = "Created successfully.", data = saved });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/client/update")]
        public async Task<IActionResult> Update([FromBody] IntranetClientUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var saved = await _clientUpdater.UpdateAsync(dto);
            return Ok(new { success = true, message = "Updated successfully.", data = saved });
        }

        [HttpDelete]
        [Route("api/client/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientDeleter.DeleteAsync(id);
            return Ok(new { success = true, message = "Deleted successfully." });
        }
    }
}
