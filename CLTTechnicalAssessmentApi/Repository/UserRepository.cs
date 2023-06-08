using CLTTechnicalAssessmentApi.Data;
using CLTTechnicalAssessmentApi.Models;
using CLTTechnicalAssessmentApi.Repository.IRepository;

namespace CLTTechnicalAssessmentApi.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ApplicationDBContext _db;
        public UserRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _db.Users.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
