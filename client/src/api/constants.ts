if (!process.env.REACT_APP_BACKEND_URL) {
  throw new Error("REACT_APP_BACKEND_URL is not defined");
}

export const BackendUrl: URL = new URL(process.env.REACT_APP_BACKEND_URL);
