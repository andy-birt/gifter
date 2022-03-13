import React from "react";
import { Link } from "react-router-dom";
import { Card, CardImg, CardBody, Row, Col, ListGroup } from "reactstrap";
import Comment from "./Comment";

const Post = ({ post }) => {
  return (
      <Card className="mt-4">
        <Row xs="1" sm="1" md="2" lg="2">
          <Col>
            <CardImg src={post.imageUrl} alt={post.title} />
          </Col>
          <Col>
            <CardBody>
              <p className="text-left px-2">Posted by: {post.userProfile.name}</p>
              <p className="text-left px-2">
                <Link to={`/posts/${post.id}`}>
                  <strong>{post.title}</strong>
                </Link>
              </p>
              <p>{post.caption}</p>
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