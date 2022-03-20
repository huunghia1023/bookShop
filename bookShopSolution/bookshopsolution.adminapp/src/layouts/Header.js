import React from "react";
import { useNavigate } from "react-router-dom";
import {
  Navbar,
  Collapse,
  NavbarBrand,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
  Dropdown,
  Button,
} from "reactstrap";
import Logo from "./Logo";
import "./Layout.css";
import { ReactComponent as LogoWhite } from "../assets/images/logos/adminprowhite.svg";
import user1 from "../assets/images/users/user4.jpg";
import Swal from "sweetalert2";

const Header = () => {
  let navigate = useNavigate();
  const [isOpen, setIsOpen] = React.useState(false);

  const [dropdownOpen, setDropdownOpen] = React.useState(false);

  const toggle = () => setDropdownOpen((prevState) => !prevState);
  const Handletoggle = () => {
    setIsOpen(!isOpen);
  };
  const showMobilemenu = () => {
    document.getElementById("sidebarArea").classList.toggle("showSidebar");
  };

  const OpenMyAccountPage = async () => {
    var accountId = localStorage.getItem("accountId");
    if (accountId) {
      navigate(`/manage-users/${accountId}`, { replace: true });
      return;
    }
    await Swal.fire({
      icon: "error",
      title: "Error: Please login again",
      showConfirmButton: true,
    });
    navigate("/login", { replace: true });
  };

  const Logout = () => {
    try {
      localStorage.removeItem("user");
      localStorage.removeItem("token");
      // do somethings
      navigate("/login", { replace: true });
    } catch (error) {}
  };

  return (
    <Navbar color="white" light expand="md" className="fix-header">
      <div className="d-flex align-items-center">
        <div className="d-lg-block d-none me-5 pe-3">
          <Logo />
        </div>
        <NavbarBrand href="/">
          <LogoWhite className="d-lg-none" />
        </NavbarBrand>
        <Button
          color="primary"
          className=" d-lg-none"
          onClick={() => showMobilemenu()}
        >
          <i className="bi bi-list"></i>
        </Button>
      </div>
      <div className="hstack gap-2">
        <Button
          color="primary"
          size="sm"
          className="d-sm-block d-md-none"
          onClick={Handletoggle}
        >
          {isOpen ? (
            <i className="bi bi-x"></i>
          ) : (
            <i className="bi bi-three-dots-vertical"></i>
          )}
        </Button>
      </div>

      <Collapse className="right-position" navbar isOpen={isOpen}>
        <Dropdown isOpen={dropdownOpen} toggle={toggle}>
          <DropdownToggle color="transparent">
            <img
              src={user1}
              alt="profile"
              className="rounded-circle"
              width="50"
            ></img>
          </DropdownToggle>
          <DropdownMenu>
            <DropdownItem onClick={OpenMyAccountPage}>My Account</DropdownItem>
            <DropdownItem divider />
            <DropdownItem onClick={Logout}>Logout</DropdownItem>
          </DropdownMenu>
        </Dropdown>
      </Collapse>
    </Navbar>
  );
};

export default Header;
