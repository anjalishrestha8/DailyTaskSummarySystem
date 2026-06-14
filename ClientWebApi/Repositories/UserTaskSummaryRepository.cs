using ClientWebApi.Common;
using ClientWebApi.Data;
using ClientWebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ClientWebApi.Repositories
{
    public class UserTaskSummaryRepository : GenericRepository<UserTaskSummary, Guid>, IUserTaskSummaryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserTaskSummaryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> HasSummaryForTodayAsync(string userId)
        {
            var today = DateTime.UtcNow.Date;
            return await _dbContext.UserTaskSummary
                .AnyAsync(s => s.UserId == userId && s.CreatedAt.Date == today);
        }

        public async Task<bool> CanSummaryBeUpdatedAsync(Guid summaryId)
        {
            var today = DateTime.UtcNow.Date;
            var summary = await _dbContext.UserTaskSummary
                .FirstOrDefaultAsync(s => s.Id == summaryId);
            if (summary == null)
            {
                return false;
            }
            if (summary.CreatedAt.Date == today)
            {
                return true;
            }
            return false;

        }

        public override async Task<IEnumerable<UserTaskSummary>> GetAllAsync()
        {
            return await _dbContext.UserTaskSummary
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }


        public async Task<IEnumerable<UserTaskSummary>> GetSummariesByUserIdAsync(string userId)
        {
            return await _dbContext.UserTaskSummary
                .Where(s => s.UserId == userId)
                .Include(s => s.User)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<UserTaskSummary?> GetSummaryWithCommentsAsync(Guid summaryId)
        {
            return await _dbContext.UserTaskSummary
                                .Include(s => s.User)
                .Include(s => s.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(s => s.Id == summaryId);
        }

        public async Task<PagedList<UserTaskSummary>> GetAllPagedSummariesAsync(int pageNumber, int pageSize, string? searchTerm = null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _dbContext.UserTaskSummary
                .Include(s => s.User)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Title.Contains(searchTerm) || s.Description.Contains(searchTerm) || s.User!.UserName!.Contains(searchTerm));
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "title":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(s => s.Title) : query.OrderBy(s => s.Title);
                        break;
                    case "username":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(s => s.User!.UserName) : query.OrderBy(s => s.User!.UserName);
                        break;
                    case "status":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status);
                        break;
                    default:
                        query = query.OrderByDescending(s => s.CreatedAt);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(s => s.CreatedAt);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(s => s.CreatedAt.Date >= fromDate.Value.Date);
            }
            if (toDate.HasValue)
            {
                if (!fromDate.HasValue)
                {
                    var earliestDate = await _dbContext.UserTaskSummary.MinAsync(s => s.CreatedAt);
                    fromDate = earliestDate.Date;
                    query = query.Where(s => s.CreatedAt.Date >= fromDate.Value.Date);
                }
                query = query.Where(s => s.CreatedAt.Date <= toDate.Value.Date);

            }
            //query = query.OrderByDescending(s => s.CreatedAt);
            return await PagedList<UserTaskSummary>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<PagedList<UserTaskSummary>> GetPagedSummariesByUserIdAsync(string userId, int pageNumber, int pageSize, string? searchTerm = null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _dbContext.UserTaskSummary
                .Where(s => s.UserId == userId)
                .Include(s => s.User)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(s => s.Title.Contains(searchTerm) || s.Description.Contains(searchTerm));
            }
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "title":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(s => s.Title) : query.OrderBy(s => s.Title);
                        break;
                    case "username":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(s => s.User!.UserName) : query.OrderBy(s => s.User!.UserName);
                        break;
                    case "status":
                        query = sortOrder?.ToLower() == "desc" ? query.OrderByDescending(s => s.Status) : query.OrderBy(s => s.Status);
                        break;
                    default:
                        query = query.OrderByDescending(s => s.CreatedAt);
                        break;
                }
            }
            else
            {
                query = query.OrderByDescending(s => s.CreatedAt);
            }

            if (fromDate.HasValue)
            {
                query = query.Where(s => s.CreatedAt.Date >= fromDate.Value.Date);
            }
            if (toDate.HasValue)
            {
                if (!fromDate.HasValue)
                {
                    var earliestDate = await _dbContext.UserTaskSummary.MinAsync(s => s.CreatedAt);
                    fromDate = earliestDate.Date;
                    query = query.Where(s => s.CreatedAt.Date >= fromDate.Value.Date);
                }
                query = query.Where(s => s.CreatedAt.Date <= toDate.Value.Date);

            }
            //query = query.OrderByDescending(s => s.CreatedAt);
            return await PagedList<UserTaskSummary>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
