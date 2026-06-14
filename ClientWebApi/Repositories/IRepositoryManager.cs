namespace ClientWebApi.Repositories
{
    public interface IRepositoryManager
    {
       
        IUserTaskSummaryRepository UserTaskSummaryRepository { get; }
        ICommentRepository CommentRepository { get; }

        Task SaveAsync();
    }
}
