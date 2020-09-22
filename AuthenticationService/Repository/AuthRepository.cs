using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Repository
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e AuthRepository by inheriting IAuthRepository class 
    //which is used to implement all methods in the classs.
    public class AuthRepository: IAuthRepository
    {
        //Define a private variable to represent AuthDbContext
        readonly AuthDbContext dbContext;

        public AuthRepository(AuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool CreateUser(User user)
        {
            dbContext.Users.Add(user);
            return dbContext.SaveChanges() > 0;
        }

        public bool IsUserExists(string userId)
        {
            return dbContext.Users.Any(u => string.Equals(userId, u.UserId));
        }

        public bool LoginUser(User user)
        {
            return dbContext.Users.Any(u => string.Equals(u.UserId, user.UserId)
            && string.Equals(u.Password, user.Password));
        }
    }
}
