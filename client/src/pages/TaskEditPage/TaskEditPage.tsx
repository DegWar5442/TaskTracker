import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getTaskById, updateTask } from "../../api/tasks/tasksApi";
import { getFolders } from "../../api/folders/foldersApi";
import {
  TaskDto,
  UpdateTaskDto,
  FolderDto,
} from "../../api/folders/foldersModels";
import show from "../../utils/SnackbarUtils";
import {
  Button,
  Card,
  Spinner,
  Container,
  Row,
  Col,
  Form,
} from "react-bootstrap";
import { useAppSelector } from "../../redux/hooks";
import { RootState } from "../../redux/store";

const TaskEditPage: React.FC = () => {
  const { taskId } = useParams<{ taskId: string }>();
  const [task, setTask] = useState<TaskDto | null>(null);
  const [taskContent, setTaskContent] = useState<string>("");
  const [isCompleted, setIsCompleted] = useState<boolean>(false);
  const [selectedFolder, setSelectedFolder] = useState<string>("");
  const [folders, setFolders] = useState<FolderDto[]>([]);
  const navigate = useNavigate();
  const user = useAppSelector((state: RootState) => state.auth.user);

  useEffect(() => {
    if (!user) {
      navigate("/about");
    }
  }, [user, navigate]);

  useEffect(() => {
    const fetchTask = async () => {
      try {
        if (!taskId) {
          show.error("Task ID is required");
          return;
        }
        const fetchedTask = await getTaskById(taskId);
        setTask(fetchedTask);
        setTaskContent(fetchedTask.content);
        setIsCompleted(fetchedTask.isCompleted);
        setSelectedFolder(fetchedTask.folder.id);
      } catch (error) {
        console.error("Failed to fetch task:", error);
      }
    };

    const fetchFolders = async () => {
      try {
        const response = await getFolders({ page: 1, pageSize: 100 });
        setFolders(response.items);
      } catch (error) {
        console.error("Failed to fetch folders:", error);
      }
    };

    fetchTask();
    fetchFolders();
  }, [taskId]);

  const handleSaveTask = async () => {
    if (!task) return;
    try {
      const updatedTask: UpdateTaskDto = {
        id: task.id,
        content: taskContent,
        isCompleted: isCompleted,
        folderId: selectedFolder,
      };
      await updateTask(updatedTask);
      show.success("Завдання оновлено успішно!");
      navigate("/tasks");
    } catch (error) {
      console.error("Failed to save task:", error);
      show.error("Failed to save task");
    }
  };

  return (
    <Container>
      {task ? (
        <Row className="mt-4">
          <Col md={6}>
            <Card>
              <Card.Header>
                <Form.Group controlId="taskContent">
                  <Form.Label>Зміст Завдання</Form.Label>
                  <Form.Control
                    type="text"
                    value={taskContent}
                    onChange={(e) => setTaskContent(e.target.value)}
                  />
                </Form.Group>
                <Form.Group controlId="taskFolder" className="mt-3">
                  <Form.Label>Папка</Form.Label>
                  <Form.Control
                    as="select"
                    value={selectedFolder}
                    onChange={(e) => setSelectedFolder(e.target.value)}
                  >
                    {folders.map((folder) => (
                      <option key={folder.id} value={folder.id}>
                        {folder.name}
                      </option>
                    ))}
                  </Form.Control>
                </Form.Group>
                <Form.Group controlId="taskCompleted" className="mt-3">
                  <Form.Check
                    type="checkbox"
                    label="Виконано"
                    checked={isCompleted}
                    onChange={(e) => setIsCompleted(e.target.checked)}
                  />
                </Form.Group>
                <Button
                  variant="primary"
                  className="mt-3"
                  onClick={handleSaveTask}
                >
                  Оновити Завдання
                </Button>
              </Card.Header>
            </Card>
          </Col>
          <Col md={6}>
            <Card>
              <Card.Body>
                <Container>
                  <Row className="mb-2">
                    <Col>
                      <span className="me-2">
                        <i className="bi bi-folder-fill text-primary"></i>{" "}
                        Папка: {task.folder.name}
                      </span>
                    </Col>
                  </Row>
                  <Row>
                    <Col>
                      <span>
                        <i
                          className={`bi ${
                            isCompleted
                              ? "bi-check-circle-fill text-success"
                              : "bi-x-circle-fill text-danger"
                          }`}
                        ></i>{" "}
                        {isCompleted ? "Виконано" : "Невиконано"}
                      </span>
                    </Col>
                  </Row>
                </Container>
              </Card.Body>
            </Card>
          </Col>
        </Row>
      ) : (
        <Row className="justify-content-center mt-4">
          <Col xs="auto">
            <Spinner animation="border" role="status"></Spinner>
          </Col>
        </Row>
      )}
    </Container>
  );
};

export default TaskEditPage;
