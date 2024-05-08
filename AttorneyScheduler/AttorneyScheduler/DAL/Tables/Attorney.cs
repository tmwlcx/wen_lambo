namespace AttorneyScheduler.DAL.Tables
{
    public class Attorney
    {
        public int AttorneyId { get; set; }
        public string AttorneyName { get; set; }
        public int AttorneyTypeId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public AttorneyType AttorneyType { get; set; }
        public virtual ICollection<AttorneyTimeOff>? AttorneyTimeOff { get; set; }
    }
}
