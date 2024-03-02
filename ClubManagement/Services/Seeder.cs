using ClubManagement.Models;
using ClubManagement.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;

namespace ClubManagement.Services
{/*
    public class Seeder
    {
        private readonly ClubDbContext _dbContext;

        public Seeder(ClubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async void Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                if (!(await _dbContext.Roles.AnyAsync()))
                {
                    var roles = GetRoles();
                    await _dbContext.Roles.AddRangeAsync(roles);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "Footballer"
                },
                new Role()
                {
                    Name= "Couch"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };

            return roles;
        }


    }*/
}
