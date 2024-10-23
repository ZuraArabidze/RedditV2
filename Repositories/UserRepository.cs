using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Reddit.Models;
using System.Linq.Expressions;

namespace Reddit.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext applcationDBContext)
        {
            _context = applcationDBContext;
        }
        public async Task<UserList<User>> GetPosts(int pageNumber, int pageSize, string? SearchKey, string? sortKey = null, bool isAscending = true)
        {
            var users = _context.Users.AsQueryable();

            // searching
            if (SearchKey != null)
            {
                users = _context.Users.Where(u => u.Name.Contains(SearchKey) || u.Email.Contains(SearchKey));
            }

            // sorting

            if (isAscending)
            {
                users = users.OrderBy(GetSortExpression(sortKey));
            }
            else
            {
                users = users.OrderByDescending(GetSortExpression(sortKey));
            }

            return await UserList<User>.CreateAsync(users, pageNumber, pageSize);
        }


        private Expression<Func<User, object>> GetSortExpression(string? sortTerm)
        {
            sortTerm = sortTerm?.ToLower();
            return sortTerm switch
            {
                "numbeerOfPosts" => user => user.Posts.Count,
                "id" => user => user.Id,
                _ => user => user.Id
            };
        }
    }
}
