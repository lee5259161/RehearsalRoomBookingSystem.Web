using Microsoft.AspNetCore.Http;
using RehearsalRoomBookingSystem.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RehearsalRoomBookingSystem.Common.Implement
{
    public class UserContextHelper : IUserContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserAccount()
        {
            return _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
        }
    }
}
