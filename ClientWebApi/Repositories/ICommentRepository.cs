using ClientWebApi.Models.Entities;

namespace ClientWebApi.Repositories
{
    public interface ICommentRepository:IGenericRepository<Comments,Guid>
    {
        Task<IEnumerable<Comments>> GetCommentsByIdAsync(Guid commentId);
    }
}
