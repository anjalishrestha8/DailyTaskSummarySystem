using ClientMVC.ApiResponse;
using ClientMVC.Common;
using ClientMVC.Dto.RequestDto;
using ClientMVC.Dto.ResponseDto;
using ClientWebApi.Dto.RequestDto;

namespace ClientMVC.Services
{
    public class UserTaskSummaryService : IUserTaskSummaryService
    {
        private readonly IApiService _apiService;
        public UserTaskSummaryService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>> GetAllSummariesAsync()
        {
            return await _apiService.GetAsync<ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>>("UserTaskSummary/GetAllSummaries")
                 ?? new ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>
                 {
                     Success = false,
                     Message = "Failed to retrieve task summaries",
                     Data = new List<UserTaskSummaryResponseDto>()
                 };
        }
        public async Task<ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>> GetAllPagedSummariesAsync(
            int pageNumber, int pageSize, string? searchTerm = null,string ? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            string url = $"UserTaskSummary/GetAllPagedSummaries?pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(searchTerm))
                url += $"&searchTerm={searchTerm}";
            if (!string.IsNullOrEmpty(sortBy))
                url += $"&sortBy={sortBy}";
            if (!string.IsNullOrEmpty(sortOrder))
                url += $"&sortOrder={sortOrder}";
            if (fromDate.HasValue)
                url += $"&fromDate={fromDate.Value:yyyy-MM-dd}";
            if (toDate.HasValue)
                url += $"&toDate={toDate.Value:yyyy-MM-dd}";

            return await _apiService.GetAsync<ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>>(url)
                ?? new ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve paged task summaries",
                    Data = null
                };
        }


        public async Task<ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>> GetSummariesByUserIdAsync(string userId)
        {
            return await _apiService.GetAsync<ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>>($"UserTaskSummary/GetSummariesByUserId?userId={userId}")
                 ?? new ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>
                 {
                     Success = false,
                     Message = "Failed to retrieve user task summaries",
                     Data = new List<UserTaskSummaryResponseDto>()
                 };
        }
        public async Task<ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>> GetPagedSummariesByUserIdAsync(string userId, int pageNumber, int pageSize, string? searchTerm = null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            string url = $"UserTaskSummary/GetPagedSummariesByUserId?userId={userId}&pageNumber={pageNumber}&pageSize={pageSize}";

            if (!string.IsNullOrEmpty(searchTerm))
                url += $"&searchTerm={searchTerm}";
            if (!string.IsNullOrEmpty(sortBy))
                url += $"&sortBy={sortBy}";
            if (!string.IsNullOrEmpty(sortOrder))
                url += $"&sortOrder={sortOrder}";
            if (fromDate.HasValue)
                url += $"&fromDate={fromDate.Value:yyyy-MM-dd}";
            if (toDate.HasValue)
                url += $"&toDate={toDate.Value:yyyy-MM-dd}";

            return await _apiService.GetAsync<ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>>(url)
                ?? new ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve paged task summaries",
                    Data = null
                };
        }

        public async Task<ApiResponse<string>> AddUserTaskSummaryAsync(AddUserTaskSummaryDto summaryDto)
        {
            return await _apiService.PostAsync<AddUserTaskSummaryDto, ApiResponse<string>>("UserTaskSummary/AddSummary", summaryDto)
                 ?? new ApiResponse<string>
                 {
                     Success = false,
                     Message = "Failed to add user task summary",
                     Data = null
                 };
        }

        public async Task<ApiResponse<string>> UpdateUserTaskSummaryAsync(Guid summaryId, UpdateUserTaskSummaryDto summaryDto)
        {
            return await _apiService.PutAsync<UserTaskSummaryReqDto, ApiResponse<string>>($"UserTaskSummary/UpdateSummary?summaryId={summaryId}", summaryDto)
                 ?? new ApiResponse<string>
                 {
                     Success = false,
                     Message = "Failed to update user task summary",
                     Data = null
                 };
        }

        public async Task<ApiResponse<string>> AddCommentAsync(CommentRequestDto commentDto)
        {
            return await _apiService.PostAsync<CommentRequestDto, ApiResponse<string>>("UserTaskSummary/AddComment", commentDto)
                 ?? new ApiResponse<string>
                 {
                     Success = false,
                     Message = "Failed to add comment",
                     Data = null
                 };
        }

        public async Task<ApiResponse<string>> UpdateCommentAsync(Guid commentId, CommentRequestDto commentDto)
        {
            return await _apiService.PutAsync<CommentRequestDto, ApiResponse<string>>($"UserTaskSummary/UpdateComment?commentId={commentId}", commentDto)
                 ?? new ApiResponse<string>
                 {
                     Success = false,
                     Message = "Failed to update comment",
                     Data = null
                 };
        }

        public async Task<ApiResponse<UserTaskSummaryResponseDto>> GetSummaryDetailsByIdAsync(Guid summaryId)
        {
            return await _apiService.GetAsync<ApiResponse<UserTaskSummaryResponseDto>>(
                $"UserTaskSummary/GetSummaryDetailsById?id={summaryId}")
                ?? new ApiResponse<UserTaskSummaryResponseDto>
                {
                    Success = false,
                    Message = "Failed to retrieve task summary details",
                    Data = null
                };
        }

        public async Task<ApiResponse<IEnumerable<CommentResponseDto>>> GetCommentsByIdAsync(Guid commentId)
        {
            return await _apiService.GetAsync<ApiResponse<IEnumerable<CommentResponseDto>>>(
    $"UserTaskSummary/GetCommentsById?commentId={commentId}")
                ?? new ApiResponse<IEnumerable<CommentResponseDto>>
                {
                    Success = false,
                    Message = "Failed to retrieve comments",
                    Data = null
                };
        }
    }
}
