import React from "react";
import { Row, Col, CardImg, CardText } from "reactstrap";

const User = ({ user }) => {
  return (
    <Row sm="1" className="pt-5">
      <Col md="4">
        { user && user.imageUrl ? <CardImg src={user.imageUrl} /> : <CardImg src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png" />}
      </Col>
      <Col md="8">
        <CardText>{user?.name}</CardText>
        <CardText>{user?.email}</CardText>
        <CardText>{user?.bio}</CardText>
      </Col>
    </Row>
  );
};

export default User;