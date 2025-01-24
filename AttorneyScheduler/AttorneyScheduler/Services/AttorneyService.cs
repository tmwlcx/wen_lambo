using AttorneyScheduler.DAL;
using AttorneyScheduler.DAL.Tables;
using AttorneyScheduler.DTO;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AttorneyScheduler.Services
{
    public class AttorneyService : IAttorneyService
    {
        private readonly AttorneySchedulerDbContext _context;
        private readonly IConfiguration _configuration;

        public AttorneyService(AttorneySchedulerDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<AttorneyDto>> GetAttorneys()
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

        public async Task<string> GenerateSchedule(int scheduleYear, int scheduleMonth, int numSlots)
        {
            if (scheduleMonth < 1 || scheduleMonth > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(scheduleMonth), "The schedule month must be between 1 and 12.");
            }
            var pythonSettings = _configuration.GetSection("Python").Get<PythonSettings>();

            string condaPath = pythonSettings.ExecutablePath;
            string scriptPath = pythonSettings.ScriptPath;

            if (!File.Exists(condaPath))
            {
                throw new InvalidOperationException($"conda.exe not found at {condaPath}");
            }

            if (!File.Exists(scriptPath))
            {
                throw new InvalidOperationException($"Script not found at {scriptPath}");
            }

            var attorneyIds = await _context.Attorney
                .Select(x => x.AttorneyId)
                .ToListAsync();

            var jrAttorneyIds = await _context.Attorney
                .Where(x => x.AttorneyTypeId == 3)
                .Select(x => x.AttorneyId)
                .ToListAsync();

            string attorneyIdsString = string.Join(",", attorneyIds);
            string jrAttorneyIdsString = string.Join(",", jrAttorneyIds);

            // TODO: add courtroom

            // build the command-line arguments to pass to the Python script
            string arguments = $"{scheduleYear} {scheduleMonth} {numSlots} {attorneyIdsString} {jrAttorneyIdsString}";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = condaPath,
                Arguments = $"run -n opt python {scriptPath} {arguments}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = Process.Start(psi);
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start the Python process.");
            }

            // read the output and error streams
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
            {
                throw new InvalidOperationException(error);
            }

            // just returning the JSON string
            return output;
        }

        private bool AttorneyTimeOffExists(int id)
        {
            return _context.AttorneyTimeOff.Any(e => e.AttorneyTimeOffId == id);
        }
    }
}

