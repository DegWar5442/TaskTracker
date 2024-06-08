namespace TaskTracker.Web.Services.Common.Dtos;

public record PagedResultDto<TEntityDto>
{
    public PagedResultDto(int page, int pageSize, int totalCount, List<TEntityDto> items)
    {
        Page = page;
        TotalCount = totalCount;

        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        Items = items;
    }

    public int Page { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public bool HasNext => TotalPages > Page;
    public bool HasPrevious => Page > 1;

    public List<TEntityDto> Items { get; set; }
}