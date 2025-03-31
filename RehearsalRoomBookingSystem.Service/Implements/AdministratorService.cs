using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Service.DTOs;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Service.MappingProfile;

namespace RehearsalRoomBookingSystem.Service.Implements
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IServiceMapProfile _serviceMapProfile;

        public AdministratorService(
            IAdministratorRepository administratorRepository,
            IServiceMapProfile serviceMapProfile)
        {
            _administratorRepository = administratorRepository;
            _serviceMapProfile = serviceMapProfile;
        }

        public AdministratorDTO ValidateLogin(string account, string password)
        {
            var adminEntity = _administratorRepository.ValidateLogin(account, password);
            if (adminEntity == null)
                return null;

            return new AdministratorDTO
            {
                AdminID = adminEntity.AdminID,
                TypeID = adminEntity.TypeID,
                Name = adminEntity.Name,
                Account = adminEntity.Account,
                UpdateUser = adminEntity.UpdateUser,
                UpdateDate = adminEntity.UpdateDate
            };
        }

        public AdministratorDTO GetByAccount(string account)
        {
            var adminEntity = _administratorRepository.GetByAccount(account);
            if (adminEntity == null)
                return null;

            return new AdministratorDTO
            {
                AdminID = adminEntity.AdminID,
                TypeID = adminEntity.TypeID,
                Name = adminEntity.Name,
                Account = adminEntity.Account,
                UpdateUser = adminEntity.UpdateUser,
                UpdateDate = adminEntity.UpdateDate
            };
        }
    }
}