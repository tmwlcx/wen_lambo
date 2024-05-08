using AttorneyScheduler.DAL;
using Microsoft.AspNetCore.Mvc;

namespace AttorneyScheduler.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly AttorneySchedulerDbContext _context;

        public BaseController(AttorneySchedulerDbContext context)
        {
            _context = context;
        }
    }
}