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
        public ActionResult Index([FromQuery] int? page, string phone)
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

        [HttpGet]
        public ActionResult CreateMemberData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMemberData([FromForm]Member member)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(member);
                }

                // 檢查電話是否已存在
                if (_memberService.IsPhoneExist(member.Phone))
                {
                    ModelState.AddModelError("Phone", "此電話號碼已被使用");
                    return View(member);
                }

                var memberDTO = new MemberDTO
                {
                    Name = member.Name,
                    Phone = member.Phone,
                    Birthday = member.Birthday,
                    Memo = member.Memo,
                    Card_Available_Hours = 0,
                    UpdateUser = User.Identity.Name,
                    UpdateDate = DateTime.Now
                };

                var result = _memberService.CreateMember(memberDTO);

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "新增會員失敗");
                return View(member);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "新增會員時發生錯誤");
                ModelState.AddModelError("", "處理過程發生錯誤");
                return View(member);
            }
        }

        [HttpGet]
        public ActionResult EditMemberData([FromQuery] int memberId)
        {
            try
            {
                var memberDTO = _memberService.GetById(memberId);
                if (memberDTO == null)
                {
                    return NotFound();
                }

                var member = _controllerMapProfile.MapToMember(memberDTO);
                return View(member);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "取得會員資料時發生錯誤。MemberId: {MemberId}", memberId);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMemberData([FromForm]Member member)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(member);
                }

                // 取得原始會員資料
                var originalMember = _memberService.GetById(member.MemberId);
                if (originalMember == null)
                {
                    ModelState.AddModelError("", "找不到會員資料");
                    return View(member);
                }

                // 檢查電話是否已存在，但如果是原本的電話則允許
                if (_memberService.IsPhoneExist(member.Phone) && member.Phone != originalMember.Phone)
                {
                    ModelState.AddModelError("Phone", "此電話號碼已被使用");
                    return View(member);
                }

                var result = _memberService.UpdateMemberData(new MemberDTO
                {
                    MemberId = member.MemberId,
                    Name = member.Name,
                    Phone = member.Phone,
                    Birthday = member.Birthday,
                    Memo = member.Memo
                });

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "更新會員資料失敗");
                return View(member);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "更新會員資料時發生錯誤。MemberId: {MemberId}", member.MemberId);
                ModelState.AddModelError("", "處理過程發生錯誤");
                return View(member);
            }
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
        public ActionResult GetMemberTransactions([FromQuery]int memberId, int? page)
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

                // 檢查交易是否存在且尚未被回復
                var transactions = _memberTransactionsService.GetMemberTransactions(parameter.MemberId);
                var transaction = transactions.FirstOrDefault(t => t.TransactionId == parameter.TransactionId);

                if (transaction == null)
                {
                    Log.Warning("找不到要回復的交易記錄。TransactionId: {TransactionId}", parameter.TransactionId);
                    return Json(new { success = false, message = "找不到要回復的交易記錄" });
                }

                if (transaction.IsRecovered)
                {
                    Log.Warning("此交易已被回復過。TransactionId: {TransactionId}", parameter.TransactionId);
                    return Json(new { success = false, message = "此交易已被回復過" });
                }

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
