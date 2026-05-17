using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTOs.IntranetDto;
using Model.ViewModel;
using Service.PartRelated.Interface;

namespace CarRepair.Areas.Intranet.Controllers
{
    [Area("Intranet")]
    [Authorize(Roles = "Admin,Manager,Employee")]
    public class PartController : Controller
    {
        private readonly IPartCreator _partCreator;
        private readonly IPartUpdater _partUpdater;
        private readonly IPartDeleter _partDeleter;
        private readonly IPartStatusChanger _partStatusChanger;
        private readonly IPartReader _partReader;

        public PartController(IPartCreator partCreator, IPartUpdater partUpdater, IPartDeleter partDeleter, IPartStatusChanger partStatusChanger, IPartReader partReader)
        {
            _partCreator = partCreator;
            _partUpdater = partUpdater;
            _partDeleter = partDeleter;
            _partStatusChanger = partStatusChanger;
            _partReader = partReader;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var parts = await _partReader.GetAllForIndex();
                return View(new IntranetPartIndexVM { Items = parts });
            }
            catch
            {
                return RedirectToAction("Index", "Error", new { area = "Portal" });
            }
        }

        [HttpPost]
        [Route("api/part/upsert")]
        public async Task<IActionResult> Upsert([FromBody] IntranetPartUpsertDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    error = "Invalid data submitted.",
                    validation = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var saved = dto.Id == 0
                ? await _partCreator.CreateAsync(dto)
                : await _partUpdater.UpdateAsync(dto);

            return Ok(new
            {
                success = true,
                message = dto.Id == 0 ? "Created successfully." : "Updated successfully.",
                data = saved
            });
        }

        [HttpPut]
        [Route("api/part/status/{id:int}")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] IntranetPartStatusDto dto)
        {
            var saved = await _partStatusChanger.ChangeStatusAsync(id, dto.Status);

            return Ok(new
            {
                success = true,
                message = "Status updated successfully.",
                data = saved
            });
        }

        [HttpDelete]
        [Route("api/part/delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _partDeleter.DeleteAsync(id);
            return Ok(new { success = true, message = "Deleted successfully." });
        }
    }
}
