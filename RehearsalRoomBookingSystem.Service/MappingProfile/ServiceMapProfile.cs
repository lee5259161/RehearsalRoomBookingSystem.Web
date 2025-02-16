using RehearsalRoomBookingSystem.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RehearsalRoomBookingSystem.Repository.Entities;

namespace RehearsalRoomBookingSystem.Service.MappingProfile
{
    public class ServiceMapProfile : IServiceMapProfile
    {
        public MemberDTO MapToMemberDTO(MemberEntity entity)
        {
            return new MemberDTO
            {
                MemberID = entity.MemberID,
                Name = entity.Name,
                Phone = entity.Phone,
                Card_Available_Hours = entity.Card_Available_Hours,
                Memo = entity.Memo,
                UpdateUser = entity.UpdateUser,
                UpdateDate = entity.UpdateDate
            };
        }

        public MemberEntity MapToMemberEntity(MemberDTO dto)
        {
            return new MemberEntity
            {
                MemberID = dto.MemberID,
                Name = dto.Name,
                Phone = dto.Phone,
                Card_Available_Hours = dto.Card_Available_Hours,
                Memo = dto.Memo,
                UpdateUser = dto.UpdateUser,
                UpdateDate = dto.UpdateDate
            };
        }

        public IEnumerable<MemberDTO> MapToMemberDTOs(IEnumerable<MemberEntity> entities)
        {
            return entities.Select(MapToMemberDTO);
        }

        public IEnumerable<MemberEntity> MapToMemberEntities(IEnumerable<MemberDTO> dtos)
        {
            return dtos.Select(MapToMemberEntity);
        }
    }
}
