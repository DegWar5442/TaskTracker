import React, { useState } from 'react';
import { Form, Button, Container, Row, Col, Spinner } from 'react-bootstrap';
import { login } from '../../api/auth/authApi';
import { useNavigate } from 'react-router-dom';
import show from './../../utils/SnackbarUtils';
import { LoginRequest } from '../../api/auth/models/login';
import { setAuth } from '../../redux/slices/authSlice';
import { useAppDispatch } from '../../redux/hooks';

const LoginPage: React.FC = () => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      setIsLoading(true);
      const model: LoginRequest = {
        userName,
        password,
      };
      const response = await login(model);
      dispatch(setAuth({ token: response.token, user: response.user }));

      navigate('/');
    } catch (error) {
      show.error('Некоректний логін або пароль');
    } finally {
      setIsLoading(false);
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      handleSubmit(e as unknown as React.FormEvent);
    }
  };

  return (
    <Container>
      <Row className="justify-content-center mt-5">
        <Col md={4}>
          <h1>Вхід у систему</h1>
          <Form
            onSubmit={handleSubmit}
            onKeyPress={handleKeyPress}
            className="mt-3"
          >
            <Form.Group controlId="userName">
              <Form.Label>Логін</Form.Label>
              <Form.Control
                type="text"
                placeholder="Введіть ваш логін"
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
              />
            </Form.Group>

            <Form.Group controlId="password" className="mt-3">
              <Form.Label>Пароль</Form.Label>
              <Form.Control
                type="password"
                placeholder="Введіть ваш пароль"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </Form.Group>

            <Button
              type="submit"
              variant="primary"
              className={`col-12 mt-3 ${isLoading ? 'disabled' : ''}`}
            >
              {isLoading ? <Spinner animation="border" size="sm" /> : 'Вхід'}
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default LoginPage;
