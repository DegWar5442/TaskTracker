import React from "react";
import { Button } from "react-bootstrap";
import { Link, NavLink, useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import { clearAuth } from "../../redux/slices/authSlice";
import { RootState } from "../../redux/store";

export const Header: React.FC = () => {
  const dispatch = useDispatch();
  const token = useSelector((state: RootState) => state.auth.token);
  const userName = useSelector((state: RootState) => state.auth.user?.userName);
  const navigate = useNavigate();

  const isLoggedIn = !!token;

  const signOut = () => {
    dispatch(clearAuth());
    // show.info('You have been signed out.');
    window.location.reload();
    navigate("/", { replace: true });
  };

  return (
    <header className="mt-3 mb-3">
      <nav
        className="navbar navbar-expand-lg bg-transparent"
        data-bs-theme="light"
      >
        <div className="container-fluid">
          {isLoggedIn && (
            <Link className="navbar-brand" to="/">
              Домашня сторінка
            </Link>
          )}

          <div className="collapse navbar-collapse" id="navbarColor03">
            {isLoggedIn ? (
              <ul className="navbar-nav me-auto">
                <li className="nav-item">
                  <NavLink className="nav-link" to="folders">
                    Папки
                  </NavLink>
                </li>
                <li className="nav-item">
                  <NavLink className="nav-link" to="tasks">
                    Завдання
                  </NavLink>
                </li>
                <li className="nav-item">
                  <NavLink className="nav-link" to="/about">
                    Про Нас
                  </NavLink>
                </li>
              </ul>
            ) : (
              <div className="me-auto"></div>
            )}
            {isLoggedIn ? (
              <>
                <span className="navbar-text me-3 fw-bold">Ласкаво просимо, {userName}!</span>
                <Button variant="primary" className="ms-3" onClick={signOut}>
                  Вихід
                </Button>
              </>
            ) : (
              <>
                <Link to="/login" className="text-decoration-none text-reset">
                  <Button variant="primary" className="ms-3">
                    Вхід
                  </Button>
                </Link>
                <Link
                  to="/register"
                  className="text-decoration-none text-reset"
                >
                  <Button variant="outline-primary" className="ms-3">
                    Реєстрація
                  </Button>
                </Link>
              </>
            )}
          </div>
        </div>
      </nav>
    </header>
  );
};
