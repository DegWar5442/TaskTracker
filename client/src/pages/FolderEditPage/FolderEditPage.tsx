import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getFolderById, updateFolder } from "../../api/folders/foldersApi";
import { FolderDto } from "../../api/folders/foldersModels";
import show from "../../utils/SnackbarUtils";
import {
  Button,
  Card,
  ListGroup,
  Spinner,
  Container,
  Row,
  Col,
  Form,
} from "react-bootstrap";
import { deleteTask } from "../../api/tasks/tasksApi";

const FolderEditPage: React.FC = () => {
  const { folderId } = useParams<{ folderId: string }>();
  const [folder, setFolder] = useState<FolderDto | null>(null);
  const [folderName, setFolderName] = useState<string>("");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchFolder = async () => {
      try {
        if (!folderId) {
          show.error("ID папки обов'язкове");
          return;
        }
        const fetchedFolder = await getFolderById(folderId);
        setFolder(fetchedFolder);
        setFolderName(fetchedFolder.name);
      } catch (error) {
        console.error("Помилка при отриманні папки:", error);
      }
    };

    fetchFolder();
  }, [folderId]);

  const handleDeleteTask = async (taskId: string) => {
    try {
      await deleteTask(taskId);
      setFolder((prevFolder) => {
        if (prevFolder) {
          const updatedTasks = prevFolder.tasks.filter(
            (task) => task.id !== taskId
          );
          return { ...prevFolder, tasks: updatedTasks };
        }
        return prevFolder;
      });
      show.success("Завдання видалено успішно!");
    } catch (error) {
      console.error("Failed to delete task:", error);
      show.error("Помилка при видаленні завдання");
    }
  };

  const handleSaveFolder = async () => {
    if (!folder) return;
    try {
      const updatedFolder = { ...folder, name: folderName };
      await updateFolder(updatedFolder);
      setFolder(updatedFolder);
      show.success("Папку оновлено успішно");
      navigate("/folders");
    } catch (error) {
      console.error("Помилка при збереженні папки:", error);
      show.error("Помилка при збереженні папки");
    }
  };

  return (
    <Container>
      {folder ? (
        <Row className="mt-4">
          <Col md={6}>
            <Card>
              <Card.Header>
                <Form.Group controlId="folderName">
                  <Form.Label>Назва Папки</Form.Label>
                  <Form.Control
                    type="text"
                    value={folderName}
                    onChange={(e) => setFolderName(e.target.value)}
                  />
                </Form.Group>
                <Button
                  variant="primary"
                  className="mt-3"
                  onClick={handleSaveFolder}
                >
                  Оновити
                </Button>
              </Card.Header>
            </Card>
          </Col>
          <Col md={6}>
            <Card>
              <Card.Body>
                <div className="mb-2">
                  <span className="me-2">
                    <i className="bi bi-check-circle-fill text-success"></i>{" "}
                    Виконано:{" "}
                    {folder?.tasks.filter((task) => task.isCompleted).length}
                  </span>
                  <span>
                    <i className="bi bi-x-circle-fill text-danger"></i>{" "}
                    Невиконано:{" "}
                    {folder?.tasks.filter((task) => !task.isCompleted).length}
                  </span>
                </div>
                <ListGroup className="shadow-none">
                  {folder.tasks.map((task) => (
                    <ListGroup.Item
                      key={task.id}
                      className="d-flex justify-content-between align-items-center shadow-lg mt-2 list-item-card"
                      //   style={{ cursor: "pointer" }}
                    >
                      <div>
                        {task.content}
                        {task.isCompleted && (
                          <span className="badge bg-success ms-2">
                            Виконано
                          </span>
                        )}
                      </div>
                      <Button
                        variant="danger"
                        onClick={() => handleDeleteTask(task.id)}
                      >
                        Видалити
                      </Button>
                    </ListGroup.Item>
                  ))}
                </ListGroup>
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

export default FolderEditPage;
