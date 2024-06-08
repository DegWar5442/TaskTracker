import React, { useEffect } from "react";
import { useAppSelector } from "../../redux/hooks";
import { useNavigate } from "react-router";
import { Spinner } from "react-bootstrap";

const HomePage: React.FC = () => {
  const user = useAppSelector((state) => state.auth.user);
  const navigate = useNavigate();

  useEffect(() => {
    if (!user) {
      navigate("/about");
    } else {
      navigate("/folders");
    }
  }, [user, navigate]);

  return <Spinner animation="border" role="status"></Spinner>;
};

export default HomePage;
