export interface PagedResultDto<TEntityDto> {
    page: number;
    totalCount: number;
    totalPages: number;
    hasNext: boolean;
    hasPrevious: boolean;
    items: TEntityDto[];
}

export interface PagedRequestDto {
    page: number;
    pageSize: number;
    sorting?: string | null;
}