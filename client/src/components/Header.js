import React from "react";
import { Link } from "react-router-dom";
import { Container } from "reactstrap";

const Header = () => {
  return (
    <nav className="navbar navbar-expand navbar-dark bg-info">
      <Container>
        <Link to="/" className="navbar-brand">
          GiFTER
        </Link>
        <ul className="navbar-nav mr-auto">
          <li className="nav-item">
            <Link to="/" className="nav-link">
              Feed
            </Link>
          </li>
          <li className="nav-item">
            <Link to="/posts/add" className="nav-link">
              New Post
            </Link>
          </li>
        </ul>
      </Container>
    </nav>
  );
};

export default Header;