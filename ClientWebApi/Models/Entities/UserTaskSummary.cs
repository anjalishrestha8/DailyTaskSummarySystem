using ClientWebApi.Common.Enums;
using ClientWebApi.Models.Identity;
using System.ComponentModel.DataAnnotations;

namespace ClientWebApi.Models.Entities
{
    public class UserTaskSummary:EntityBase<Guid>
    {
        public string UserId { get; set; } = null!;
        public ApplicationUser? User { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        [Range(1, 3, ErrorMessage = "Invalid status value.")]
        public TaskStatusEnum Status { get; set; }
        public ICollection<Comments> Comments { get; set; } = new List<Comments>();
    }
}
