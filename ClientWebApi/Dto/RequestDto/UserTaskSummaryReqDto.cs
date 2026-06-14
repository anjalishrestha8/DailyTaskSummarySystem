using ClientWebApi.Common.Enums;

namespace ClientWebApi.Dto.RequestDto
{
    public class UserTaskSummaryReqDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public TaskStatusEnum Status { get; set; }
    }

    public class AddUserTaskSummaryDto : UserTaskSummaryReqDto
    {
        public string UserId { get; set; } = null!;
    }

    public class UpdateUserTaskSummaryDto : UserTaskSummaryReqDto
    {
        public Guid Id { get; set; }

    }
}
