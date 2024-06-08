namespace TaskTracker.Web.Services.Folder.Dtos;

public record CreateFolderDto
{
    public required string Name { get; set; }
}
