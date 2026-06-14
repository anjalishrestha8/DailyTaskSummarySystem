using ClientMVC.ApiResponse;
using ClientMVC.Dto.RequestDto;
using ClientWebApi.Dto.RequestDto;
using ClientMVC.Dto.ResponseDto;
using ClientMVC.Common;

namespace ClientMVC.Services
{
    public interface IUserTaskSummaryService
    {
        Task<ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>> GetAllSummariesAsync();
        Task<ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>> GetAllPagedSummariesAsync(int pageNumber, int pageSize,string? searchTerm = null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>> GetSummariesByUserIdAsync(string userId);
        Task<ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>> GetPagedSummariesByUserIdAsync(string userId, int pageNumber,int pageSize, string? searchTerm = null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDate = null);
        Task<ApiResponse<IEnumerable<CommentResponseDto>>> GetCommentsByIdAsync(Guid commentId);

        Task<ApiResponse<UserTaskSummaryResponseDto>> GetSummaryDetailsByIdAsync(Guid summaryId);
        Task<ApiResponse<string>> AddUserTaskSummaryAsync(AddUserTaskSummaryDto summaryDto);
        Task<ApiResponse<string>> UpdateUserTaskSummaryAsync(Guid summaryId, UpdateUserTaskSummaryDto summaryDto);

        Task<ApiResponse<string>> AddCommentAsync(CommentRequestDto commentDto);
        Task<ApiResponse<string>> UpdateCommentAsync(Guid commentId, CommentRequestDto commentDto);




    }
}
