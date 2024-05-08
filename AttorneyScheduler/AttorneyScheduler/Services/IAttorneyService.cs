using AttorneyScheduler.DAL.Tables;

namespace AttorneyScheduler.Services
{
    public interface IAttorneyService
    {
        Task<IEnumerable<Attorney>> GetAttorneys();
        Task<Attorney?> GetAttorney(int id);
        Task<Attorney> CreateAttorney(Attorney attorney);
        Task<IEnumerable<AttorneyType>> GetAttorneyTypes();
        Task<AttorneyTimeOff?> GetAttorneyTimeOff(int id);
        Task<AttorneyTimeOff> CreateAttorneyTimeOff(int attorneyId, DateTime timeOffDateFrom, DateTime timeOffDateTo);
        Task<AttorneyTimeOff?> UpdateAttorneyTimeOff(int id, AttorneyTimeOff attorneyTimeOff);
    }
}
