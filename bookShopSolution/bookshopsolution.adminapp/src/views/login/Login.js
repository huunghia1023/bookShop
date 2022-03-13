import React from "react";
import { Container, Row, Col, Media } from "reactstrap";
import loginPic from "../../assets/images/bg/bg1.jpg";
import "./Login.css";
import LoginForm from "../login/LoginForm";
import { Navigate } from "react-router-dom";

function Login() {
  var token = localStorage.getItem("token");
  if (token !== null) {
    // neu da login thi Redirect
    return <Navigate to="/home" />;
    //navigate("/home", {replace: true});
  }

  return (
    <React.Fragment>
      <Container className="themed-container" fluid={true}>
        <Row xs="1" md="2" className="full-height">
          <Col className="center-align margin-auto">
            <Media>
              <Media object src={loginPic} alt="Generic placeholder image" />
            </Media>
            {/* <img src={loginPic} class="img-fluid ${3|rounded-top,rounded-right,rounded-bottom,rounded-left,rounded-circle,|}" alt="ThemeLogin"/> */}
          </Col>
          <Col className="margin-auto">
            <div className="width-50p">
              <h1 className="center-align margin-bot-50">Login</h1>
              <LoginForm />
            </div>
          </Col>
        </Row>
      </Container>
    </React.Fragment>
  );
}

export default Login;
