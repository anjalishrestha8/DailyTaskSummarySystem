using ClientWebApi.Data;
using ClientWebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClientWebApi.Repositories
{
    public class CommentRepository:GenericRepository<Comments,Guid>, ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CommentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Comments>> GetCommentsByIdAsync(Guid commentId)
        {
            return await _dbContext.Comments
                .Include(c => c.User)
                .Include(c => c.UserTaskSummary)
                .Where(c => c.Id == commentId)
                .ToListAsync();
        }

    }
}
