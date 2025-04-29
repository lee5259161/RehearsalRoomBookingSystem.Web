using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Service.DTOs;
using RehearsalRoomBookingSystem.Web.Infrastructure.MappingProfile;
using RehearsalRoomBookingSystem.Web.Models.ViewModels;
using RehearsalRoomBookingSystem.Web.Models.DataModel;
using RehearsalRoomBookingSystem.Web.Models.Parameter;
using RehearsalRoomBookingSystem.Web.Models.APIResult;
using Serilog;
using X.PagedList;

namespace RehearsalRoomBookingSystem.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IControllerMapProfile _controllerMapProfile;
        private readonly IMemberTransactionsService _memberTransactionsService;

        public MemberController(IMemberService memberService, 
                              IControllerMapProfile controllerMapProfile,
                              IMemberTransactionsService memberTransactionsService)
        {
            _memberService = memberService;
            _controllerMapProfile = controllerMapProfile;
            _memberTransactionsService = memberTransactionsService;
        }

        // GET: MemberController
        [HttpGet]
        public ActionResult Index(int? page, string phone)
        {
            int pageNumber = page ?? 1;
            const int pageSize = 10;

            var totalCount = 0;
            IEnumerable<MemberDTO> memberDTOs;
            
            if (!string.IsNullOrWhiteSpace(phone))
            {
                memberDTOs = _memberService.SearchByPhone(phone, pageNumber, pageSize);
                totalCount = _memberService.GetTotalCountFromSearchByPhone(phone);
            }
            else
            {
                memberDTOs = _memberService.GetPagedCollection(pageNumber, pageSize);
                totalCount = _memberService.GetTotalCount();
            }

            var members = _controllerMapProfile.MapToMembers(memberDTOs);

            var memberViewModel = new MemberViewModel
            {
                Members = new StaticPagedList<Models.DataModel.Member>(
                    members,
                    pageNumber,
                    pageSize,
                    totalCount
                ),
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return View(memberViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UseCardTime([FromBody] UseCardTimeParameter parameter)
        {
            try
            {
                Log.Information("開始處理扣除會員練團卡時數請求。MemberId: {MemberId}, Hours: {Hours}", 
                    parameter.MemberId, parameter.UseHours);

                var serviceResult = _memberService.UseCardTime(parameter.MemberId, parameter.UseHours);

                if (!serviceResult.Success)
                {
                    Log.Warning("扣除會員練團卡時數失敗。MemberId: {MemberId}, Hours: {Hours}, Message: {Message}",
                        parameter.MemberId, parameter.UseHours, serviceResult.Message);
                }

                var result = new UseCardTimeResult
                {
                    Success = serviceResult.Success,
                    RemainingHours = serviceResult.RemainingHours,
                    Message = serviceResult.Message
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "扣除會員練團卡時數處理發生未預期的錯誤。MemberId: {MemberId}, Hours: {Hours}", 
                    parameter.MemberId, parameter.UseHours);
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyCardTime([FromBody] BuyCardTimeParameter parameter)
        {
            try
            {
                Log.Information("開始處理購買會員練團卡時數請求。MemberId: {MemberId}", parameter.MemberId);

                var serviceResult = _memberService.BuyCardTime(parameter.MemberId);

                if (!serviceResult.Success)
                {
                    Log.Warning("購買會員練團卡時數失敗。MemberId: {MemberId}, Message: {Message}",
                        parameter.MemberId, serviceResult.Message);
                }

                var result = new BuyCardTimeResult
                {
                    Success = serviceResult.Success,
                    RemainingHours = serviceResult.RemainingHours,
                    Message = serviceResult.Message
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "購買會員練團卡時數處理發生未預期的錯誤。MemberId: {MemberId}", parameter.MemberId);
                return BadRequest();
            }
        }

        [HttpGet]
        public ActionResult GetMemberTransactions(int memberId, int? page)
        {
            try
            {
                const int pageSize = 10;
                int pageNumber = page ?? 1;

                var totalCount = _memberTransactionsService.GetMemberTransactionsCount(memberId);
                var transactions = _memberTransactionsService.GetPagedMemberTransactions(memberId, pageNumber, pageSize);

                return Json(new
                {
                    transactions = transactions,
                    totalCount = totalCount,
                    pageSize = pageSize,
                    currentPage = pageNumber
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "取得會員交易記錄時發生錯誤。MemberId: {MemberId}", memberId);
                return BadRequest(new { message = "取得交易記錄失敗" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecoverTransaction([FromBody] RecoverTransactionParameter parameter)
        {
            try
            {
                Log.Information("開始回復交易記錄。TransactionId: {TransactionId}, MemberId: {MemberId}",
                    parameter.TransactionId, parameter.MemberId);

                var result = _memberTransactionsService.RecoverTransaction(parameter.TransactionId, parameter.MemberId);

                if (!result.Success)
                {
                    Log.Warning("回復交易記錄失敗。TransactionId: {TransactionId}, Message: {Message}",
                        parameter.TransactionId, result.Message);
                }

                return Json(new
                {
                    success = result.Success,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "回復交易記錄時發生錯誤。TransactionId: {TransactionId}, MemberId: {MemberId}",
                    parameter.TransactionId, parameter.MemberId);
                return BadRequest(new { message = "回復交易記錄失敗" });
            }
        }
    }
}
