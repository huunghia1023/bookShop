import {
  Card,
  CardBody,
  CardTitle,
  CardSubtitle,
  Table,
  Button,
  CardHeader,
  InputGroup,
  Input,
  Container,
  Row,
  Col,
} from "reactstrap";
import "./ManageUser.css";
import userResquest from "../../requests/UserRequest";
import React, { useEffect, useState } from "react";
import UserModel from "../../models/UserModel";
import Swal from "sweetalert2";
import DataTable from "react-data-table-component";
import styled, { keyframes } from "styled-components";
import { useNavigate, Link } from "react-router-dom";
import UserRequestModel from "../../models/UserRequestModel";

const columns = [
  {
    name: "Id",
    selector: (row) => row.id,
    sortable: true,
  },
  {
    name: "Username",
    selector: (row) => row.username,
    sortable: true,
  },
  {
    name: "FirstName",
    selector: (row) => row.firstName,
    sortable: true,
  },
  {
    name: "LastName",
    selector: (row) => row.lastName,
    sortable: true,
  },
  {
    name: "Email",
    selector: (row) => row.email,
    sortable: true,
  },
  {
    name: "Phone",
    selector: (row) => row.phoneNumber,
    sortable: true,
  },
  {
    cell: (row, index, column, id) => (
      <Link to={`/home/manage-user/edit/${row.id}`} replace key={"id"}>
        <Button variant="contained" color="warning">
          Edit
        </Button>
      </Link>
    ),
    button: true,
  },
];

const contextActions = (deleteHandler) => (
  <Button onClick={deleteHandler} className="btn" color="danger">
    Delete
  </Button>
);

const rotate360 = keyframes`
  from {
    transform: rotate(0deg);
  }

  to {
    transform: rotate(360deg);
  }
`;

const Spinner = styled.div`
  margin: 16px;
  animation: ${rotate360} 1s linear infinite;
  transform: translateZ(0);
  border-top: 2px solid grey;
  border-right: 2px solid grey;
  border-bottom: 2px solid grey;
  border-left: 4px solid black;
  background: transparent;
  width: 30px;
  height: 30px;
  border-radius: 50%;
`;

const CustomLoader = () => (
  <div style={{ padding: "24px" }}>
    <Spinner />
    <div>Loading...</div>
  </div>
);

function UserTable() {
  const [users, setUsers] = useState([]);
  const [keyword, setKeyword] = useState("");
  const [loading, setLoading] = useState(true);
  const [selectedData, setSelectedData] = useState([]);
  const [toggleCleared, setToggleCleared] = useState(false);
  const [totalRows, setTotalRows] = useState(0);
  const [pageSize, setPageSize] = useState(10);

  let navigate = useNavigate();
  useEffect(() => {
    GetListUser(1, 10);
  }, []);

  const addUser = () => {
    navigate("/home/manage-user/add", { replace: true });
  };

  const actions = (
    <Button onClick={addUser} className="btn" color="primary">
      Add
    </Button>
  );

  async function GetListUser(page, size) {
    try {
      setLoading(true);
      let token = sessionStorage.getItem("token");
      if (token) {
        var listUser = [];

        let response = await userResquest.getAll(page, size, keyword, token);
        if (response.status === 200) {
          var responseData = response.data ? response.data : "";

          if (!responseData) {
            await Swal.fire({
              icon: "error",
              title: "Error: Can not get authenticate",
              showConfirmButton: true,
            });
            return;
          }
          var results = responseData.results ? responseData.results : "";
          if (results) {
            var totalRows = results.totalRecord ? results.totalRecord : 0;
            setTotalRows(totalRows);
            var userResponse = results.items ? results.items : [];
            userResponse.forEach((element) => {
              var user = new UserModel();
              user.id = element.id ? element.id : "";
              user.email = element.email ? element.email : "";
              user.firstName = element.firstName ? element.firstName : "";
              user.lastName = element.lastName ? element.lastName : "";
              user.username = element.userName ? element.userName : "";
              user.phoneNumber = element.phoneNumber ? element.phoneNumber : "";
              listUser.push(user);
            });

            setUsers(listUser);
          }

          setLoading(false);
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
  }

  const DeleteUser = async (ids) => {
    try {
      let token = sessionStorage.getItem("token");
      if (token) {
        let requestModel = new UserRequestModel();
        let requestFormData = requestModel.GetDeleteUserFormData(ids);
        let response = await userResquest.deleteMultiple(
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
          var results = responseData.results ? responseData.results : "";
          var userData = users.filter(function (e) {
            return (
              results.filter(function (ae) {
                return ae == e.id;
              }).length == 0
            );
          });

          setUsers(userData);
          await Swal.fire({
            icon: "success",
            title: "Detete user success",
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

  const handlePageChange = (page) => {
    GetListUser(page, pageSize);
  };

  const handlePerRowsChange = async (newPageSize, page) => {
    // call api
    GetListUser(page, newPageSize);
    //const response = await axios.get(`https://reqres.in/api/users?page=${page}&per_page=${newPerPage}&delay=1`);

    //setData(response.data.data);
    setPageSize(newPageSize);
    setLoading(false);
  };

  const handleRowSelected = (state) => {
    setSelectedData(state.selectedRows);
  };

  const deleteUser = async () => {
    const ids = selectedData.map((r) => r.id);
    //delete
    await DeleteUser(ids);
    setToggleCleared(!toggleCleared);
  };

  return (
    <div>
      <Card>
        <CardHeader>
          <CardTitle tag="h5">Manage User</CardTitle>
          <CardSubtitle className="mb-2 text-muted" tag="h6">
            List user in system BookShop
          </CardSubtitle>
        </CardHeader>
        <CardBody>
          <Container>
            <Row className="mt-3">
              <Col xs="6">
                <InputGroup>
                  <Input
                    onChange={(e) => setKeyword(e.target.value)}
                    placeholder="search username or phonenumer"
                  />
                  <Button onClick={GetListUser} color="success">
                    Search
                  </Button>
                </InputGroup>
              </Col>
              <Col className="position-relative" xs="6">
                <Button className="btn position-absolute-right" color="primary">
                  Add
                </Button>
              </Col>
            </Row>
          </Container>

          <DataTable
            columns={columns}
            data={users}
            selectableRows
            pagination
            paginationServer
            paginationTotalRows={totalRows}
            onChangeRowsPerPage={handlePerRowsChange}
            onChangePage={handlePageChange}
            progressPending={loading}
            progressComponent={<CustomLoader />}
            highlightOnHover
            pointerOnHover
            actions={actions}
            contextActions={contextActions(deleteUser)}
            onSelectedRowsChange={handleRowSelected}
            clearSelectedRows={toggleCleared}
          />
        </CardBody>
      </Card>
    </div>
  );
}

export default UserTable;
