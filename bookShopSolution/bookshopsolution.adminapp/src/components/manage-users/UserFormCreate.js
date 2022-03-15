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
import userResquest from "../../requests/UserRequest";
import { useState } from "react";
import UserRequestModel from "../../models/UserRequestModel";
import Swal from "sweetalert2";
import { roles } from "aria-query";
import {
  validateConfirmPassword,
  validateEmail,
  validatePassword,
} from "../../utils/validate";

const UserFormCreate = () => {
  const [lastName, setLastName] = useState("");
  const [firstName, setFirstName] = useState("");
  const [userName, setUserName] = useState("");
  const [phoneNumber, setPhoneNumber] = useState("");
  const [birthDay, setBirthDay] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [admin, setAdmin] = useState(false);
  const [customer, setCustomer] = useState(false);

  const createUser = async () => {
    try {
      if (!birthDay) {
        await Swal.fire({
          icon: "error",
          title: "Error: Please select birthday",
          showConfirmButton: true,
        });

        return;
      }
      if (!userName) {
        await Swal.fire({
          icon: "error",
          title: "Error: Username invalid",
          showConfirmButton: true,
        });

        return;
      }

      if (!validateEmail(email)) {
        await Swal.fire({
          icon: "error",
          title: "Error: Email invalid",
          showConfirmButton: true,
        });

        return;
      }
      if (!validatePassword(password)) {
        await Swal.fire({
          icon: "error",
          title: "Error: Password invalid",
          showConfirmButton: true,
        });

        return;
      }
      if (!validateConfirmPassword(confirmPassword, password)) {
        await Swal.fire({
          icon: "error",
          title: "Error: Confirm password invalid",
          showConfirmButton: true,
        });

        return;
      }
      let token = localStorage.getItem("token");
      if (token) {
        var listRole = [];
        if (admin) listRole.push("admin");
        if (customer) listRole.push("customer");
        //let requestModel = new UserRequestModel();
        // let requestFormData = requestModel.GetCreateUserFormData(
        //   lastName,
        //   userName,
        //   phoneNumber,
        //   firstName,
        //   confirmPassword,
        //   password,
        //   email,
        //   birthDay,
        //   listRole
        // );
        var requestFormData = {
          lastName: lastName,
          userName: userName,
          phoneNumber: phoneNumber,
          firstName: firstName,
          confirmPassword: confirmPassword,
          password: password,
          email: email,
          birthDay: birthDay,
          listRole: listRole,
        };
        let response = await userResquest.create(requestFormData, token);
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
          var idUserResponse = responseData.id ? responseData.id : "";
          await Swal.fire({
            icon: "success",
            title: "Create user success",
          });
        }
      }
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error.response.data.message
          ? error.response.data.message
          : "Failed",
        showConfirmButton: true,
      });
    }
  };

  return (
    <div>
      <Card>
        <CardBody className="position-relative form-width-center">
          <CardTitle tag="h2" className="center margin-bottom-40">
            Create User
          </CardTitle>
          <Form>
            <FormGroup>
              <Label for="adduserfirstname">First Name</Label>
              <Input
                onChange={(e) => setFirstName(e.target.value)}
                id="adduserfirstname"
                name="adduserfirstname"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="adduserlasttname">Last Name</Label>
              <Input
                onChange={(e) => setLastName(e.target.value)}
                id="adduserlasttname"
                name="adduserlasttname"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="adduseremail">Email</Label>
              <Input
                onChange={(e) => setEmail(e.target.value)}
                id="adduseremail"
                name="adduseremail"
                type="email"
              />
            </FormGroup>
            <FormGroup>
              <Label for="adduserbirthday">Birth Day</Label>
              <Input
                onChange={(e) => setBirthDay(e.target.value.toString())}
                id="adduserbirthDay"
                name="adduserbirthDay"
                type="date"
              />
            </FormGroup>
            <FormGroup>
              <Label for="adduserphone">Phone Number</Label>
              <Input
                onChange={(e) => setPhoneNumber(e.target.value)}
                id="adduserphone"
                name="adduserphone"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addusername">Username</Label>
              <Input
                onChange={(e) => setUserName(e.target.value)}
                id="addusername"
                name="addusername"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="adduserpassword">Password</Label>
              <Input
                onChange={(e) => setPassword(e.target.value)}
                id="adduserpassword"
                name="adduserpassword"
                type="password"
              />
            </FormGroup>
            <FormGroup>
              <Label for="confirmPassword">Confirm Password</Label>
              <Input
                onChange={(e) => setConfirmPassword(e.target.value)}
                id="confirmPassword"
                name="confirmPassword"
                type="password"
              />
            </FormGroup>
            <FormGroup className="margin-bottom-0">
              <Label check>Roles</Label>
            </FormGroup>
            <FormGroup check inline>
              <Input
                type="checkbox"
                onChange={(e) => setAdmin(e.target.value)}
              />
              <Label check>Admin</Label>
            </FormGroup>
            <FormGroup check inline>
              <Input
                type="checkbox"
                onChange={(e) => setCustomer(e.target.value)}
              />
              <Label check>Customer</Label>
            </FormGroup>
            <br />
            <Button active block onClick={createUser}>
              Create
            </Button>
          </Form>
        </CardBody>
      </Card>
    </div>
  );
};
export default UserFormCreate;
