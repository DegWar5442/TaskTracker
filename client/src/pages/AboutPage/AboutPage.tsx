import React from "react";
import { Container, Row, Col, Card } from "react-bootstrap";

const AboutPage: React.FC = () => {
  return (
    <Container>
      <h1 className="my-4">Про Нас</h1>
      <Row>
        <Col md={8}>
          <p>
            Ласкаво просимо на наш сайт управління завданнями! Ця платформа
            дозволяє ефективно створювати та керувати завданнями, організовуючи
            їх у папки. Незалежно від того, чи керуєте ви особистими проектами,
            командними співпрацями чи будь-яким іншим типом завдань, наш сайт
            пропонує інструменти, необхідні для підтримання організованості та
            продуктивності.
          </p>
          <h2>Особливості</h2>
          <ul>
            <li>Створення та управління завданнями</li>
            <li>Групування завдань у налаштовані папки</li>
            <li>Відстеження прогресу та виконання завдань</li>
          </ul>
          <h2>Наша Місія</h2>
          <p>
            Наша місія полягає в тому, щоб надати простий, але потужний
            інструмент управління завданнями, який допомагає окремим особам та
            командам триматися на вершині своєї роботи. Ми віримо, що за
            допомогою правильних інструментів кожен може досягти своїх цілей та
            реалізувати свій повний потенціал.
          </p>
          <h2>Зв'яжіться з Нами</h2>
          <p>
            Якщо у вас є якісь питання або відгуки, не соромтеся звертатися до
            нас за адресою{" "}
            <a href="mailto:support@taskmanagement.com">
              support@taskmanagement.com
            </a>
            . Ми завжди раді допомогти!
          </p>
        </Col>
        <Col md={4}>
          <Card>
            <Card.Img variant="top" src="about.webp" alt="Управління Завданнями" />
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default AboutPage;
