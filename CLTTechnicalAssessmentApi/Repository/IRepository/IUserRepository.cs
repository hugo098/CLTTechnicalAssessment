﻿using CLTTechnicalAssessmentApi.Models;

namespace CLTTechnicalAssessmentApi.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> UpdateAsync(User entity);
    }
}
