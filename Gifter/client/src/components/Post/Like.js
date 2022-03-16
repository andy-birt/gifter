import React from "react";
import { Button } from "reactstrap";

const Like = ({ likes }) => {
  return (
    <Button>
      <i className="bi bi-star mx-2"></i>
      {/** 
      //* Dont show likes unless they are more than 0  
      **/}
      { likes > 0 && <span className="mx-2">{likes}</span>}
    </Button>
  );
};

export default Like;