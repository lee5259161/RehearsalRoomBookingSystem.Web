using RehearsalRoomBookingSystem.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RehearsalRoomBookingSystem.Repository.Entities;
using static Dapper.SqlMapper;

namespace RehearsalRoomBookingSystem.Service.MappingProfile
{
    public class ServiceMapProfile : IServiceMapProfile
    {
        public MemberDTO MapToMemberDTO(MemberEntity entity)
        {
            return new MemberDTO
            {
                MemberId = entity.MemberId,
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
                MemberId = dto.MemberId,
                Name = dto.Name,
                Phone = dto.Phone,
                Card_Available_Hours = dto.Card_Available_Hours,
                Memo = dto.Memo,
                UpdateUser = dto.UpdateUser,
                UpdateDate = dto.UpdateDate
            };
        }

        public MemberTransactionDTO MapToMemberTransactionDTO(MemberTransactionEntity entity)
        {
            return new MemberTransactionDTO
            {
                TransactionId = entity.TransactionId,
                MemberId = entity.MemberId,
                TypeId = entity.TypeId,
                TransactionHours = entity.TransactionHours,
                CreateUser = entity.CreateUser,
                CreateDate = entity.CreateDate,
                RecoveryTransactionId = entity.RecoveryTransactionId,
                IsRecovered = entity.IsRecovered,
                RecoverUser = entity.RecoverUser,
                RecoverDate = entity.RecoverDate
            };
        }

        public MemberTransactionEntity MapToMemberTransactionEntity(MemberTransactionDTO dto)
        {
            return new MemberTransactionEntity
            {
                TransactionId = dto.TransactionId,
                MemberId = dto.MemberId,
                TypeId = dto.TypeId,
                TransactionHours = dto.TransactionHours,
                CreateUser = dto.CreateUser,
                CreateDate = dto.CreateDate,
                RecoveryTransactionId = dto.RecoveryTransactionId,
                IsRecovered = dto.IsRecovered,
                RecoverUser = dto.RecoverUser,
                RecoverDate = dto.RecoverDate
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

        public IEnumerable<MemberTransactionEntity> MapToMemberTransactionEntities(IEnumerable<MemberTransactionDTO> dtos)
        {
            return dtos.Select(MapToMemberTransactionEntity);
        }


        public IEnumerable<MemberTransactionDTO> MapToMemberTransactionDTOs(IEnumerable<MemberTransactionEntity> entities)
        {
            return entities.Select(MapToMemberTransactionDTO);
        }
    }
}
