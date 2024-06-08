import axios from "axios";
import { BackendUrl } from "./constants";
import { getAccessToken } from "../common/localStorage";
import show from "../utils/SnackbarUtils";

interface IdentityError {
  Code: string;
  Description: string;
}

const api = axios.create({
  baseURL: BackendUrl.toString(),
  withCredentials: true,
});

api.interceptors.request.use(
  (config) => {
    const token = getAccessToken();
    if (token) {
      config!.headers!.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

api.interceptors.response.use(
  (response) => {
    return response;
  },
  async (error) => {
    if (axios.isAxiosError(error)) {
      const { response } = error;
      if (response) {
        if (response.status === 401) {
          const token = getAccessToken();
          if (token) {
            const originalRequest = error.config;
            if (originalRequest !== undefined) {
              const token = getAccessToken();
              originalRequest!.headers!.Authorization = `Bearer ${token}`;
              return api(originalRequest);
            }
          }
          return;
        } else if (response.status >= 400 && response.status <= 499) {
          const data = response?.data as
            | { Message: string; IdentityErrors?: IdentityError[] }
            | undefined;
          if (data === undefined) {
            return Promise.reject(error);
          }

          if (data.IdentityErrors !== undefined) {
            data.IdentityErrors.forEach((error) => {
              show.error(error.Description);
            });
          } else {
            show.error(data.Message);
          }
        } else if (response.status >= 500) {
          show.error("Сервер недоступний");
        }
      }
    }

    return Promise.reject(error);
  }
);

export default api;
