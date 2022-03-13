import React from "react";
import "./loader.scss";
import { Spinner } from "reactstrap";

const Loader = () => (
  <Spinner size="sm" type="grow" color="warning" />
  // <div className="fallback-spinner">
  //   <div className="loading">

  //   </div>
  // </div>
);
export default Loader;
