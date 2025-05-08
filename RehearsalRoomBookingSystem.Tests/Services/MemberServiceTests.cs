using Xunit;
using Moq;
using FluentAssertions;
using RehearsalRoomBookingSystem.Service.Interface;
using RehearsalRoomBookingSystem.Service.Implements;
using RehearsalRoomBookingSystem.Repository.Interface;
using RehearsalRoomBookingSystem.Service.DTOs;
using RehearsalRoomBookingSystem.Repository.Entities;
using RehearsalRoomBookingSystem.Service.MappingProfile;
using RehearsalRoomBookingSystem.Repository.Entities.ResultEntity;

namespace RehearsalRoomBookingSystem.Tests.Services
{
    public class MemberServiceTests
    {
        private readonly Mock<IMemberRepository> _mockMemberRepository;
        private readonly Mock<IServiceMapProfile> _mockServiceMapProfile;
        private readonly MemberService _memberService;

        public MemberServiceTests()
        {
            _mockMemberRepository = new Mock<IMemberRepository>();
            _mockServiceMapProfile = new Mock<IServiceMapProfile>();
            _memberService = new MemberService(_mockMemberRepository.Object, _mockServiceMapProfile.Object);
        }

        [Fact]
        public void CreateMember_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            var memberDto = new MemberDTO
            {
                Name = "Test User",
                Phone = "0912345678",
                Birthday = DateTime.Now.AddYears(-20)
            };

            var memberEntity = new MemberEntity
            {
                Name = memberDto.Name,
                Phone = memberDto.Phone,
                Birthday = memberDto.Birthday,
                Card_Available_Hours = 0
            };

            _mockServiceMapProfile.Setup(x => x.MapToMemberEntity(It.IsAny<MemberDTO>()))
                .Returns(memberEntity);

            _mockMemberRepository.Setup(x => x.IsPhoneExist(It.IsAny<string>()))
                .Returns(false);

            _mockMemberRepository.Setup(x => x.CreateMember(It.IsAny<MemberEntity>()))
                .Returns(true);

            // Act
            var result = _memberService.CreateMember(memberDto);

            // Assert
            result.Should().BeTrue();
            _mockMemberRepository.Verify(x => x.CreateMember(It.IsAny<MemberEntity>()), Times.Once);
        }

        [Fact]
        public void CreateMember_WithExistingPhone_ShouldReturnFalse()
        {
            // Arrange
            var memberDto = new MemberDTO
            {
                Name = "Test User",
                Phone = "0912345678",
                Birthday = DateTime.Now.AddYears(-20)
            };

            _mockMemberRepository.Setup(x => x.IsPhoneExist(It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = _memberService.CreateMember(memberDto);

            // Assert
            result.Should().BeFalse();
            _mockMemberRepository.Verify(x => x.CreateMember(It.IsAny<MemberEntity>()), Times.Never);
        }

        [Fact]
        public void UseCardTime_WithSufficientHours_ShouldReturnSuccess()
        {
            // Arrange
            var memberId = 1;
            var hours = 2;

            var useCardTimeResult = new UseCardTimeResultDTO
            {
                Success = true,
                Message = "扣除時數成功",
                RemainingHours = 8
            };

            _mockMemberRepository.Setup(x => x.UseCardTime(memberId, hours))
                .Returns(new CardTimeResultEntity 
                { 
                    Success = true,
                    Message = "扣除時數成功",
                    RemainingHours = 8
                });

            // Act
            var result = _memberService.UseCardTime(memberId, hours);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.RemainingHours.Should().Be(8);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnMember()
        {
            // Arrange
            var memberId = 1;
            var memberEntity = new MemberEntity
            {
                MemberId = memberId,
                Name = "Test User",
                Phone = "0912345678",
                Card_Available_Hours = 10
            };

            var expectedDto = new MemberDTO
            {
                MemberId = memberId,
                Name = "Test User",
                Phone = "0912345678",
                Card_Available_Hours = 10
            };

            _mockMemberRepository.Setup(x => x.GetById(memberId))
                .Returns(memberEntity);

            _mockServiceMapProfile.Setup(x => x.MapToMemberDTO(memberEntity))
                .Returns(expectedDto);

            // Act
            var result = _memberService.GetById(memberId);

            // Assert
            result.Should().NotBeNull();
            result.MemberId.Should().Be(memberId);
            result.Name.Should().Be("Test User");
            result.Phone.Should().Be("0912345678");
            result.Card_Available_Hours.Should().Be(10);
        }

        [Fact]
        public void SearchByPhone_WithValidPhone_ShouldReturnMembers()
        {
            // Arrange
            var phone = "0912345678";
            var pageNumber = 1;
            var pageSize = 10;
            var memberEntities = new List<MemberEntity>
            {
                new MemberEntity 
                { 
                    MemberId = 1,
                    Name = "Test User 1",
                    Phone = phone
                }
            };

            var expectedDtos = new List<MemberDTO>
            {
                new MemberDTO
                {
                    MemberId = 1,
                    Name = "Test User 1",
                    Phone = phone
                }
            };

            _mockMemberRepository.Setup(x => x.SearchByPhone(phone, pageNumber, pageSize))
                .Returns(memberEntities);

            _mockServiceMapProfile.Setup(x => x.MapToMemberDTOs(memberEntities))
                .Returns(expectedDtos);

            // Act
            var results = _memberService.SearchByPhone(phone, pageNumber, pageSize);

            // Assert
            results.Should().NotBeNull();
            results.Should().HaveCount(1);
            results.First().Phone.Should().Be(phone);
        }

        [Fact]
        public void BuyCardTime_ShouldAddTenHours()
        {
            // Arrange
            var memberId = 1;
            var expectedResult = new BuyCardTimeResultDTO
            {
                Success = true,
                Message = "購買成功",
                RemainingHours = 20
            };

            _mockMemberRepository.Setup(x => x.BuyCardTime(memberId))
                .Returns(new BuyCardTimeResultEntity
                {
                    Success = true,
                    Message = "購買成功",
                    RemainingHours = 20
                });

            // Act
            var result = _memberService.BuyCardTime(memberId);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.RemainingHours.Should().Be(20);
        }
    }
}