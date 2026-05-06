namespace Model.DTOs.IntranetDto
{
    public class IntranetRepairScheduleDto
    {
        public int RepairId { get; set; }
        public DateTime CarDeliveryDateTime { get; set; }
        public DateTime CarPickupDateTime { get; set; }
    }
}
