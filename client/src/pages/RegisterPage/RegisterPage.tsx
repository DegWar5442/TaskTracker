import React, { useState } from 'react';
import { Form, Button, Container, Row, Col, Spinner } from 'react-bootstrap';
import { register } from '../../api/auth/authApi';
import { RegisterRequest } from '../../api/auth/models/register';
import { useNavigate } from 'react-router';
import show from './../../utils/SnackbarUtils';
import { setAuth } from '../../redux/slices/authSlice';
import { useAppDispatch } from '../../redux/hooks';

const RegisterPage: React.FC = () => {
  const [login, setLogin] = useState('');
  const [password, setPassword] = useState('');
  const [passwordErrors, setPasswordErrors] = useState<string[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const validatePassword = (password: string): string[] => {
    const errors = [];
    if (!/(?=.*[a-z])/.test(password)) {
      errors.push('Пароль має містити хоча б одну малу літеру');
    }
    if (!/(?=.*[A-Z])/.test(password)) {
      errors.push('Пароль має містити хоча б одну велику літеру');
    }
    if (!/(?=.*\d)/.test(password)) {
      errors.push('Пароль має містити хоча б одну цифру');
    }
    if (!/(?=.*[#$^+=!*()@%&])/.test(password)) {
      errors.push('Пароль має містити хоча б один спеціальний символ');
    }
    if (!/.{8,}/.test(password)) {
      errors.push('Пароль має містити принаймні 8 символів');
    }
    return errors;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    const passwordValidationErrors = validatePassword(password);
    if (passwordValidationErrors.length > 0) {
      setPasswordErrors(passwordValidationErrors);
      return;
    }

    try {
      const model: RegisterRequest = {
        userName: login,
        password,
      };
      setIsLoading(true);
      const response = await register(model);
      dispatch(setAuth({ token: response.token, user: response.user }));
      show.success('Акаунт успішно створено');

      navigate('/');
    } catch (error) {
      show.error('Виникли помилки під час реєстрації');
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
          <h1>Реєстрація</h1>
          <Form
            onSubmit={handleSubmit}
            onKeyPress={handleKeyPress}
            className="mt-3"
          >
            <Form.Group controlId="login">
              <Form.Label>Логін</Form.Label>
              <Form.Control
                type="text"
                placeholder="Введіть ваш логін"
                value={login}
                onChange={(e) => setLogin(e.target.value)}
              />
            </Form.Group>

            <Form.Group controlId="password" className="mt-3">
              <Form.Label>Пароль</Form.Label>
              <Form.Control
                type="password"
                placeholder="Введіть ваш пароль"
                value={password}
                onChange={(e) => {
                  setPassword(e.target.value);
                  setPasswordErrors([]); // Clear error messages on change
                }}
                isInvalid={passwordErrors.length > 0}
              />
              <Form.Control.Feedback type="invalid">
                {passwordErrors.map((error, index) => (
                  <div key={index}>{error}</div>
                ))}
              </Form.Control.Feedback>
            </Form.Group>

            <Button
              type="submit"
              variant="primary"
              className={`col-12 mt-3 ${isLoading ? 'disabled' : ''}`}
            >
              {isLoading ? <Spinner animation="border" size="sm" /> : 'Зареєструватись'}
            </Button>
          </Form>
        </Col>
      </Row>
    </Container>
  );
};

export default RegisterPage;
