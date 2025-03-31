using Microsoft.AspNetCore.Http;

namespace RehearsalRoomBookingSystem.Common.Interface
{
    public interface IUserContextHelper
    {
        string GetCurrentUserAccount();
    }
}
