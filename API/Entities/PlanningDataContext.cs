using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entities
{
    public class PlanningDataContext : IdentityDbContext<PlanningUser>
    {
        private IConfiguration _config;
        bool _testing = false;

        public PlanningDataContext(DbContextOptions options,
            IConfiguration config,
            bool testing = false)
            : base(options)
        {
            _config = config;
            _testing = testing;
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectPlanning> Plannings { get; set; }
        //public DbSet<PlanningUser> PlanningUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!_testing)
            {
                base.OnConfiguring(optionsBuilder);

                string conString = _config.GetSection("ConnectionStrings:DefaultConnection").Value;
                optionsBuilder.UseSqlServer(conString);
            }
        }
    }

}
