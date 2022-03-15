import React, { useContext, useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { CardImg, CardText, Col, Container, Row } from "reactstrap";
import { PostContext } from "../../providers/PostProvider";
import Post from "./Post";

const UserPosts = () => {

  const [user, setUser] = useState({});

  const { getAllPostsByUser } = useContext(PostContext);

  const { id } = useParams();

  useEffect(() => {
    getAllPostsByUser(id).then(setUser);
  }, []);

  return (
    <Container>
      <Row sm="1" className="pt-5">
        <Col md="4">
          { user.imageUrl ? <CardImg src={user.imageUrl} /> : <CardImg src="https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png" />}
        </Col>
        <Col md="8">
          <CardText>{user.name}</CardText>
          <CardText>{user.email}</CardText>
          <CardText>{user.bio}</CardText>
        </Col>
      </Row>
      {user.posts.map(post => <Post key={post.id} post={post} />)}
    </Container>
  );
};

export default UserPosts;