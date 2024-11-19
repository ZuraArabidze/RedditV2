using Reddit.Models;

namespace Reddit.Repositories
{
    public interface ICommunityRepository
    {
        public Task<CommunityList<Community>> GetCommunities(int pageNumber, int pageSize, string? SearchKey, string? sortKey = null, bool isAscending = true);
    }
}
