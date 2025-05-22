using Microsoft.EntityFrameworkCore;
using WebApiBasicAuthentication.Models;

namespace WebApiBasicAuthentication.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;
        public UserService(UserDbContext context)
        {
            _context = context;
        }
        // User CRUD Operations
        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            // Note: In production, passwords should be hashed and salted.
            return await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
        // Role Management
        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await _context.Roles.AsNoTracking().ToListAsync();
        }
        public async Task AssignRoleToUserAsync(int userId, string roleName)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new ArgumentException("User not found");
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            if (role == null)
                throw new ArgumentException("Role not found");
            if (!user.UserRoles.Any(ur => ur.RoleId == role.Id))
            {
                user.UserRoles.Add(new UserRole { UserId = userId, RoleId = role.Id });
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
            return user?.UserRoles.Select(ur => ur.Role) ?? Enumerable.Empty<Role>();
        }
    }
}
