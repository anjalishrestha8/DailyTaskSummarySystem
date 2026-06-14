using ClientMVC.ApiResponse;
using ClientMVC.Common;
using ClientMVC.Dto.RequestDto;
using ClientMVC.Dto.ResponseDto;
using ClientMVC.Models;
using ClientMVC.Services;
using ClientWebApi.Dto.RequestDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientMVC.Controllers
{
    public class UserTaskSummaryController : Controller
    {
        private readonly IUserTaskSummaryService _userTaskService;
        public UserTaskSummaryController(IUserTaskSummaryService userTaskService)
        {
            _userTaskService = userTaskService;
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
                throw new Exception("UserId claim not found in token");
            return userId;
        }


        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string? searchTerm = null, string? sortBy = null, string? sortOrder = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var model = new ApiResponse<PaginationModel<UserTaskSummaryResponseDto>>();
            if (User.IsInRole("Admin"))
            {
                model = await _userTaskService.GetAllPagedSummariesAsync(pageNumber, pageSize, searchTerm, sortBy, sortOrder, fromDate, toDate);
            }
            else
            {
                var userId = GetUserId();
                model = await _userTaskService.GetPagedSummariesByUserIdAsync(userId, pageNumber, pageSize, searchTerm, sortBy, sortOrder, fromDate, toDate);
            }

            if (!model.Success || model.Data == null)
            {
                var emptyVm = new PaginatedUserTaskSummaryVm();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return PartialView("_UserTaskSummaryPartial", emptyVm);
                }
                else
                {
                    return View(emptyVm);
                }
            }
            var paginatedSummaries = model.Data;
            var viewModel = new PaginatedUserTaskSummaryVm
            {
                Items = paginatedSummaries.Items.Select(s => new UserTaskSummaryVm
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    UserName = s.UserName,
                    Title = s.Title,
                    Description = s.Description,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt
                }).ToList(),
                CurrentPage = paginatedSummaries.CurrentPage,
                TotalPages = paginatedSummaries.TotalPages,
                PageSize = paginatedSummaries.PageSize,
                TotalCount = paginatedSummaries.TotalCount,
                HasNext = paginatedSummaries.HasNext,
                HasPrevious = paginatedSummaries.HasPrevious,
                SearchTerm = paginatedSummaries.SearchTerm,
                SortBy = paginatedSummaries.SortBy,
                SortOrder = paginatedSummaries.SortOrder,
                FromDate = paginatedSummaries.FromDate,
                ToDate = paginatedSummaries.ToDate
            };
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_UserTaskSummaryPartial", viewModel);
            }
            else
            {
                return View(viewModel);

            }
        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddTaskSummary(UserTaskSummaryVm model)
        {
            ModelState.Remove("UserId");
            ModelState.Remove("UserName");
            if (!ModelState.IsValid)
            {
                return Json(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid task summary data",
                });
            }

            var summaryDto = new AddUserTaskSummaryDto
            {
                UserId = GetUserId(),
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,

            };
            var response = await _userTaskService.AddUserTaskSummaryAsync(summaryDto);
            return Json(response);
        }

        [Authorize(Roles = "User")]
        [HttpPut]
        public async Task<IActionResult> UpdateSummary(Guid summaryId, UserTaskSummaryVm model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid summary update data",
                });
            }

            var updateDto = new UpdateUserTaskSummaryDto
            {
                Title = model.Title,
                Description = model.Description,
                Status = model.Status
            };

            var response = await _userTaskService.UpdateUserTaskSummaryAsync(summaryId, updateDto);

            return Json(response);
        }


        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public async Task<IActionResult> AddComment(Guid summaryId, string content)
        {
            var commentDto = new CommentRequestDto
            {
                UserId = GetUserId(),
                UserTaskSummaryId = summaryId,
                Content = content
            };

            var response = await _userTaskService.AddCommentAsync(commentDto);
            return Json(response);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] CommentsVm commentsVm)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid comment update data",
                });
            }

            var updateDto = new CommentRequestDto
            {
                UserId = commentsVm.UserId!,
                UserTaskSummaryId = commentsVm.UserTaskSummaryId,
                Content = commentsVm.Content,
            };

            var response = await _userTaskService.UpdateCommentAsync(commentId, updateDto);
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _userTaskService.GetSummaryDetailsByIdAsync(id);
            if (!response.Success || response.Data == null)
                return NotFound();

            var summary = response.Data;

            var viewModel = new UserTaskSummaryVm
            {
                Id = summary.Id,
                UserId = summary.UserId,
                UserName = summary.UserName,
                Title = summary.Title,
                Description = summary.Description,
                Status = summary.Status,
                CreatedAt = summary.CreatedAt,
                Comments = summary.Comments.Select(c => new CommentsVm
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    UserTaskSummaryId = c.UserTaskSummaryId,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    UserName = c.UserName
                }).OrderByDescending(c => c.CreatedAt).ToList()
            };

            return View(viewModel);
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult AddSummaryPartial()
        {
            var model = new UserTaskSummaryVm();
            return PartialView("Partials/_AddEditSummaryPartial", model);
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> UpdateSummaryPartial(Guid summaryId)
        {
            var response = await _userTaskService.GetSummaryDetailsByIdAsync(summaryId);
            if (!response.Success || response.Data == null)
                return NotFound();

            var model = new UserTaskSummaryVm
            {
                Id = response.Data.Id,
                UserId = response.Data.UserId,
                UserName = response.Data.UserName,
                Title = response.Data.Title,
                Description = response.Data.Description,
                Status = response.Data.Status,
                CreatedAt = response.Data.CreatedAt
            };

            return PartialView("Partials/_AddEditSummaryPartial", model);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<IActionResult> EditCommentPartial(Guid commentId)
        {
            var response = await _userTaskService.GetCommentsByIdAsync(commentId);
            if (!response.Success || response.Data == null)
                return NotFound();
            var comment = response.Data.FirstOrDefault();
            if (comment == null)
                return NotFound();

            var model = new CommentsVm
            {
                Id = comment.Id,
                UserId = comment.UserId,
                Content = comment.Content,
                UserTaskSummaryId = comment.UserTaskSummaryId,
                UpdatedAt = comment.UpdatedAt
            };
            return PartialView("Partials/_EditCommentPartial", model);
        }
    }
}
