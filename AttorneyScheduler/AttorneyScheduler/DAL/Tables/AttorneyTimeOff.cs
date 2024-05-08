namespace AttorneyScheduler.DAL.Tables
{
    public class AttorneyTimeOff
    {
        public int AttorneyTimeOffId { get; set; }
        public int AttorneyId { get; set; }
        public DateTime TimeOffDateFrom { get; set; }
        public DateTime TimeOffDateTo { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public virtual Attorney? Attorney { get; set; }
    }


}
