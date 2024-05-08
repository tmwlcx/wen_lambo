﻿using AttorneyScheduler.DAL.Tables;

namespace AttorneyScheduler.Services
{
    public interface IAttorneyService
    {
        Task<IEnumerable<Attorney>> GetAttorneys();
        Task<Attorney?> GetAttorney(int id);
        Task<Attorney> CreateAttorney(Attorney attorney);
        Task<IEnumerable<AttorneyType>> GetAttorneyTypes();
    }
}
