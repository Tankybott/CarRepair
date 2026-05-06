using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.WorkTaskRelated.Interface;

namespace Service.WorkTaskRelated
{
    public class WorkTaskUpdater : IWorkTaskUpdater
    {
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IEmployeeProfileRepository _employeeProfileRepository;
        private readonly IEmployeeBookingRepository _employeeBookingRepository;
        private readonly IMapper _mapper;

        public WorkTaskUpdater(
            IWorkTaskRepository workTaskRepository,
            IEmployeeProfileRepository employeeProfileRepository,
            IEmployeeBookingRepository employeeBookingRepository,
            IMapper mapper)
        {
            _workTaskRepository = workTaskRepository;
            _employeeProfileRepository = employeeProfileRepository;
            _employeeBookingRepository = employeeBookingRepository;
            _mapper = mapper;
        }

        public async Task<IntranetWorkTaskReadDto> UpdateAsync(IntranetWorkTaskUpdateDto dto)
        {
            var task = await _workTaskRepository.GetTrackedWithEmployeesAsync(dto.Id);
            if (task == null) throw new InvalidOperationException("Task not found.");

            task.Description = dto.Description;
            task.PredictedStart = dto.PredictedStart;
            task.PredicetedEnd = dto.PredictedEnd;

            task.EmployeesAssigned.Clear();
            if (dto.AssignedEmployeeIds.Any())
            {
                var employees = await _employeeProfileRepository.GetAllAsync(
                    e => dto.AssignedEmployeeIds.Contains(e.Id), tracked: true);
                foreach (var emp in employees)
                    task.EmployeesAssigned.Add(emp);
            }

            var oldBookings = await _employeeBookingRepository.GetAllAsync(
                b => b.TaskId == dto.Id, tracked: true);
            foreach (var b in oldBookings)
                _employeeBookingRepository.Remove(b);

            foreach (var empId in dto.AssignedEmployeeIds)
            {
                _employeeBookingRepository.Add(new EmployeeBooking
                {
                    EmployeeProfileId = empId,
                    TaskId = dto.Id,
                    BookingType = BookingType.Task,
                    StartDateTime = dto.PredictedStart,
                    EndDateTime = dto.PredictedEnd
                });
            }

            await _workTaskRepository.SaveAsync();

            return _mapper.Map<IntranetWorkTaskReadDto>(task);
        }
    }
}
