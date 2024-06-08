import api from "../api";
import { AuthResponse, LoginRequest } from "./models/login";
import { RegisterRequest } from "./models/register";

const AUTH = "/auth";

export async function login(request: LoginRequest): Promise<AuthResponse> {
  const response = await api.post<AuthResponse>(`${AUTH}/login`, request);
  return response.data;
}

export async function register(
  request: RegisterRequest
): Promise<AuthResponse> {
  const reponse = await api.post(`${AUTH}/register`, request);
  return reponse.data;
}

export async function me(): Promise<AuthResponse> {
  const reponse = await api.get(`${AUTH}/me`);
  return reponse.data;
}