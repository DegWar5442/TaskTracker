namespace TaskTracker.Web.Services.Folder.Dtos;

public record UpdateFolderDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
