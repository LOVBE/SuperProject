using EMT_API.Interfaces.IUserRepository;
using EMT_API.Models;

namespace EMT_API.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Account?> AssignRoleAsync(int userId, string newRole)
        {
            return await _userRepository.AssignRoleAsync(userId, newRole);
        }

        public async Task<Account> CreateAccountAsync(Account acc)
        {
            return await _userRepository.CreateAccountAsync(acc);
        }

        public async Task<Teacher> CreateTeacherAsync(int accountId, string? desc = null, string? cert = null)
        {
            return await _userRepository.CreateTeacherAsync(accountId, desc, cert);
        }

        public async Task<UserDetail> CreateUserDetailAsync(int accountId)
        {
            return await _userRepository.CreateUserDetailAsync(accountId);
        }

        public async Task<List<Account>> GetAllStudentsAsync()
        {
            return await _userRepository.GetAllStudentsAsync();
        }

        public async Task<List<Account>> GetAllTeachersAsync()
        {
            return await _userRepository.GetAllTeachersAsync();
        }

        public async Task<List<Account>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<Account?> GetByEmailOrUsernameAsync(string emailOrUsername)
        {
            return await _userRepository.GetByEmailOrUsernameAsync(emailOrUsername);
        }

        public async Task<Account?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _userRepository.IsEmailExistsAsync(email);
        }

        public async Task<bool> IsUsernameExistsAsync(string username)
        {
            return await _userRepository.IsUsernameExistsAsync(username);
        }

        public async Task<Account?> LockUserAsync(int id)
        {
            return await _userRepository.LockUserAsync(id);
        }

        public async Task<List<Account>> SearchUsersAsync(string? keyword, string? role, string? status)
        {
            return await _userRepository.SearchUsersAsync(keyword, role, status);
        }

        public async Task<Account?> UnlockUserAsync(int id)
        {
            return await _userRepository.UnlockUserAsync(id);
        }

        public async Task UpdateAccountAsync(Account acc) => await _userRepository.UpdateAccountAsync(acc);
    }
}
