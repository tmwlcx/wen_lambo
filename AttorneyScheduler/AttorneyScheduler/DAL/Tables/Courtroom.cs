namespace AttorneyScheduler.DAL.Tables
{
    public class Courtroom
    {
        public int CourtRoomId { get; set; }
        public string CourtRoomNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
