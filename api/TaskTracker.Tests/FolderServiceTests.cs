using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;
using TaskTracker.Data;
using TaskTracker.Data.Entities;
using TaskTracker.Web;
using TaskTracker.Web.Services.Auth;
using TaskTracker.Web.Services.Common.Dtos;
using TaskTracker.Web.Services.Folder;
using TaskTracker.Web.Services.Folder.Dtos;

namespace TaskTracker.Tests;

[TestFixture]
public class FolderServiceTests
{
    private Mock<IDbContext> _dbContextMock;
    private Guid CurrentIdentityId = Guid.NewGuid();
    private Mock<ICurrentIdentity> _currentIdentityMock;
    private IMapper _mapper;
    private IFolderService _folderService;

    [SetUp]
    public void SetUp()
    {
        _dbContextMock = new Mock<IDbContext>();
        _currentIdentityMock = new Mock<ICurrentIdentity>();
        _currentIdentityMock.Setup(_currentIdentityMock => _currentIdentityMock.GetUserGuid()).Returns(CurrentIdentityId);

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        });

        _mapper = mapperConfig.CreateMapper();
        _folderService = new FolderService(_dbContextMock.Object, _mapper, _currentIdentityMock.Object);
    }

    [Test]
    public async Task CreateFolder_ShouldCreateFolder()
    {
        // Arrange
        var createFolderDto = new CreateFolderDto
        {
            Name = "Test Folder"
        };
        var cancellationToken = CancellationToken.None;

        _dbContextMock.Setup(x => x.Set<FolderEntity>()).ReturnsDbSet(new List<FolderEntity>());

        // Act
        var result = await _folderService.CreateFolder(createFolderDto, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Name, Is.EqualTo(createFolderDto.Name));
    }

   

    [Test]
    public async Task DeleteFolder_ShouldDeleteFolder()
    {
        // Mock folders
        var folderId = Guid.NewGuid();

        var folders = new List<FolderEntity>
        {
            new() { Id = folderId, Name = "Folder 1", OwnerId = CurrentIdentityId, Owner = new UserEntity() { Id = CurrentIdentityId } }
        };

        _dbContextMock.Setup(x => x.Set<FolderEntity>()).ReturnsDbSet(folders);

        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        await _folderService.DeleteFolder(folderId, cancellationToken);

        // Assert
        _dbContextMock.Verify(db => db.Set<FolderEntity>().Remove(It.Is<FolderEntity>(f => f.Id == folderId)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Test]
    public async Task UpdateFolder_ShouldUpdateFolder()
    {
        // Mock folders
        var folderId = Guid.NewGuid();
        var folders = new List<FolderEntity>
        {
            new() { Id = folderId, Name = "Folder 1", OwnerId = CurrentIdentityId, Owner = new UserEntity() { Id = CurrentIdentityId } }
        };

        _dbContextMock.Setup(x => x.Set<FolderEntity>()).ReturnsDbSet(folders);

        // Arrange
        var updateFolderDto = new UpdateFolderDto
        {
            Id = folderId,
            Name = "Folder 2"
        };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _folderService.UpdateFolder(updateFolderDto, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.That(result.Name, Is.EqualTo(updateFolderDto.Name));
    }
}
