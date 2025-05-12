using GameTogetherAPI.DTO;
using GameTogetherAPI.Models;
using GameTogetherAPI.Repository;
using GameTogetherAPI.Services;
using GameTogetherAPI.Test.Util;
using Moq;

namespace GameTogetherAPI.Test;

[TestFixture]
public class UserServiceTest
{
    private Mock<IUserRepository> _mockRepository;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepository.Object);
    }

    #region AddOrUpdateProfileAsync Tests

    [Test]
    public async Task AddOrUpdateProfileAsync_WithValidProfile_ReturnsSuccess()
    {
        // Arrange
        var userId = Constants.TestUserId;
        var profileDto = new UpdateProfileRequestDTO
        {
            BirthDate = Constants.ValidBirthDate,
            Description = Constants.TestUserDescription,
            Region = Constants.TestRegion,
            ProfilePicture = "profile.jpg"
        };

        _mockRepository.Setup(repo => repo.AddOrUpdateProfileAsync(It.IsAny<Profile>()))
            .ReturnsAsync(true);

        // Act
        var result = await _userService.AddOrUpdateProfileAsync(userId, profileDto);

        // Assert
        Assert.That(result, Is.EqualTo(UpdateProfileStatus.Success));
        _mockRepository.Verify(repo => repo.AddOrUpdateProfileAsync(It.Is<Profile>(p => 
            p.Id == userId && 
            p.Description == Constants.TestUserDescription &&
            p.Region == Constants.TestRegion &&
            p.ProfilePicture == "profile.jpg")), Times.Once);
    }

    [Test]
    public async Task AddOrUpdateProfileAsync_WithTooYoungBirthDate_ReturnsInvalidBirthDate()
    {
        // Arrange
        var profileDto = new UpdateProfileRequestDTO
        {
            BirthDate = Constants.TooYoungBirthDate,
            Description = Constants.TestUserDescription,
            Region = Constants.TestRegion
        };

        // Act
        var result = await _userService.AddOrUpdateProfileAsync(Constants.TestUserId, profileDto);

        // Assert
        Assert.That(result, Is.EqualTo(UpdateProfileStatus.InvalidBirthDate));
        _mockRepository.Verify(repo => repo.AddOrUpdateProfileAsync(It.IsAny<Profile>()), Times.Never);
    }

    [Test]
    public async Task AddOrUpdateProfileAsync_WithTooOldBirthDate_ReturnsInvalidBirthDate()
    {
        // Arrange
        var profileDto = new UpdateProfileRequestDTO
        {
            BirthDate = Constants.TooOldBirthDate,
            Description = Constants.TestUserDescription,
            Region = Constants.TestRegion
        };

        // Act
        var result = await _userService.AddOrUpdateProfileAsync(Constants.TestUserId, profileDto);

        // Assert
        Assert.That(result, Is.EqualTo(UpdateProfileStatus.InvalidBirthDate));
        _mockRepository.Verify(repo => repo.AddOrUpdateProfileAsync(It.IsAny<Profile>()), Times.Never);
    }

    [Test]
    public async Task AddOrUpdateProfileAsync_WithTooLongDescription_ReturnsInvalidDescription()
    {
        // Arrange
        var profileDto = new UpdateProfileRequestDTO
        {
            BirthDate = Constants.ValidBirthDate,
            Description = Constants.TooLongDescription,
            Region = Constants.TestRegion
        };

        // Act
        var result = await _userService.AddOrUpdateProfileAsync(Constants.TestUserId, profileDto);

        // Assert
        Assert.That(result, Is.EqualTo(UpdateProfileStatus.InvalidDescription));
        _mockRepository.Verify(repo => repo.AddOrUpdateProfileAsync(It.IsAny<Profile>()), Times.Never);
    }

    [Test]
    public async Task AddOrUpdateProfileAsync_WithDescriptionContainingLinks_ReturnsInvalidDescription()
    {
        // Arrange
        var profileDto = new UpdateProfileRequestDTO
        {
            BirthDate = Constants.ValidBirthDate,
            Description = Constants.DescriptionWithLinks,
            Region = Constants.TestRegion
        };

        // Act
        var result = await _userService.AddOrUpdateProfileAsync(Constants.TestUserId, profileDto);

        // Assert
        Assert.That(result, Is.EqualTo(UpdateProfileStatus.InvalidDescription));
        _mockRepository.Verify(repo => repo.AddOrUpdateProfileAsync(It.IsAny<Profile>()), Times.Never);
    }

    [Test]
    public async Task AddOrUpdateProfileAsync_WithEmptyDescription_IsValid()
    {
        // Arrange
        var profileDto = new UpdateProfileRequestDTO
        {
            BirthDate = Constants.ValidBirthDate,
            Description = string.Empty,
            Region = Constants.TestRegion
        };

        _mockRepository.Setup(repo => repo.AddOrUpdateProfileAsync(It.IsAny<Profile>()))
            .ReturnsAsync(true);

        // Act
        var result = await _userService.AddOrUpdateProfileAsync(Constants.TestUserId, profileDto);

        // Assert
        Assert.That(result, Is.EqualTo(UpdateProfileStatus.Success));
        _mockRepository.Verify(repo => repo.AddOrUpdateProfileAsync(It.IsAny<Profile>()), Times.Once);
    }

    [Test]
    public async Task AddOrUpdateProfileAsync_RepositoryFailure_ReturnsUnknownFailure()
    {
        // Arrange
        var profileDto = new UpdateProfileRequestDTO
        {
            BirthDate = Constants.ValidBirthDate,
            Description = Constants.TestUserDescription,
            Region = Constants.TestRegion
        };

        _mockRepository.Setup(repo => repo.AddOrUpdateProfileAsync(It.IsAny<Profile>()))
            .ReturnsAsync(false);

        // Act
        var result = await _userService.AddOrUpdateProfileAsync(Constants.TestUserId, profileDto);

        // Assert
        Assert.That(result, Is.EqualTo(UpdateProfileStatus.UnknownFailure));
    }

    #endregion

    #region GetProfileAsync Tests

    [Test]
    public async Task GetProfileAsync_ExistingProfile_ReturnsProfile()
    {
        // Arrange
        var userId = Constants.TestUserId;
        var profile = new Profile
        {
            Id = userId,
            User = new User { Username = Constants.TestUserName },
            BirthDate = Constants.ValidBirthDate,
            Description = Constants.TestUserDescription,
            Region = Constants.TestRegion,
            ProfilePicture = "profile.jpg"
        };

        _mockRepository.Setup(repo => repo.GetProfileAsync(userId))
            .ReturnsAsync(profile);

        // Act
        var result = await _userService.GetProfileAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Username, Is.EqualTo(Constants.TestUserName));
        Assert.That(result.BirthDate, Is.EqualTo(Constants.ValidBirthDate));
        Assert.That(result.Description, Is.EqualTo(Constants.TestUserDescription));
        Assert.That(result.Region, Is.EqualTo(Constants.TestRegion));
        Assert.That(result.ProfilePicture, Is.EqualTo("profile.jpg"));
    }

    [Test]
    public async Task GetProfileAsync_NonExistentProfile_ThrowsException()
    {
        // Arrange
        var userId = Constants.TestUserId;
        _mockRepository.Setup(repo => repo.GetProfileAsync(userId))
            .ThrowsAsync(new ArgumentException($"User profile not found for ID {userId}"));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _userService.GetProfileAsync(userId));
    }

    #endregion

    #region GetProfileByIdAsync Tests

    [Test]
    public async Task GetProfileByIdAsync_ExistingProfile_ReturnsProfile()
    {
        // Arrange
        var userId = Constants.TestUserId;
        var profile = new Profile
        {
            Id = userId,
            User = new User { Username = Constants.TestUserName },
            BirthDate = Constants.ValidBirthDate,
            Description = Constants.TestUserDescription,
            Region = Constants.TestRegion,
            ProfilePicture = "profile.jpg"
        };

        _mockRepository.Setup(repo => repo.GetProfileAsync(userId))
            .ReturnsAsync(profile);

        // Act
        var result = await _userService.GetProfileByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Username, Is.EqualTo(Constants.TestUserName));
        Assert.That(result.BirthDate, Is.EqualTo(Constants.ValidBirthDate));
        Assert.That(result.Description, Is.EqualTo(Constants.TestUserDescription));
        Assert.That(result.Region, Is.EqualTo(Constants.TestRegion));
        Assert.That(result.ProfilePicture, Is.EqualTo("profile.jpg"));
    }

    [Test]
    public async Task GetProfileByIdAsync_NonExistentProfile_ThrowsException()
    {
        // Arrange
        var userId = Constants.TestUserId;
        _mockRepository.Setup(repo => repo.GetProfileAsync(userId))
            .ThrowsAsync(new ArgumentException($"User profile not found for ID {userId}"));

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(async () => await _userService.GetProfileByIdAsync(userId));
    }

    [Test]
    public async Task GetProfileByIdAsync_ProfileWithNullUser_ReturnsProfileWithNullUsername()
    {
        // Arrange
        var userId = Constants.TestUserId;
        var profile = new Profile
        {
            Id = userId,
            User = null,
            BirthDate = Constants.ValidBirthDate,
            Description = Constants.TestUserDescription,
            Region = Constants.TestRegion
        };

        _mockRepository.Setup(repo => repo.GetProfileAsync(userId))
            .ReturnsAsync(profile);

        // Act
        var result = await _userService.GetProfileByIdAsync(userId);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNull(result.Username);
    }

    #endregion

    #region GetUserIdByEmailAsync Tests

    [Test]
    public async Task GetUserIdByEmailAsync_ExistingUser_ReturnsUserId()
    {
        // Arrange
        var email = Constants.TestUserEmail;
        var user = new User
        {
            Id = Constants.TestUserId,
            Email = email
        };

        _mockRepository.Setup(repo => repo.GetUserByEmailAsync(email))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.GetUserIdByEmailAsync(email);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result, Is.EqualTo(Constants.TestUserId));
    }

    [Test]
    public async Task GetUserIdByEmailAsync_NonExistentUser_ReturnsNull()
    {   
        // Arrange
        var email = Constants.NonExistentEmail;
        bool failed = false;
        try
        {
            _mockRepository.Setup(repo => repo.GetUserByEmailAsync(email))
                .ReturnsAsync((User)null);
        }
        catch (Exception e)
        {
            failed = true; // this is a success condition
        }
        // Act
        var result = await _userService.GetUserIdByEmailAsync(email);

        // Assert
        Assert.IsNull(result);
    }

    #endregion
}
