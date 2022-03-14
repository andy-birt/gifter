import React, { useContext } from "react";
import { Link } from "react-router-dom";
import { Card, CardImg, CardBody, Row, Col, ListGroup } from "reactstrap";
import { UserContext } from "../providers/UserProvider";
import Comment from "./Comment";
import EditDeletePost from "./EditDeletePost";

const Post = ({ post }) => {
  
  const { currentUser } = useContext(UserContext);

  return (
      <Card className="mt-4">
        <Row xs="1" sm="1" md="2" lg="2">
          <Col>
            <CardImg src={post.imageUrl} alt={post.title} />
          </Col>
          <Col>
            <CardBody>
              <p className="text-left px-2">Posted by: <Link to={`/users/${post.userProfileId}`}>{post.userProfile.name}</Link></p>
              <p className="text-left px-2">
                <Link to={`/posts/${post.id}`}>
                  <strong>{post.title}</strong>
                </Link>
              </p>
              <p className="text-left px-2">{post.caption}</p>
              { currentUser.id === post.userProfileId && <EditDeletePost id={post.id} /> }
            </CardBody>
          </Col>
        </Row>
        <ListGroup flush>
          {post.comments.map( comment => <Comment key={comment.id} comment={comment} />)}
        </ListGroup>
      </Card>
    
  );
};

export default Post;