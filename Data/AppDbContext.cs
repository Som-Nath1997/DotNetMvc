using CustomPageApp.Models;
using CustomPageApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomPageApp.Data
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base (options)
        {
            
        }

        public DbSet<AppUser> appUsers { get; set; }  
        public DbSet<LoginVm> loginVms { get; set; }  
        public DbSet<RegisterVm> registerVm { get; set; }
        public DbSet<EmpModel> Employees { get; set; }


    }
}
