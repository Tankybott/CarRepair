using AutoMapper;
using DataAccess.Repository.Interfaces;
using Model.DomainModel;
using Model.DTOs.IntranetDto;
using Service.WorkTaskRelated.Interface;

namespace Service.WorkTaskRelated
{
    public class WorkTaskCreator : IWorkTaskCreator
    {
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IEmployeeProfileRepository _employeeProfileRepository;
        private readonly IEmployeeBookingRepository _employeeBookingRepository;
        private readonly IMapper _mapper;

        public WorkTaskCreator(
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

        public async Task<IntranetWorkTaskReadDto> CreateAsync(IntranetWorkTaskCreateDto dto)
        {
            var task = new WorkTask
            {
                RepairId = dto.RepairId,
                ServiceId = dto.ServiceId,
                Description = dto.Description,
                PredictedStart = dto.PredictedStart,
                PredicetedEnd = dto.PredictedEnd,
                Status = Model.DomainModel.TaskStatus.Assigned
            };

            if (dto.AssignedEmployeeIds.Any())
            {
                var employees = await _employeeProfileRepository.GetAllAsync(
                    e => dto.AssignedEmployeeIds.Contains(e.Id), tracked: true);
                foreach (var emp in employees)
                    task.EmployeesAssigned.Add(emp);
            }

            _workTaskRepository.Add(task);
            await _workTaskRepository.SaveAsync();

            foreach (var empId in dto.AssignedEmployeeIds)
            {
                _employeeBookingRepository.Add(new EmployeeBooking
                {
                    EmployeeProfileId = empId,
                    TaskId = task.Id,
                    BookingType = BookingType.Task,
                    StartDateTime = dto.PredictedStart,
                    EndDateTime = dto.PredictedEnd
                });
            }

            if (dto.AssignedEmployeeIds.Any())
                await _employeeBookingRepository.SaveAsync();

            return _mapper.Map<IntranetWorkTaskReadDto>(task);
        }
    }
}
