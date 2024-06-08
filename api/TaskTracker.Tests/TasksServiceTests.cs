using AutoMapper;
using Moq;
using Moq.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskTracker.Common.Exceptions;
using TaskTracker.Data;
using TaskTracker.Data.Entities;
using TaskTracker.Web;
using TaskTracker.Web.Services.Auth;
using TaskTracker.Web.Services.Common.Dtos;
using TaskTracker.Web.Services.Tasks;
using TaskTracker.Web.Services.Tasks.Dtos;

namespace TaskTracker.Tests;

[TestFixture]
public class TaskServiceTests
{
    private Mock<IDbContext> _dbContextMock;
    private Guid CurrentIdentityId = Guid.NewGuid();
    private Mock<ICurrentIdentity> _currentIdentityMock;
    private IMapper _mapper;
    private ITaskService _taskService;

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
        _taskService = new TaskService(_dbContextMock.Object, _mapper, _currentIdentityMock.Object);
    }

    [Test]
    public async Task CreateTask_ShouldCreateTask()
    {
        // Mock folders
        var folder2Id = Guid.NewGuid();

        var folders = new List<FolderEntity>
        {
            new() { Id = folder2Id, Name = "Folder 2", OwnerId = CurrentIdentityId, Owner = new UserEntity() { Id = CurrentIdentityId }  }
        };

        _dbContextMock.Setup(x => x.Set<FolderEntity>()).ReturnsDbSet(folders);
        _dbContextMock.Setup(x => x.Set<TaskEntity>()).ReturnsDbSet(new List<TaskEntity>());

        // Arrange
        var createTaskDto = new CreateTaskDto
        {
            Content = "Test Task",
            IsCompleted = false,
            FolderId = folder2Id
        };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _taskService.CreateTask(createTaskDto, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Content, Is.EqualTo(createTaskDto.Content));
        Assert.That(result.IsCompleted, Is.EqualTo(createTaskDto.IsCompleted));
        _dbContextMock.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Test]
    public async Task GetAllTasks_ShouldReturnPagedResult()
    {
        var tasks = new List<TaskEntity>()
        {
            new() { Id = Guid.NewGuid(), Content = "Task 1", CreatedAt = DateTime.UtcNow, Folder = new() { Name = "Test 1", OwnerId = CurrentIdentityId, Owner = new UserEntity() { Id = CurrentIdentityId } } },
            new() { Id = Guid.NewGuid(), Content = "Task 2", CreatedAt = DateTime.UtcNow.AddMinutes(-1), Folder = new() { Name = "Test 1", OwnerId = CurrentIdentityId, Owner = new UserEntity() { Id = CurrentIdentityId }  } },
        };

        _dbContextMock.Setup(x => x.Set<TaskEntity>()).ReturnsDbSet(tasks);

        var pagedRequestDto = new PagedRequestDto { Page = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;

        var result = await _taskService.GetAllTasks(pagedRequestDto, cancellationToken);

        Assert.IsNotNull(result);
        Assert.That(result.TotalCount, Is.EqualTo(tasks.Count));
        Assert.That(result.Items.Count, Is.EqualTo(tasks.Count));
    }

    [Test]
    public async Task DeleteTask_ShouldRemoveTask()
    {
        // Mock tasks
        var taskId = Guid.NewGuid();
        var tasks = new List<TaskEntity>
        {
            new() { Id = taskId, Content = "Task 1", Folder = new() {  Name = "Test", OwnerId = CurrentIdentityId, Owner = new UserEntity() { Id = CurrentIdentityId } } },
        };

        _dbContextMock.Setup(x => x.Set<TaskEntity>()).ReturnsDbSet(tasks);

        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        await _taskService.DeleteTask(taskId, cancellationToken);

        // Assert
        _dbContextMock.Verify(db => db.Set<TaskEntity>().Remove(It.Is<TaskEntity>(t => t.Id == taskId)), Times.Once);
        _dbContextMock.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Test]
    public async Task UpdateTask_ShouldUpdateTask()
    {
        // Mock folders and tasks
        var folderId = Guid.NewGuid();
        var folder = new FolderEntity { Id = folderId, Name = "Folder 1", OwnerId = CurrentIdentityId, Owner = new UserEntity() { Id = CurrentIdentityId } };
        var taskId = Guid.NewGuid();
        var folders = new List<FolderEntity>
        {
            folder
        };
        var tasks = new List<TaskEntity>
        {
            new() { Id = taskId, Content = "Task 1", FolderId = folderId, Folder = folder }
        };

        _dbContextMock.Setup(x => x.Set<FolderEntity>()).ReturnsDbSet(folders);
        _dbContextMock.Setup(x => x.Set<TaskEntity>()).ReturnsDbSet(tasks);

        // Arrange
        var updateTaskDto = new UpdateTaskDto
        {
            Id = taskId,
            Content = "Updated Task",
            IsCompleted = true,
            FolderId = folderId
        };
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _taskService.UpdateTask(updateTaskDto, cancellationToken);

        // Assert
        Assert.IsNotNull(result);
        Assert.That(result.Content, Is.EqualTo(updateTaskDto.Content));
        Assert.That(result.IsCompleted, Is.EqualTo(updateTaskDto.IsCompleted));
        _dbContextMock.Verify(db => db.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Test]
    public void UpdateTask_ShouldThrowBadRequestException_WhenUserIsNotOwner()
    {
        // Mock folders and tasks
        var folderId = Guid.NewGuid();
        var folder = new FolderEntity { Id = folderId, Name = "Folder 1", OwnerId = Guid.NewGuid(), Owner = new UserEntity() { Id = Guid.NewGuid() } };
        var taskId = Guid.NewGuid();
        var folders = new List<FolderEntity>
        {
            folder
        };
        var tasks = new List<TaskEntity>
        {
            new() { Id = taskId, Content = "Task 1", FolderId = folderId, Folder = folder }
        };

        _dbContextMock.Setup(x => x.Set<FolderEntity>()).ReturnsDbSet(folders);
        _dbContextMock.Setup(x => x.Set<TaskEntity>()).ReturnsDbSet(tasks);

        // Arrange
        var updateTaskDto = new UpdateTaskDto
        {
            Id = taskId,
            Content = "Updated Task",
            IsCompleted = true,
            FolderId = folderId
        };
        var cancellationToken = CancellationToken.None;

        // Act
        var exception = Assert.ThrowsAsync<ForbiddenException>(() => _taskService.UpdateTask(updateTaskDto, cancellationToken));

        // Assert
        Assert.IsNotNull(exception);
    }

    [Test]
    public void DeleteTask_ShouldThrowBadRequestException_WhenTaskNotFound()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        _dbContextMock.Setup(x => x.Set<FolderEntity>()).ReturnsDbSet([]);
        _dbContextMock.Setup(x => x.Set<TaskEntity>()).ReturnsDbSet([]);

        // Act
        var exception = Assert.ThrowsAsync<BadRequestException>(() => _taskService.DeleteTask(taskId, cancellationToken));

        // Assert
        Assert.IsNotNull(exception);
    }

    [Test]
    public void DeleteTask_ShouldThrowForbiddenException_WhenUserIsNotOwner()
    {
        // Mock folders and tasks
        var folderId = Guid.NewGuid();
        var folder = new FolderEntity { Id = folderId, Name = "Folder 1", OwnerId = Guid.NewGuid(), Owner = new UserEntity() { Id = Guid.NewGuid() } };
        var taskId = Guid.NewGuid();
        var folders = new List<FolderEntity>
        {
            folder
        };
        var tasks = new List<TaskEntity>
        {
            new() { Id = taskId, Content = "Task 1", FolderId = folderId, Folder = folder }
        };

        _dbContextMock.Setup(x => x.Set<FolderEntity>()).ReturnsDbSet(folders);
        _dbContextMock.Setup(x => x.Set<TaskEntity>()).ReturnsDbSet(tasks);

        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        var exception = Assert.ThrowsAsync<ForbiddenException>(() => _taskService.DeleteTask(taskId, cancellationToken));

        // Assert
        Assert.IsNotNull(exception);
    }
}
