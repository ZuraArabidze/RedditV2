using Reddit.Models;

namespace Reddit.Repositories
{
    public interface IUserRepository
    {
        public Task<UserList<User>> GetPosts(int pageNumber, int pageSize, string? SearchKey, string? sortKey = null, bool isAscending = true);
    }
}