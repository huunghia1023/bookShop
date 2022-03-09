import {
  Card,
  CardBody,
  CardTitle,
  Button,
  Form,
  FormGroup,
  Label,
  Input,
} from "reactstrap";
import "./ManageUser.css";
import { useState, useEffect } from "react";
import UserModel from "../../models/UserModel";
import userResquest from "../../requests/UserRequest";
import Swal from "sweetalert2";
import { useNavigate, useParams } from "react-router-dom";
import UserRequestModel from "../../models/UserRequestModel";

const UserFormUpdate = (props) => {
  const [lastName, setLastName] = useState("");
  const [firstName, setFirstName] = useState("");
  const [userName, setUserName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [birthDay, setBirthDay] = useState("");
  const [email, setEmail] = useState("");
  const [admin, setAdmin] = useState(false);
  const [customer, setCustomer] = useState(false);
  let params = useParams();
  let navigate = useNavigate();
  var userId = params.id ? params.id : "";
  useEffect(() => {
    if (!userId) {
      GetUserInfo();
    }
  }, []);
  const GetUserInfo = async () => {
    try {
      let token = sessionStorage.getItem("token");
      if (token) {
        var user = new UserModel();
        let response = await userResquest.getInfo(token);
        if (response.status === 200) {
          var responseData = response.data ? response.data : "";
          if (!responseData) {
            await Swal.fire({
              icon: "error",
              title: "Error: Can not get account",
              showConfirmButton: true,
            });

            return;
          }
          var userResponse = responseData.results ? responseData.results : "";
          if (userResponse) {
            user.id = userResponse.id ? userResponse.id : "";
            user.email = userResponse.email ? userResponse.email : "";
            user.firstName = userResponse.firstName
              ? userResponse.firstName
              : "";
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

            setBirthDay(user.birthDay);
            setEmail(user.email);
            setFirstName(user.firstName);
            setLastName(user.lastName);
            setPhoneNumber(user.phoneNumber);
            setUserName(user.username);
            if (user.roles.includes("admin")) setAdmin(true);
            if (user.roles.includes("customter")) setCustomer(true);
          }

          // await Swal.fire({
          //     icon: 'success',
          //     title: 'Load user success',
          //     showConfirmButton: true
          // });
        } else {
          await Swal.fire({
            icon: "error",
            title: "Can not get list user in system",
            showConfirmButton: true,
          });
        }
      }
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error,
        showConfirmButton: true,
      });
    }
  };

  const UpdateUser = async () => {
    try {
      var userId = params.id ? params.id : "";
      if (!userId) {
        navigate("/home/manage-user", { replace: true });
        return;
      }
      let token = sessionStorage.getItem("token");
      if (token) {
        var listRole = [];
        if (admin) listRole.push("admin");
        if (customer) listRole.push("customer");
        let requestModel = new UserRequestModel();
        let requestFormData = requestModel.GetUpdateUserFormData(
          firstName,
          lastName,
          phoneNumber,
          email,
          birthDay,
          listRole
        );
        let response = await userResquest.update(
          userId,
          requestFormData,
          token
        );
        if (response.status === 200) {
          var responseData = response.data ? response.data : "";
          if (!responseData) {
            await Swal.fire({
              icon: "error",
              title: "Error: Can not create user",
              showConfirmButton: true,
            });

            return;
          }
          var idUserResponse = responseData.results ? responseData.results : "";
          await Swal.fire({
            icon: "success",
            title: "Update user success",
          });
        }
      }
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error,
        showConfirmButton: true,
      });
    }
  };

  return (
    <div>
      <Card>
        <CardBody className="position-relative form-width-center">
          <CardTitle tag="h2" className="center margin-bottom-40">
            Account
          </CardTitle>
          <Form>
            <FormGroup>
              <Label for="accountname">Username</Label>
              <Input
                value={userName}
                onChange={(e) => setUserName(e.target.value)}
                id="accountname"
                name="accountname"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="firstname">First Name</Label>
              <Input
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                id="firstname"
                name="firstname"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="lasttname">Last Name</Label>
              <Input
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                id="lasttname"
                name="lasttname"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="email">Email</Label>
              <Input
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                id="email"
                name="email"
                type="email"
              />
            </FormGroup>
            <FormGroup>
              <Label for="birthday">Birth Day</Label>
              <Input
                value={birthDay}
                onChange={(e) => {
                  setBirthDay(e.target.value);
                  console.log(e.target.value);
                }}
                id="birthDay"
                name="birthDay"
                type="date"
              />
            </FormGroup>
            <FormGroup>
              <Label for="phone">Phone Number</Label>
              <Input
                value={phoneNumber}
                onChange={(e) => setPhoneNumber(e.target.value)}
                id="phone"
                name="phone"
                type="text"
              />
            </FormGroup>
            <FormGroup className="margin-bottom-0">
              <Label check>Roles</Label>
            </FormGroup>
            <FormGroup check inline>
              <Input
                type="checkbox"
                checked={admin}
                onChange={(e) => setAdmin(e.target.value)}
              />
              <Label check>Admin</Label>
            </FormGroup>
            <FormGroup check inline>
              <Input
                type="checkbox"
                checked={customer}
                onChange={(e) => setCustomer(e.target.value)}
              />
              <Label check>Customer</Label>
            </FormGroup>
            <br />
            <Button onClick={UpdateUser} active block>
              Update
            </Button>
          </Form>
        </CardBody>
      </Card>
    </div>
  );
};
export default UserFormUpdate;
