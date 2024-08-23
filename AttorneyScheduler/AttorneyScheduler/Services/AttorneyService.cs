using AttorneyScheduler.DAL;
using AttorneyScheduler.DAL.Tables;
using AttorneyScheduler.DTO;
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

        public async Task <IEnumerable<AttorneyDto>> GetAttorneys()
        {
            //return await _context.Attorney.ToListAsync();
            var results = await _context.Attorney
                .Include(a => a.AttorneyType)
                .Select(x => new AttorneyDto
                {
                    AttorneyId = x.AttorneyId,
                    AttorneyName = x.AttorneyName,
                    AttorneyTypeId = x.AttorneyTypeId,
                    AttorneyTypeName = x.AttorneyType.TypeName
                }).ToListAsync();

            return results;
        }

        public async Task<AttorneyDto?> GetAttorney(int id)
        {
            var result = await _context.Attorney
                .Include(a => a.AttorneyType)
                .Where(x => x.AttorneyId == id)
                .Select(x => new AttorneyDto
                {
                    AttorneyId = x.AttorneyId,
                    AttorneyName = x.AttorneyName,
                    AttorneyTypeId = x.AttorneyTypeId,
                    AttorneyTypeName = x.AttorneyType.TypeName
                }).FirstOrDefaultAsync();

            return result;
        }


        public async Task<Attorney> PostAttorney(Attorney attorney)
        {
            return new Attorney { };
        }

        public async Task AddUpdateAttorney(AttorneyDto attorney)
        {

            if (attorney.AttorneyId == null)
            {
                var newAttorney = new Attorney
                {
                    AttorneyName = attorney.AttorneyName,
                    AttorneyTypeId = attorney.AttorneyTypeId
                };

                _context.Attorney.Add(newAttorney);
                await _context.SaveChangesAsync();
            }
            else
            {
                var updatedAttorney = await _context.Attorney
                    .FirstOrDefaultAsync(x => x.AttorneyId == attorney.AttorneyId);

                if (updatedAttorney != null)
                {
                    updatedAttorney.AttorneyName = attorney.AttorneyName;
                    updatedAttorney.AttorneyTypeId = attorney.AttorneyTypeId;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentException("Invalid AttorneyId");
                }
            }
        }


        public async Task<IEnumerable<AttorneyType>> GetAttorneyTypes()
        {
            return await _context.AttorneyType.ToListAsync();
        }

        public async Task<AttorneyTimeOffDto?> GetAttorneyTimeOff(int id)
        {
            var result = await _context.AttorneyTimeOff
                .Where(x => x.AttorneyTimeOffId == id)
                .Select(x => new AttorneyTimeOffDto
                {
                    AttoneryId = x.AttorneyId,
                    AttorneyName = x.Attorney.AttorneyName,
                    TimeOffDateFrom = x.TimeOffDateFrom,
                    TimeOffDateTo = x.TimeOffDateTo
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<AttorneyTimeOff> CreateAttorneyTimeOff(int attorneyId, DateTime timeOffDateFrom, DateTime timeOffDateTo)
        {
            var attorneyTimeOff = new AttorneyTimeOff
            {
                AttorneyId = attorneyId,
                TimeOffDateFrom = timeOffDateFrom,
                TimeOffDateTo = timeOffDateTo,
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

