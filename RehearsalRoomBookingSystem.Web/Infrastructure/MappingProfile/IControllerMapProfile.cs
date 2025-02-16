using RehearsalRoomBookingSystem.Service.DTOs;
using RehearsalRoomBookingSystem.Web.Models.DataModel;

namespace RehearsalRoomBookingSystem.Web.Infrastructure.MappingProfile
{
    public interface IControllerMapProfile
    {
        Member MapToMember(MemberDTO dto);
        IEnumerable<Member> MapToMembers(IEnumerable<MemberDTO> entities);
    }
}
