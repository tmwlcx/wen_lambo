namespace AttorneyScheduler.DTO
{
    public class AttorneyTimeOffDto
    {
        public int AttoneryId { get; set; }
        public string? AttorneyName { get; set; }
        public DateTime TimeOffDateFrom { get; set; }
        public DateTime TimeOffDateTo { get; set; }
    }
}
