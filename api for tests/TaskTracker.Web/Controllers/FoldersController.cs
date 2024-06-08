using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Web.Services.Common.Dtos;
using TaskTracker.Web.Services.Folder;
using TaskTracker.Web.Services.Folder.Dtos;

namespace TaskTracker.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FoldersController(IFolderService folderService) : ControllerBase
{
    [HttpGet("all")]
    public async Task<PagedResultDto<FolderDto>> GetFolders([FromQuery] PagedRequestDto pagedRequestDto, CancellationToken cancellationToken)
    {
        return await folderService.GetAllFolders(pagedRequestDto, cancellationToken);
    }

    [HttpGet("{folderId}")]
    public async Task<FolderDto> GetFolder([FromRoute] Guid folderId, CancellationToken cancellationToken)
    {
        return await folderService.GetFolder(folderId, cancellationToken);
    }

    [HttpPost("create")]
    public async Task<FolderDto> CreateFolder([FromBody] CreateFolderDto createFolderDto, CancellationToken cancellationToken)
    {
        return await folderService.CreateFolder(createFolderDto, cancellationToken);
    }

    [HttpPut("update")]
    public async Task<FolderDto> UpdateFolder([FromBody] UpdateFolderDto updateFolderDto, CancellationToken cancellationToken)
    {
        return await folderService.UpdateFolder(updateFolderDto, cancellationToken);
    }

    [HttpDelete("delete/{folderId}")]
    public async Task<IActionResult> DeleteFolder([FromRoute] Guid folderId, CancellationToken cancellationToken)
    {
        await folderService.DeleteFolder(folderId, cancellationToken);
        return Ok();
    }
}
