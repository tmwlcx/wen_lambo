using AttorneyScheduler.DAL;
using AttorneyScheduler.DAL.Tables;
using Microsoft.EntityFrameworkCore;

namespace AttorneyScheduler.Services
{
    public class AttorneyService : IAttorneyService
    {
        private readonly AttorneySchedulerDbContext _context;

        public AttorneyService(AttorneySchedulerDbContext context)
        {
            _context = context;
        }

        public async Task <IEnumerable<Attorney>> GetAttorneys()
        {
            return await _context.Attorney.ToListAsync();
        }

        public async Task<Attorney?> GetAttorney(int id)
        {
            var attorney = await _context.Attorney.FindAsync(id);
            if (attorney == null)
            {
                return null;
            }
            return attorney;
        }

        public async Task<Attorney> CreateAttorney(Attorney attorney)
        {
            _context.Add(attorney);
            await _context.SaveChangesAsync();
            return attorney;
        }

        public async Task<IEnumerable<AttorneyType>> GetAttorneyTypes()
        {
            return await _context.AttorneyType.ToListAsync();
        }

        public async Task<AttorneyTimeOff?> GetAttorneyTimeOff(int id)
        {
            return await _context.AttorneyTimeOff.FindAsync(id);
        }

        public async Task<AttorneyTimeOff> CreateAttorneyTimeOff(int attorneyId, DateTime timeOffDateFrom, DateTime timeOffDateTo)
        {
            var attorneyTimeOff = new AttorneyTimeOff
            {
                AttorneyId = attorneyId,
                TimeOffDateFrom = timeOffDateFrom,
                TimeOffDateTo = timeOffDateTo,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            _context.Add(attorneyTimeOff);
            await _context.SaveChangesAsync();

            return attorneyTimeOff;
        }

        public async Task<AttorneyTimeOff?> UpdateAttorneyTimeOff(int id, AttorneyTimeOff attorneyTimeOff)
        {
            if (!AttorneyTimeOffExists(id))
            {
                return null;
            }

            attorneyTimeOff.AttorneyTimeOffId = id;
            await _context.SaveChangesAsync();
            return attorneyTimeOff;
        }

        private bool AttorneyTimeOffExists(int id)
        {
            return _context.AttorneyTimeOff.Any(e => e.AttorneyTimeOffId == id);
        }
    }
}

