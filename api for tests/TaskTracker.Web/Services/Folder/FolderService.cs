using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Common.Exceptions;
using TaskTracker.Data;
using TaskTracker.Data.Entities;
using TaskTracker.Web.Services.Auth;
using TaskTracker.Web.Services.Common.Dtos;
using TaskTracker.Web.Services.Folder.Dtos;

namespace TaskTracker.Web.Services.Folder;

public interface IFolderService
{
    public Task<FolderDto> CreateFolder(CreateFolderDto createFolderDto, CancellationToken cancellationToken);
    public Task<PagedResultDto<FolderDto>> GetAllFolders(PagedRequestDto pagedRequestDto, CancellationToken cancellationToken);
    public Task<FolderDto> GetFolder(Guid folderId, CancellationToken cancellationToken);
    public Task DeleteFolder(Guid folderId, CancellationToken cancellationToken);
    public Task<FolderDto> UpdateFolder(UpdateFolderDto updateFolderDto, CancellationToken cancellationToken);
}

public class FolderService(IDbContext dbContext, IMapper mapper, ICurrentIdentity currentIdentity) : IFolderService
{
    private DbSet<FolderEntity> Folders => dbContext.Set<FolderEntity>();

    public async Task<FolderDto> CreateFolder(CreateFolderDto createFolderDto, CancellationToken cancellationToken)
    {
        var folderEntity = mapper.Map<FolderEntity>(createFolderDto);
        folderEntity.OwnerId = currentIdentity.GetUserGuid();

        Folders.Add(folderEntity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<FolderDto>(folderEntity);
    }

    public async Task<PagedResultDto<FolderDto>> GetAllFolders(PagedRequestDto pagedRequestDto, CancellationToken cancellationToken)
    {
        var query = Folders;

        var totalItems = await query.CountAsync(cancellationToken);

        var folders = await query
            .Where(x => x.OwnerId == currentIdentity.GetUserGuid())
            .Include(x => x.Tasks)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pagedRequestDto.Page - 1) * pagedRequestDto.PageSize)
            .Take(pagedRequestDto.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResultDto<FolderDto>(
            pagedRequestDto.Page,
            pagedRequestDto.PageSize,
            totalItems,
            mapper.Map<List<FolderDto>>(folders));
    }

    public async Task DeleteFolder(Guid folderId, CancellationToken cancellationToken)
    {
        var folderEntity = await Folders.FirstOrDefaultAsync(x => x.Id == folderId, cancellationToken)
            ?? throw new BadRequestException("Папку не знайдено");

        if (folderEntity.OwnerId != currentIdentity.GetUserGuid())
        {
            throw new ForbiddenException("Ви не є власником цієї папки");
        }

        Folders.Remove(folderEntity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<FolderDto> UpdateFolder(UpdateFolderDto updateFolderDto, CancellationToken cancellationToken)
    {
        var folderEntity = await Folders.FirstOrDefaultAsync(x => x.Id == updateFolderDto.Id, cancellationToken)
            ?? throw new BadRequestException("Папку не знайдено");

        if (folderEntity.OwnerId != currentIdentity.GetUserGuid())
        {
            throw new ForbiddenException("Ви не є власником цієї папки");
        }

        mapper.Map(updateFolderDto, folderEntity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<FolderDto>(folderEntity);
    }

    public async Task<FolderDto> GetFolder(Guid folderId, CancellationToken cancellationToken)
    {
        var folderEntity = await Folders
            .Include(x => x.Tasks)
            .FirstOrDefaultAsync(x => x.Id == folderId, cancellationToken)
            ?? throw new BadRequestException("Папку не знайдено");

        return folderEntity.OwnerId != currentIdentity.GetUserGuid()
            ? throw new ForbiddenException("Ви не є власником цієї папки")
            : mapper.Map<FolderDto>(folderEntity);
    }
}
