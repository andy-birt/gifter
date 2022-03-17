import React from "react";
import { Link } from "react-router-dom";
import { Card, CardImg, CardBody, Row, Col, ListGroup } from "reactstrap";
import Comment from "../Comment/Comment";
import EditDeletePost from "./EditDeletePost";
import Like from "./Like";

const Post = ({ post }) => {

  const currentUser = JSON.parse(localStorage.getItem('gifterUser'));

  //! I'm not sure localStorage plays well with the Context/State API
  // const { currentUser } = useContext(UserContext);

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
              <Like likes={post.likes} postId={post.id} />
              {' '}
              {/**
               //* If this post belongs to current user give them options to edit or delete 
               **/}
              { currentUser.id === post.userProfileId && <EditDeletePost id={post.id} /> }
            </CardBody>
          </Col>
        </Row>
        { post.comments.length > 0 &&
        <ListGroup flush>
          {post.comments.map( comment => <Comment key={comment.id} comment={comment} />)}
        </ListGroup>
        }
      </Card>
  );
};

export default Post;