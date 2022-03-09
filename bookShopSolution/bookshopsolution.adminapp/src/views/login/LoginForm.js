import React, { useState } from "react";
import { Form, FormGroup, Label, Input, Button, Alert } from "reactstrap";
import "./Login.css";
import userResquest from "../../requests/UserRequest";
import AuthenticateResponseModel from "../../models/AuthenticateResponseModel";
import { useNavigate } from "react-router-dom";

function LoginForm() {
  let navigate = useNavigate();
  const [loginError, setLoginError] = useState({
    errors: [],
    isError: false,
  });
  const [loginSuccess, setLoginSuccess] = useState({
    message: "",
    isSuccess: false,
  });
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(true);

  const onErrorDismiss = () => setLoginError({ isError: false });
  const onSuccessDismiss = () => setLoginSuccess({ isSuccess: false });

  const Login = async (e) => {
    e.preventDefault();
    try {
      let response = await userResquest.authenticate(
        JSON.stringify({
          UserName: username,
          Password: password.toString(),
          RememberMe: true,
        })
      );
      if (response.status === 200) {
        var responseData = response.data ? response.data : "";
        if (!responseData) {
          setLoginError({
            isError: true,
            errors: ["Can not get authenticate"],
          });
          return;
        }
        var results = responseData.results ? responseData.results : "";
        if (results) {
          var responseModel = new AuthenticateResponseModel();
          responseModel.accessToken = results.access_token
            ? results.access_token
            : "";
          responseModel.email = results.email ? results.email : "";
          responseModel.expires = results.expires_in ? results.expires_in : 0;
          responseModel.firstName = results.firstName ? results.firstName : "";
          responseModel.lastName = results.lastName ? results.lastName : "";
          responseModel.roles = results.roles ? results.roles : "";
          responseModel.scope = results.scope ? results.scope : "";
          responseModel.tokenType = results.token_type
            ? results.token_type
            : "";
          if (responseModel.accessToken) {
            window.sessionStorage.setItem("token", responseModel.accessToken);
            setLoginSuccess({ messages: "Login Success!", isSuccess: true });

            return;
          }
        }

        setLoginError({
          errors: ["Can not get access token"],
          isError: true,
        });
      }
    } catch (error) {
      if (error.response.data.message) {
        setLoginError({
          isError: true,
          errors: [error.response.data.message],
        });
      } else if (error.response.data.errors) {
        let messageErrors = [];
        for (const [key, value] of Object.entries(error.response.data.errors)) {
          messageErrors.push(key + ": " + value);
        }
        setLoginError({
          isError: true,
          errors: messageErrors,
        });
        console.log(loginError);
      } else {
        setLoginError({
          isError: true,
          errors: error,
        });
      }
    }
  };
  if (loginSuccess.isSuccess) {
    navigate("/home", { replace: true });
  }
  return (
    <React.Fragment>
      <Form onSubmit={Login}>
        {loginError.errors.map((el) => (
          <Alert
            color="danger"
            isOpen={loginError.isError}
            toggle={onErrorDismiss}
          >
            {el}
          </Alert>
        ))}

        <Alert
          color="success"
          isOpen={loginSuccess.isSuccess}
          toggle={onSuccessDismiss}
        >
          Login success!
        </Alert>
        <FormGroup>
          <Label for="username">Username</Label>
          <Input
            onChange={(e) => setUsername(e.target.value)}
            type="text"
            name="username"
            id="username"
            placeholder="Username"
          />
        </FormGroup>
        <FormGroup>
          <Label for="password">Password</Label>
          <Input
            onChange={(e) => setPassword(e.target.value)}
            type="password"
            name="password"
            id="password"
            placeholder="Password"
          />
        </FormGroup>

        <FormGroup check>
          <Label check>
            <Input
              type="checkbox"
              onChange={(e) => setRememberMe(e.target.value)}
            />{" "}
            Remember me?
          </Label>
        </FormGroup>
        <Button type="submit">Login</Button>
      </Form>
    </React.Fragment>
  );
}

export default LoginForm;
