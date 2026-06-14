using ClientWebApi.Common;
using ClientWebApi.Models.Entities;

namespace ClientWebApi.Repositories
{
    public interface IUserTaskSummaryRepository : IGenericRepository<UserTaskSummary, Guid>
    {
        Task<IEnumerable<UserTaskSummary>> GetSummariesByUserIdAsync(string userId);
        Task<UserTaskSummary?> GetSummaryWithCommentsAsync(Guid summaryId);
        Task<bool> HasSummaryForTodayAsync(string userId);
        Task<bool>CanSummaryBeUpdatedAsync(Guid summaryId);
        Task<PagedList<UserTaskSummary>> GetAllPagedSummariesAsync(int pageNumber, int pageSize, string? searchTerm = null, string? sortBy= null, string? sortOrder = null,DateTime? fromDate = null, DateTime? toDatetime = null);
        Task<PagedList<UserTaskSummary>> GetPagedSummariesByUserIdAsync(string userId, int pageNumber, int pageSize, string? searchTerm = null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDatetime = null);
    }
}
 