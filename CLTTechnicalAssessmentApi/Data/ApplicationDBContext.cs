using CLTTechnicalAssessmentApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CLTTechnicalAssessmentApi.Data
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        { 
        }

        public DbSet<User> Users { get; set; }
    }
}
