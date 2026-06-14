using AutoMapper;
using ClientWebApi.ApiResponse;
using ClientWebApi.Common;
using ClientWebApi.Dto.RequestDto;
using ClientWebApi.Dto.ResponseDto;
using ClientWebApi.Models.Entities;
using ClientWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskSummaryController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public UserTaskSummaryController(IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
                throw new Exception("UserId claim not found in token");
            return userId;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllSummaries")]
        public async Task<IActionResult> GetAllSummaries()
        {
            var summaries = await _repository.UserTaskSummaryRepository.GetAllAsync();
            if (summaries == null || !summaries.Any())
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = "No any Task Summaries",
                    Data = null
                });
            }
            var responseDto = _mapper.Map<IEnumerable<UserTaskSummaryResponseDto>>(summaries);
            return Ok(new ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>
            {
                Success = true,
                Message = "Task summaries retrieved successfully",
                Data = responseDto
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllPagedSummaries")]
        public async Task<IActionResult> GetAllPagedSummaries(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var pagedSummaries = await _repository.UserTaskSummaryRepository.GetAllPagedSummariesAsync(pageNumber, pageSize, searchTerm,sortBy,sortOrder,fromDate,toDate);
            if (pagedSummaries == null || !pagedSummaries.Any())
            {
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "No any Task Summaries",
                    Data = null
                });
            }
            var responseDto = _mapper.Map<IEnumerable<UserTaskSummaryResponseDto>>(pagedSummaries);
            var paginationModel = new PaginationModel<UserTaskSummaryResponseDto>
            {
                Items = responseDto,
                CurrentPage = pagedSummaries.CurrentPage,
                TotalPages = pagedSummaries.TotalPages,
                PageSize = pagedSummaries.PageSize,
                TotalCount = pagedSummaries.TotalCount,
                HasNext = pagedSummaries.HasNext,
                HasPrevious = pagedSummaries.HasPrevious,
                FromDate = fromDate,
                ToDate = toDate
            };
            return Ok(new ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>
            {
                Success = true,
                Message = "Task summaries retrieved successfully",
                Data = paginationModel
            });
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetSummariesByUserId")]
        public async Task<IActionResult> GetSummariesByUserId(string userId)
        {
            var summaries = await _repository.UserTaskSummaryRepository.GetSummariesByUserIdAsync(userId);
            if (summaries == null || !summaries.Any())
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = "No Task Summaries found for the user",
                    Data = null
                });
            }
            var responseDto = _mapper.Map<IEnumerable<UserTaskSummaryResponseDto>>(summaries);
            return Ok(new ApiResponse<IEnumerable<UserTaskSummaryResponseDto>>
            {
                Success = true,
                Message = "Task summaries retrieved successfully",
                Data = responseDto
            });
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetPagedSummariesByUserId")]
        public async Task<IActionResult> GetPagedSummariesByUserId(string userId, int pageNumber = 1, int pageSize = 10,string? searchTerm= null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate= null,DateTime? toDate= null)
        {
            var pagedSummaries = await _repository.UserTaskSummaryRepository.GetPagedSummariesByUserIdAsync(userId, pageNumber, pageSize,searchTerm,sortBy,sortOrder,fromDate,toDate);
            if (pagedSummaries == null || !pagedSummaries.Any())
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = "No Task Summaries found for the user",
                    Data = null
                });
            }
            var responseDto = _mapper.Map<IEnumerable<UserTaskSummaryResponseDto>>(pagedSummaries);
            var paginationModel = new PaginationModel<UserTaskSummaryResponseDto>
            {
                Items = responseDto,
                CurrentPage = pagedSummaries.CurrentPage,
                TotalPages = pagedSummaries.TotalPages,
                PageSize = pagedSummaries.PageSize,
                TotalCount = pagedSummaries.TotalCount,
                HasNext = pagedSummaries.HasNext,
                HasPrevious = pagedSummaries.HasPrevious,
                FromDate = fromDate,
                ToDate = toDate
            };
            return Ok(new ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>
            {
                Success = true,
                Message = "Task summaries retrieved successfully",
                Data = paginationModel
            });
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("GetSummaryDetailsById")]
        public async Task<IActionResult> GetSummaryDetailsById(Guid id)
        {
            var summary = await _repository.UserTaskSummaryRepository.GetSummaryWithCommentsAsync(id);
            if (summary == null)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Task Summary with id {id} not found",
                    Data = null
                });
            }

            var responseDto = _mapper.Map<UserTaskSummaryResponseDto>(summary);
            return Ok(new ApiResponse<UserTaskSummaryResponseDto>
            {
                Success = true,
                Message = "Task summary with comments retrieved successfully",
                Data = responseDto
            });
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddSummary")]
        public async Task<IActionResult> AddSummary(AddUserTaskSummaryDto createDto)
        {
            var userId = GetUserId();

            bool alreadyHasTodaySummary = await _repository.UserTaskSummaryRepository.HasSummaryForTodayAsync(userId);
            if (alreadyHasTodaySummary)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = "You have already submitted a summary for today. You can only add Tasks to that summary. Please create a new one tomorrow.",
                    Data = null
                });
            }
            else
            {
                var summary = new UserTaskSummary
                {
                    UserId = userId,
                    Title = createDto.Title,
                    Description = createDto.Description,
                    Status = createDto.Status
                };
                var response = await _repository.UserTaskSummaryRepository.AddAsync(summary);
                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Task summary added successfully",
                    Data = response.Id.ToString()
                });
            }
        }

        [Authorize(Roles = "User")]
        [HttpPut("UpdateSummary")]
        public async Task<IActionResult> UpdateSummary(Guid summaryId, UpdateUserTaskSummaryDto updateDto)
        {
            var userId = GetUserId();
            bool canBeUpdated = await _repository.UserTaskSummaryRepository.CanSummaryBeUpdatedAsync(summaryId);
            if (!canBeUpdated)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = "This summary cannot be updated as it was not created today.",
                    Data = null
                });
            }
            var existingSummary = await _repository.UserTaskSummaryRepository.GetByIdAsync(summaryId);
            if (existingSummary == null)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Task Summary with id {summaryId} not found",
                    Data = null
                });
            }
            var summary = new UserTaskSummary
            {
                Id = summaryId,
                UserId = userId,
                Title = updateDto.Title,
                Description = updateDto.Description,
                Status = updateDto.Status
            };
            await _repository.UserTaskSummaryRepository.UpdateAsync(summary);
            await _repository.SaveAsync();
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Task summary updated successfully",
                Data = null
            });
        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost("AddComment")]
        public async Task<IActionResult> AddComment(CommentRequestDto commentDto)
        {
            var commentEntity = _mapper.Map<Comments>(commentDto);
            var comment = await _repository.CommentRepository.AddAsync(commentEntity);
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Comment added successfully",
                Data = comment.Id.ToString()
            });
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateComment(Guid commentId, CommentRequestDto commentDto)
        {
            var existingComment = await _repository.CommentRepository.GetByIdAsync(commentId);
            if (existingComment == null)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Comment with id {commentId} not found",
                    Data = null
                });
            }
            var userId = User.FindFirstValue("UserId");
            if (userId != existingComment.UserId)
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = "You are not authorized to update this comment",
                    Data = null
                });
            }
            existingComment.Content = commentDto.Content;
            existingComment.UpdatedAt = DateTime.UtcNow;

            await _repository.CommentRepository.UpdateAsync(existingComment);
            await _repository.SaveAsync();
            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Comment updated successfully",
                Data = null
            });
        }
        [HttpGet("GetCommentsById")]
        public async Task<IActionResult> GetCommentsById(Guid commentId)
        {
            var comments = await _repository.CommentRepository.GetCommentsByIdAsync(commentId);
            if (comments == null || !comments.Any())
            {
                return Ok(new ApiResponse<string>
                {
                    Success = false,
                    Message = "No Comments found for the given id",
                    Data = null
                });
            }
            var responseDto = _mapper.Map<IEnumerable<CommentResponseDto>>(comments);
            return Ok(new ApiResponse<IEnumerable<CommentResponseDto>>
            {
                Success = true,
                Message = "Comments retrieved successfully",
                Data = responseDto
            });
        }
    }
}
