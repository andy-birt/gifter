import React, { useContext, useEffect } from "react";
import { useParams } from "react-router-dom";
import { CardImg, CardText, Col, Container, Row, Button } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";
import { UserContext } from "../../providers/UserProvider";
import Post from "./Post";

const UserPosts = () => {

  const { getAllPostsByUser } = useContext(PostContext);

  const { user, providerUsers, addSubscription, removeSubscription, currentUser } = useContext(UserContext);

  const { id } = useParams();

  useEffect(() => {
    getAllPostsByUser(id);
  }, [id]);

  const handleSubscribe = () => {
    addSubscription(currentUser.id, user.id);
  };

  const handleUnsubscribe = () => {
    removeSubscription(currentUser.id, user.id);
  };

  //* I had to be super defensive in my approach to rendering users with posts.
  //* On initial render there would not be a user and render would happen before useEffect fires
  //* Not really sure why that is...

  return (
    <Container>
      <Row sm="1" className="pt-5">
        <Col md="4">
          { user && user.imageUrl ? <CardImg src={user.imageUrl} /> : <CardImg src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png" />}
        </Col>
        <Col md="8">
          <CardText>{user?.name}</CardText>
          <CardText>{user?.email}</CardText>
          <CardText>{user?.bio}</CardText>
          <CardText>
          { //* Depending the current user is subscribed to user will determine if they may sub or unsub
          providerUsers.find(u => u.id === user.id) ? 
          <Button onClick={handleUnsubscribe}>Unsubscribe</Button> : 
          <Button onClick={handleSubscribe} >Subscribe</Button>}
        </CardText>
        </Col>
      </Row>
      {user && user.posts && user.posts.map(post => <Post key={post.id} post={post} />)}
    </Container>
  );
};

export default UserPosts;