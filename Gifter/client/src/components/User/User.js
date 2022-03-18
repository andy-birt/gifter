import React, { useContext } from "react";
import { Row, Col, CardImg, CardText, Button } from "reactstrap";
import { UserContext } from "../../providers/UserProvider";

const User = ({ user, currentUser, providerUsers }) => {

  const { addSubscription, removeSubscription } = useContext(UserContext);

  const handleSubscribe = () => {
    addSubscription(currentUser.id, user.id);
  };

  const handleUnsubscribe = () => {
    removeSubscription(currentUser.id, user.id);
  };

  return (
    <Row sm="1" className="pt-5">
      <Col md="4">
        { //* If user doesn't have an image url then present a default
        user && user.imageUrl ? 
        <CardImg src={user.imageUrl} /> : 
        <CardImg src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png" />}
      </Col>
      <Col md="8">
        <CardText>{user.name}</CardText>
        <CardText>{user.email}</CardText>
        <CardText>{user.bio}</CardText>
        <CardText>
          { //* Depending the current user is subscribed to user will determine if they may sub or unsub
          providerUsers.find(u => u.id === user.id) ? 
          <Button onClick={handleUnsubscribe}>Unsubscribe</Button> : 
          <Button onClick={handleSubscribe} >Subscribe</Button>}
        </CardText>
      </Col>
    </Row>
  );
};

export default User;