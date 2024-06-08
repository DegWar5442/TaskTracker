import {
  Route,
  createBrowserRouter,
  createRoutesFromElements,
} from "react-router-dom";
import HomePage from "./pages/HomePage/HomePage";
import { Layout } from "./components/Layout/Layout";
import RegisterPage from "./pages/RegisterPage/RegisterPage";
import LoginPage from "./pages/LoginPage/LoginPage";
import NotFoundPage from "./pages/NotFoundPage/NotFoundPage";
import FoldersPage from "./pages/FoldersPage/FoldersPage";
import FolderEditPage from "./pages/FolderEditPage/FolderEditPage";
import AboutPage from "./pages/AboutPage/AboutPage";
import TasksPage from "./pages/TasksPage/TasksPage";
import TaskEditPage from "./pages/TaskEditPage/TaskEditPage";

export const router = createBrowserRouter(
  createRoutesFromElements(
    <>
      <Route path="/" element={<Layout />}>
        <Route index element={<HomePage />}></Route>
        <Route path="/about" element={<AboutPage />}></Route>
        <Route path="/register" element={<RegisterPage />}></Route>
        <Route path="/login" element={<LoginPage />}></Route>
        <Route path="/folders" element={<FoldersPage/>}></Route>
        <Route path="/folders/edit/:folderId" element={<FolderEditPage />} />
        <Route path="/tasks/edit/:taskId" element={<TaskEditPage />} />
        <Route path="/tasks" element={<TasksPage/>} />
        <Route path="*" element={<NotFoundPage />} />
      </Route>
    </>
  )
);
