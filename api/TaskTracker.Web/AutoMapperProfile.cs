using AutoMapper;
using TaskTracker.Data.Entities;
using TaskTracker.Web.Services.Auth.Dtos;
using TaskTracker.Web.Services.Folder.Dtos;
using TaskTracker.Web.Services.Tasks.Dtos;

namespace TaskTracker.Web;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<FolderEntity, FolderDto>().ReverseMap();
        CreateMap<CreateFolderDto, FolderEntity>();
        CreateMap<UpdateFolderDto, FolderEntity>();

        CreateMap<TaskEntity, TaskDto>().ReverseMap();
        CreateMap<CreateTaskDto, TaskEntity>();
        CreateMap<UpdateTaskDto, TaskEntity>();

        CreateMap<UserEntity, UserDto>().ReverseMap();
        CreateMap<RegisterUserDto, UserEntity>();
    }
}
