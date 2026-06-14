using ClientWebApi.Common.Enums;

namespace ClientWebApi.Dto.ResponseDto
{
    public class UserTaskSummaryResponseDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskStatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public IEnumerable<CommentResponseDto> Comments { get; set; } = new List<CommentResponseDto>();
    }
}
