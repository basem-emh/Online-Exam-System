using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.DAL.Entities.Identity;

namespace OnlineExamSystem.DAL.Persistence.Data.Contexts
{
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) 
            : base(options)
        {
        }
    }
}
