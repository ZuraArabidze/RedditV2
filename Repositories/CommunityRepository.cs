using Reddit.Models;
using System.Linq.Expressions;

namespace Reddit.Repositories
{
    public class CommunityRepository: ICommunityRepository
    {
        private readonly ApplicationDbContext _context;
        public CommunityRepository(ApplicationDbContext applcationDBContext)
        {
            _context = applcationDBContext;
        }
        public async Task<CommunityList<Community>> GetCommunities(int pageNumber, int pageSize, string? SearchKey, string? sortKey = null, bool isAscending = true)
        {
            var communities = _context.Communities.AsQueryable();
            // searching
            if (SearchKey != null)
            {
                communities = _context.Communities.Where(u => u.Name.Contains(SearchKey) || u.Description.Contains(SearchKey));
            }
            // sorting
            if (isAscending)
            {
                communities = communities.OrderBy(GetSortExpression(sortKey));
            }
            else
            {
                communities = communities.OrderByDescending(GetSortExpression(sortKey));
            }
            return await CommunityList<Community>.CreateAsync(communities, pageNumber, pageSize);
        }
        private Expression<Func<Community, object>> GetSortExpression(string? sortTerm)
        {
            sortTerm = sortTerm?.ToLower();
            return sortTerm switch
            {
                "createdat" => community => community.CreatedAt,
                "postscount" => community => community.Posts.Count,
                "subscriberscount" => community => community.Subscribers.Count,
                "id" => community => community.Id,
                _ => community => community.Id 
            };
        }
    }
}
