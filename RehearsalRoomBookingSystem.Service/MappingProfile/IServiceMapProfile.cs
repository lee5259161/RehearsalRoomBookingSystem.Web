using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RehearsalRoomBookingSystem.Repository.Entities;
using RehearsalRoomBookingSystem.Service.DTOs;

namespace RehearsalRoomBookingSystem.Service.MappingProfile
{
    public interface IServiceMapProfile
    {
        MemberDTO MapToMemberDTO(MemberEntity entity);

        MemberEntity MapToMemberEntity(MemberDTO dto);

        MemberTransactionDTO MapToMemberTransactionDTO(MemberTransactionEntity entity);

        MemberTransactionEntity MapToMemberTransactionEntity(MemberTransactionDTO dto);

        IEnumerable<MemberDTO> MapToMemberDTOs(IEnumerable<MemberEntity> entities);

        IEnumerable<MemberEntity> MapToMemberEntities(IEnumerable<MemberDTO> dtos);

        IEnumerable<MemberTransactionEntity> MapToMemberTransactionEntities(IEnumerable<MemberTransactionDTO> dtos);

        IEnumerable<MemberTransactionDTO> MapToMemberTransactionDTOs(IEnumerable<MemberTransactionEntity> entities);
    }
}
