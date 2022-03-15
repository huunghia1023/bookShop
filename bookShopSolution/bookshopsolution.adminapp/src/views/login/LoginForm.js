import React, { useEffect, useState } from "react";
import {
  Form,
  FormGroup,
  Label,
  Input,
  Button,
  Alert,
  Spinner,
} from "reactstrap";
import "./Login.css";
import userResquest from "../../requests/UserRequest";
import AuthenticateResponseModel from "../../models/AuthenticateResponseModel";
import { useNavigate } from "react-router-dom";
import UserModel from "../../models/UserModel";
import Swal from "sweetalert2";

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
  const [loading, setLoading] = useState(false);

  var user = new UserModel();

  const onErrorDismiss = () => setLoginError({ errors: [], isError: false });
  const onSuccessDismiss = () => setLoginSuccess({ isSuccess: false });

  const GetAccountInfo = async (token) => {
    // var user = new UserModel();
    let response = await userResquest.getAccountInfo(token);
    if (response.status === 200) {
      var responseData = response.data ? response.data : "";
      if (responseData) {
        var userResponse = responseData.results ? responseData.results : "";
        if (userResponse) {
          user.id = userResponse.id ? userResponse.id : "";
          user.email = userResponse.email ? userResponse.email : "";
          user.firstName = userResponse.firstName ? userResponse.firstName : "";
          user.lastName = userResponse.lastName ? userResponse.lastName : "";
          user.username = userResponse.userName ? userResponse.userName : "";
          user.phoneNumber = userResponse.phoneNumber
            ? userResponse.phoneNumber
            : "";
          if (userResponse.birthDay) {
            var birth = new Date(userResponse.birthDay);
            birth = birth
              .toLocaleDateString("pt-br")
              .split("/")
              .reverse()
              .join("-");
            user.birthDay = birth;
          }

          user.emailVerified = userResponse.emailVerified
            ? userResponse.emailVerified
            : false;
          user.roles = userResponse.roles ? userResponse.roles : "";
          user.id = userResponse.id ? userResponse.id : "";
        }
      }
    }
  };

  const CloseAlert = () => {
    window.setTimeout(() => {
      setLoginError({ errors: [], isError: false });
    }, 2000);
  };

  const Login = async (e) => {
    e.preventDefault();
    // validate
    if (!username || !password || password.length < 6) {
      setLoginError({
        isError: true,
        errors: ["Username or Password invalid"],
      });
      return;
    }
    try {
      setLoading(true);
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
          //CloseAlert();
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
            localStorage.setItem("token", responseModel.accessToken);
            setLoginSuccess({ messages: "Login Success!", isSuccess: true });
            setLoading(false);
            navigate("/home", { replace: true });
            return;
          }
        }

        setLoginError({
          errors: ["Can not get access token"],
          isError: true,
        });
        setLoading(false);
      }
    } catch (error) {
      setLoading(false);
      if (error.response.data.message) {
        setLoginError({
          isError: true,
          errors: [error.response.data.message],
        });
      } else if (error.response.data.errors) {
        let messageErrors = [];
        for (const [key, value] of Object.entries(error.response.data.errors)) {
          messageErrors.push(value);
        }
        setLoginError({
          isError: true,
          errors: messageErrors,
        });
      } else {
        setLoginError({
          isError: true,
          errors: error,
        });
      }
      //CloseAlert();
    }
  };

  // if (loginSuccess.isSuccess) {
  //   setLoading(false);
  //   navigate("/home", { replace: true });
  // }
  return (
    <React.Fragment>
      <Form onSubmit={Login}>
        {loginError.errors.map((el, index) => (
          <Alert
            key={index}
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
        <Button type="submit">
          Login
          {loading ? (
            <Spinner size="sm" className="loading-custom" color="warning" />
          ) : (
            ""
          )}
        </Button>
      </Form>
    </React.Fragment>
  );
}

export default LoginForm;
