import React, { useCallback, useEffect, useState } from "react";
import {
  Container,
  Row,
  Col,
  Card,
  Pagination,
  Button,
  Modal,
  Form,
  Spinner,
} from "react-bootstrap";
import { deleteTask, getTasks, createTask } from "../../api/tasks/tasksApi";
import { getFolders } from "../../api/folders/foldersApi";
import { FaEdit, FaTrash } from "react-icons/fa";
import "./TasksPage.css";
import { Link, useNavigate } from "react-router-dom";
import { PagedResultDto } from "../../api/pagedResultDto";
import { TaskDto, FolderDto } from "../../api/folders/foldersModels";
import show from "../../utils/SnackbarUtils";
import { useAppSelector } from "../../redux/hooks";
import { RootState } from "../../redux/store";

const TasksPage: React.FC = () => {
  const [tasks, setTasks] = useState<TaskDto[]>([]);
  const [folders, setFolders] = useState<FolderDto[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [newTaskContent, setNewTaskContent] = useState("");
  const [selectedFolder, setSelectedFolder] = useState<string>("");
  const [isCompleted, setIsCompleted] = useState(false);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const user = useAppSelector((state: RootState) => state.auth.user);

  useEffect(() => {
    if (!user) {
      navigate("/about");
    }
  }, [user, navigate]);

  const fetchTasks = useCallback(async () => {
    try {
      setLoading(true);
      const response: PagedResultDto<TaskDto> = await getTasks({
        page: currentPage,
        pageSize: 12,
      });
      setTasks(response.items);
      setHasNextPage(response.hasNext);
      setHasPreviousPage(response.hasPrevious);
      setTotalPages(response.totalPages);
    } catch (error: any) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  }, [currentPage]);

  const fetchFolders = useCallback(async () => {
    try {
      const response = await getFolders({ page: 1, pageSize: 100 });
      setFolders(response.items);
    } catch (error: any) {
      console.error(error);
    }
  }, []);

  useEffect(() => {
    fetchTasks();
    fetchFolders();
  }, [fetchTasks, fetchFolders]);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
  };

  const handlePreviousPage = () => {
    if (hasPreviousPage) {
      setCurrentPage((prevPage) => prevPage - 1);
    }
  };

  const handleNextPage = () => {
    if (hasNextPage) {
      setCurrentPage((prevPage) => prevPage + 1);
    }
  };

  const handleEditTask = (taskId: string) => {
    navigate(`/tasks/edit/${taskId}`);
  };

  const handleDeleteTask = async (taskId: string) => {
    await deleteTask(taskId);
    show.success("Завдання було видалено успішно!");
    const response = await getTasks({ page: currentPage, pageSize: 12 });

    if (response.items.length === 0 && currentPage > 1) {
      setCurrentPage((prevPage) => prevPage - 1);
    } else {
      setTasks(response.items);
      setHasNextPage(response.hasNext);
      setHasPreviousPage(response.hasPrevious);
      setTotalPages(response.totalPages);
    }
  };

  const handleShowModal = () => setShowModal(true);
  const handleCloseModal = () => setShowModal(false);

  const handleAddTask = async () => {
    await createTask({
      content: newTaskContent,
      folderId: selectedFolder,
      isCompleted,
    });
    show.success("Нове завдання успішно додано!");
    setNewTaskContent("");
    setSelectedFolder("");
    setIsCompleted(false);
    setShowModal(false);
    await fetchTasks();
  };

  return (
    <Container className="tasks-container">
      <Row className="d-flex justify-content-between align-items-center mb-4">
        <Col md={4}>
          <h3>
            {" "}
            <i className="bi bi-check2-square text-primary"></i> Завдання
          </h3>
        </Col>
        <Col md={4} className="d-flex justify-content-end">
          <Button variant="primary" onClick={handleShowModal}>
            + Створити Завдання
          </Button>
        </Col>
      </Row>
      {loading ? (
        <Row className="justify-content-center mt-4">
          <Col xs="auto">
            <Spinner animation="border" role="status"></Spinner>
          </Col>
        </Row>
      ) : (
        <>
          <Row className="flex-grow-1">
            {tasks.map((task) => (
              <Col key={task.id} md={3} className="mb-4">
                <Card className="task-card">
                  <Card.Body>
                    <Link
                      to={`/tasks/edit/${task.id}`}
                      className="text-reset text-decoration-none"
                    >
                      <Card.Title>{task.content}</Card.Title>
                      <Card.Text>
                        <Container className="p-0">
                          <Row className="mt-4">
                            <Col>
                              <span>Папка: {task.folder.name}</span>
                            </Col>
                          </Row>
                          <Row className="mt-2">
                            <Col>
                              {task.isCompleted ? (
                                <span className="badge bg-success">
                                  Виконано
                                </span>
                              ) : (
                                <span className="badge bg-danger">
                                  Невиконано
                                </span>
                              )}
                            </Col>
                          </Row>
                        </Container>
                      </Card.Text>
                    </Link>
                    <div className="task-actions mt-4">
                      <FaEdit
                        className="task-icon"
                        onClick={() => handleEditTask(task.id)}
                      />
                      <FaTrash
                        className="task-icon"
                        onClick={() => handleDeleteTask(task.id)}
                      />
                    </div>
                  </Card.Body>
                </Card>
              </Col>
            ))}
          </Row>
          {totalPages ? (
            <>
              <Row className="pagination-container d-flex justify-content-center align-items-center mt-4">
                <Col xs="auto" className="mb-3">
                  <Button
                    variant="primary"
                    onClick={handlePreviousPage}
                    disabled={!hasPreviousPage}
                  >
                    Попередня
                  </Button>
                </Col>
                <Col xs="auto">
                  <Pagination>
                    {Array.from({ length: totalPages }, (_, index) => (
                      <Pagination.Item
                        key={index}
                        active={index + 1 === currentPage}
                        onClick={() => handlePageChange(index + 1)}
                      >
                        {index + 1}
                      </Pagination.Item>
                    ))}
                  </Pagination>
                </Col>
                <Col xs="auto" className="mb-3">
                  <Button
                    variant="primary"
                    onClick={handleNextPage}
                    disabled={!hasNextPage}
                  >
                    Наступна
                  </Button>
                </Col>
              </Row>
            </>
          ) : (
            <div className="d-flex justify-content-center align-items-center mt-4">
              <h3>Тут поки немає завдань</h3>
            </div>
          )}
        </>
      )}
      <Modal show={showModal} onHide={handleCloseModal}>
        <Modal.Header closeButton>
          <Modal.Title>Створення Нового Завдання</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group controlId="formTaskContent">
              <Form.Label>Зміст Завдання</Form.Label>
              <Form.Control
                type="text"
                placeholder="Введіть зміст завдання"
                value={newTaskContent}
                onChange={(e) => setNewTaskContent(e.target.value)}
              />
            </Form.Group>
            <Form.Group controlId="formTaskFolder" className="mt-3">
              <Form.Label>Папка</Form.Label>
              <Form.Control
                as="select"
                value={selectedFolder}
                onChange={(e) => setSelectedFolder(e.target.value)}
              >
                <option value="">Виберіть Папку</option>
                {folders.map((folder) => (
                  <option key={folder.id} value={folder.id}>
                    {folder.name}
                  </option>
                ))}
              </Form.Control>
            </Form.Group>
            <Form.Group controlId="formTaskCompleted" className="mt-3">
              <Form.Check
                type="checkbox"
                label="Виконано"
                checked={isCompleted}
                onChange={(e) => setIsCompleted(e.target.checked)}
              />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Відмінити
          </Button>
          <Button
            variant="primary"
            onClick={handleAddTask}
            className={`${
              !selectedFolder ||
              selectedFolder === "Select Folder" ||
              !newTaskContent
                ? "disabled"
                : ""
            }`}
          >
            Створити
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
};

export default TasksPage;
