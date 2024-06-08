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
import { FolderDto } from "../../api/folders/foldersModels";
import {
  deleteFolder,
  getFolders,
  createFolder,
} from "../../api/folders/foldersApi";
import { FaEdit, FaTrash } from "react-icons/fa";
import "./FoldersPage.css";
import { Link, useNavigate } from "react-router-dom";
import show from '../../utils/SnackbarUtils';

const FoldersPage: React.FC = () => {
  const [folders, setFolders] = useState<FolderDto[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);
  const [showModal, setShowModal] = useState(false);
  const [newFolderName, setNewFolderName] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const fetchFolders = useCallback(async () => {
    try {
      setLoading(true);
      const response = await getFolders({ page: currentPage, pageSize: 9 });
      setFolders(response.items);
      setHasNextPage(response.hasNext);
      setHasPreviousPage(response.hasPrevious);
      setTotalPages(response.totalPages);
    } catch (error: any) {
      console.error(error);
    } finally {
      setLoading(false);
    }
  }, [currentPage]);

  useEffect(() => {
    fetchFolders();
  }, [fetchFolders]);

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

  const handleEditFolder = (folderId: string) => {
    navigate(`/folders/edit/${folderId}`);
  };

  const handleDeleteFolder = async (folderId: string) => {
    await deleteFolder(folderId);
    show.success("Папку було успішно видалено!");
    const response = await getFolders({ page: currentPage, pageSize: 9 });

    if (response.items.length === 0 && currentPage > 1) {
      setCurrentPage((prevPage) => prevPage - 1);
    } else {
      setFolders(response.items);
      setHasNextPage(response.hasNext);
      setHasPreviousPage(response.hasPrevious);
      setTotalPages(response.totalPages);
    }
  };

  const handleShowModal = () => setShowModal(true);
  const handleCloseModal = () => setShowModal(false);

  const handleAddFolder = async () => {
    await createFolder({ name: newFolderName });
    setNewFolderName("");
    setShowModal(false);
    await fetchFolders();
  };

  return (
    <Container className="folders-container">
      <Row className="d-flex justify-content-between align-items-center mb-4">
        <Col md={4}>
          <h3>Папки</h3>
        </Col>
        <Col md={4} className="d-flex justify-content-end">
          <Button variant="primary" onClick={handleShowModal}>
            + Створити Папку
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
            {folders.map((folder) => (
              <Col key={folder.id} md={4} className="mb-4">
                <Card className="folder-card">
                  <Card.Body>
                    <Link
                      to={`/folders/edit/${folder.id}`}
                      className="text-reset text-decoration-none"
                    >
                      <Card.Title>{folder.name}</Card.Title>
                      <Card.Text>К-сть завдань: {folder.tasks.length}</Card.Text>
                    </Link>
                    <div className="folder-actions">
                      <FaEdit
                        className="folder-icon"
                        onClick={() => handleEditFolder(folder.id)}
                      />
                      <FaTrash
                        className="folder-icon"
                        onClick={() => handleDeleteFolder(folder.id)}
                      />
                    </div>
                  </Card.Body>
                </Card>
              </Col>
            ))}
          </Row>
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
      )}
      <Modal show={showModal} onHide={handleCloseModal}>
        <Modal.Header closeButton>
          <Modal.Title>Створення Нової Папки</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form>
            <Form.Group controlId="formFolderName">
              <Form.Label>Ім'я Папки</Form.Label>
              <Form.Control
                type="text"
                placeholder="Введіть ім'я папки"
                value={newFolderName}
                onChange={(e) => setNewFolderName(e.target.value)}
              />
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={handleCloseModal}>
            Відмінити
          </Button>
          <Button variant="primary" onClick={handleAddFolder}>
            Зберегти зміни
          </Button>
        </Modal.Footer>
      </Modal>
    </Container>
  );
};

export default FoldersPage;
