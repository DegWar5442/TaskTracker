export interface CreateFolderDto {
  name: string;
}

export interface UpdateFolderDto {
  id: string;
  name: string;
}

export interface TaskDto {
  id: string;
  folder: FolderDto;
  content: string;
  isCompleted: boolean;
}

export interface CreateTaskDto {
  content: string;
  isCompleted: boolean;
  folderId: string;
}

export interface UpdateTaskDto {
  id: string;
  content: string;
  isCompleted: boolean;
  folderId: string;
}

export interface FolderDto {
  id: string;
  name: string;
  tasks: TaskDto[];
}
