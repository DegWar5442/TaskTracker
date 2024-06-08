import { CreateFolderDto, FolderDto, UpdateFolderDto } from "./foldersModels";
import { PagedRequestDto, PagedResultDto } from "../pagedResultDto";
import api from "../api";

const FOLDERS = "folders";

export async function getFolders(
  pagedRequestDto: PagedRequestDto
): Promise<PagedResultDto<FolderDto>> {
  try {
    const response = await api.get(`${FOLDERS}/all`, {
      params: pagedRequestDto,
    });
    return response.data;
  } catch (error: any) {
    console.error(error);
    throw new Error(error.response.data);
  }
}

export async function createFolder(createFolderDto: CreateFolderDto) {
  try {
    const response = await api.post(`${FOLDERS}/create`, createFolderDto);
    return response.data;
  } catch (error: any) {
    throw new Error(error.response.data);
  }
}

export async function deleteFolder(folderId: string) {
  try {
    const response = await api.delete(`${FOLDERS}/delete/${folderId}`);
    return response.data;
  } catch (error: any) {
    throw new Error(error.response.data);
  }
}

export async function getFolderById(folderId: string): Promise<FolderDto> {
  try {
    const response = await api.get(`${FOLDERS}/${folderId}`);
    return response.data;
  } catch (error: any) {
    console.error(error);
    throw new Error(error.response.data);
  }
}

export async function updateFolder(
  folder: UpdateFolderDto
): Promise<FolderDto> {
  try {
    const response = await api.put(`${FOLDERS}/update`, folder);
    return response.data;
  } catch (error: any) {
    console.error(error);
    throw new Error(error.response.data);
  }
}
