using ClientWebApi.Data;

namespace ClientWebApi.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;

        public RepositoryManager(ApplicationDbContext context)
        {
            _context = context;
        }

       

        public IUserTaskSummaryRepository UserTaskSummaryRepository => new UserTaskSummaryRepository(_context);
        public ICommentRepository CommentRepository => new CommentRepository(_context);
        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
