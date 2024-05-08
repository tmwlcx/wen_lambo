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
    }
}
