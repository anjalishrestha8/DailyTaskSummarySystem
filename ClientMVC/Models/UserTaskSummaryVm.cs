using ClientMVC.Common.Enums;

namespace ClientMVC.Models
{
    public class UserTaskSummaryVm
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; } 
        public string? UserName { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskStatusEnum Status { get; set; } 
        public List<CommentsVm> Comments { get; set; } = new List<CommentsVm>();
        public DateTime CreatedAt { get; set; }
    }
}
