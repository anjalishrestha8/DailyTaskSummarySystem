namespace ClientMVC.Dto.ResponseDto
{
    public class CommentResponseDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public Guid UserTaskSummaryId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string UserName { get; set; } = null!;
    }
}
