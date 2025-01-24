using AttorneyScheduler.DAL.Tables;
using AttorneyScheduler.DTO;

namespace AttorneyScheduler.Services
{
    public interface IAttorneyService
    {
        Task<IEnumerable<AttorneyDto>> GetAttorneys();
        Task<AttorneyDto?> GetAttorney(int id);
        Task AddUpdateAttorney(AttorneyDto attorney);
        Task<IEnumerable<AttorneyType>> GetAttorneyTypes();
        Task<AttorneyTimeOffDto?> GetAttorneyTimeOff(int id);
        Task<AttorneyTimeOff> CreateAttorneyTimeOff(int attorneyId, DateTime timeOffDateFrom, DateTime timeOffDateTo);
        Task<AttorneyTimeOff?> UpdateAttorneyTimeOff(int id, AttorneyTimeOff attorneyTimeOff);
        Task<string> GenerateSchedule(int scheduleYear, int scheduleMonth, int numSlots);
    }
}
