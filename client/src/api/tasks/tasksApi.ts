import api from "../api";
import { CreateTaskDto, TaskDto, UpdateTaskDto } from "../folders/foldersModels";
import { PagedRequestDto, PagedResultDto } from "../pagedResultDto";

const TASKS = "tasks";

export async function getTasks(
  pagedRequestDto: PagedRequestDto
): Promise<PagedResultDto<TaskDto>> {
  try {
    const response = await api.get(`${TASKS}/all`, {
      params: pagedRequestDto,
    });
    return response.data;
  } catch (error: any) {
    console.error(error);
    throw new Error(error.response.data);
  }
}

export async function getTaskById(taskId: string): Promise<TaskDto> {
  try {
    const response = await api.get(`${TASKS}/${taskId}`);
    return response.data;
  } catch (error: any) {
    console.error(error);
    throw new Error(error.response.data);
  }
}

export async function updateTask(updateTaskModel: UpdateTaskDto): Promise<TaskDto> {
  try {
    const response = await api.put(`${TASKS}/update`, updateTaskModel);
    return response.data;
  } catch (error: any) {
    console.error(error);
    throw new Error(error.response.data);
  }
}

export async function createTask(task: CreateTaskDto): Promise<TaskDto> {
  try {
    const response = await api.post(`${TASKS}/create`, task);
    return response.data;
  } catch (error: any) {
    throw new Error(error.response.data);
  }
}

export async function deleteTask(taskId: string) {
  try {
    const response = await api.delete(`${TASKS}/delete/${taskId}`);
    return response.data;
  } catch (error: any) {
    throw new Error(error.response.data);
  }
}

