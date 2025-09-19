using Accessio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Accessio.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<Accessio.Models.Locker> Locker { get; set; } = default!;
        public DbSet<Accessio.Models.GatePass> GatePass { get; set; } = default!;
    }
}
