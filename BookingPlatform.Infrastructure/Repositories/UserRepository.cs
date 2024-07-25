using BookingPlatform.Application.Interfaces;
using BookingPlatform.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingPlatform.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        internal readonly UserManager<User> _userManager;


        public UserRepository(BookingPlatformDbContext dbContext, UserManager<User> userManager) : base(dbContext) 
        {
            _userManager = userManager;
        }
        

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
    }
}
