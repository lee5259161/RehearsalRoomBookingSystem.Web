using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Web.Infrastructure.MappingProfile;
using RehearsalRoomBookingSystem.Web.Models.ViewModels;
using RehearsalRoomBookingSystem.Web.Models.DataModel;
using RehearsalRoomBookingSystem.Web.Models.Parameter;
using RehearsalRoomBookingSystem.Web.Models.APIResult;
using Microsoft.AspNetCore.Authorization;
using Serilog;

namespace RehearsalRoomBookingSystem.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IControllerMapProfile _controllerMapProfile;
        
        public MemberController(IMemberService memberService, 
                              IControllerMapProfile controllerMapProfile)
        {
            _memberService = memberService;
            _controllerMapProfile = controllerMapProfile;
        }

        // GET: MemberController
        public ActionResult Index()
        {
            var memberDTOs = _memberService.GetCollection();
            var members = _controllerMapProfile.MapToMembers(memberDTOs);
            var memberViewModel = new MemberViewModel
            {
                Members = members.ToList(),
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

        // GET: MemberController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MemberController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MemberController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MemberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: MemberController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MemberController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
