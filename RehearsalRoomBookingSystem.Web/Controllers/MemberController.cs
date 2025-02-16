using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Web.Infrastructure.MappingProfile;
using RehearsalRoomBookingSystem.Web.Models.ViewModels;
using RehearsalRoomBookingSystem.Web.Models.DataModel;
using RehearsalRoomBookingSystem.Web.Models.Parameter;
using RehearsalRoomBookingSystem.Web.Models.APIResult;

namespace RehearsalRoomBookingSystem.Web.Controllers
{
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
                var serviceResult = _memberService.UseCardTime(parameter.MemberId, parameter.UseHours);

                var result = new UseCardTimeResult
                {
                    Success = serviceResult.Success,
                    RemainingHours = serviceResult.RemainingHours,
                    Message = serviceResult.Message
                };

                if (!result.Success)
                {
                    return Ok(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // 建議加入日誌記錄
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
