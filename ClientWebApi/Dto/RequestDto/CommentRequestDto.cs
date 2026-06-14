namespace ClientWebApi.Dto.RequestDto
{
    public class CommentRequestDto
    {
        public string UserId { get; set; } = null!;
        public Guid UserTaskSummaryId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
