using ChatApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DBContext _db;

        public AccountRepository(DBContext db)
        {
            _db = db;
        }

        public async Task<string> RegisterUser(User user)
        {
            // Check if a user with the same username already exists
            var usernameExist = _db.AvailableUsers
                .Where(u => u.username == user.username).FirstOrDefault();

            if(usernameExist != null) 
            {
                return "username already exist";
            }
            
            _db.User.Add(user);
            await _db.SaveChangesAsync();

            // Retrieve the user ID after it's been saved to the database
            int userId = user.id;

            // Get the role IDs from the database based on the role names
            var roleId = _db.Role.Where(r => r.rolename == "user").Select(r => r.id).FirstOrDefault();

            var userRole = new UserRoles { userId = userId, roleId = roleId };

            _db.UserRoles.Add(userRole);
            await _db.SaveChangesAsync();

            return "success";
        }

        public async Task<User?> Checkuser(string email, string password)
        {
            var user = _db.User.FirstOrDefault(u => u.email == email);

            return user != null ? user : null;
        }

        public async Task<string?> GetRole(int userId)
        {
            int roleId = _db.UserRoles.Where(u => u.userId == userId).Select(u => u.roleId).FirstOrDefault();
            string role = _db.Role.Where(r => r.id == roleId).Select(r => r.rolename).FirstOrDefault();
            return role;
        }
    }
}
