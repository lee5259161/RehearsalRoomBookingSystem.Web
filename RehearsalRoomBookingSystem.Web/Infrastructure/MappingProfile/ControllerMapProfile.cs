using RehearsalRoomBookingSystem.Repository.Entities;
using RehearsalRoomBookingSystem.Service.DTOs;
using RehearsalRoomBookingSystem.Web.Models.DataModel;

namespace RehearsalRoomBookingSystem.Web.Infrastructure.MappingProfile
{
    /// <summary>
    /// Represents a mapping profile for controllers.
    /// </summary>
    public class ControllerMapProfile : IControllerMapProfile
    {
        /// <summary>
        /// Maps a MemberDTO object to a MemberViewModel object.
        /// </summary>
        /// <param name="dto">The MemberDTO object to be mapped.</param>
        /// <returns>The mapped MemberViewModel object.</returns>
        public Member MapToMember(MemberDTO dto)
        {
            return new Member
            {
                MemberId = dto.MemberId,
                Name = dto.Name,
                Phone = dto.Phone,
                Card_Available_Hours = dto.Card_Available_Hours,
                Memo = dto.Memo,
                UpdateUser = dto.UpdateUser,
                UpdateDate = dto.UpdateDate
            };
        }

        /// <summary>
        /// Maps a collection of MemberDTO objects to a collection of MemberViewModel objects.
        /// </summary>
        /// <param name="entities">The collection of MemberDTO objects to be mapped.</param>
        /// <returns>The mapped collection of MemberViewModel objects.</returns>
        public IEnumerable<Member> MapToMembers(IEnumerable<MemberDTO> entities)
        {
            return entities.Select(MapToMember);
        }
    }
}
