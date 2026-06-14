using ClientWebApi.Models.Identity;

namespace ClientWebApi.Models.Entities
{
    public class Comments:EntityBase<Guid>
    {
        public string UserId { get; set; } = null!;
        public Guid UserTaskSummaryId { get; set; }
        public string Content { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
        public virtual UserTaskSummary? UserTaskSummary { get; set; }
    }
}
