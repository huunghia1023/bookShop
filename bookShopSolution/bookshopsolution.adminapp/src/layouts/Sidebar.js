import { Button, Nav, NavItem } from "reactstrap";
import { Link, useLocation } from "react-router-dom";

const navigation = [
  {
    title: "Dashboard",
    href: "/home/starter",
    icon: "bi bi-speedometer2",
  },
  {
    title: "Manager User",
    href: "/home/manage-user",
    icon: "bi bi-person-video2",
  },
  {
    title: "Badges",
    href: "/home/badges",
    icon: "bi bi-patch-check",
  },
  {
    title: "Buttons",
    href: "/home/buttons",
    icon: "bi bi-hdd-stack",
  },
  {
    title: "Cards",
    href: "/home/cards",
    icon: "bi bi-card-text",
  },
  {
    title: "Grid",
    href: "/home/grid",
    icon: "bi bi-columns",
  },
  {
    title: "Table",
    href: "/home/table",
    icon: "bi bi-layout-split",
  },
  {
    title: "Forms",
    href: "/home/forms",
    icon: "bi bi-textarea-resize",
  },
  {
    title: "Breadcrumbs",
    href: "/home/breadcrumbs",
    icon: "bi bi-link",
  },
  {
    title: "About",
    href: "/home/about",
    icon: "bi bi-people",
  },
];

const Sidebar = () => {
  const showMobilemenu = () => {
    document.getElementById("sidebarArea").classList.toggle("showSidebar");
  };
  let location = useLocation();

  return (
    <div className="bg-dark">
      <div className="d-flex">
        <Button
          color="white"
          className="ms-auto text-white d-lg-none"
          onClick={() => showMobilemenu()}
        >
          <i className="bi bi-x"></i>
        </Button>
      </div>
      <div className="p-3 mt-2">
        <Nav vertical className="sidebarNav">
          {navigation.map((navi, index) => (
            <NavItem key={index} className="sidenav-bg">
              <Link
                to={navi.href}
                className={
                  location.pathname === navi.href
                    ? "active nav-link py-3"
                    : "nav-link py-3"
                }
              >
                <i className={navi.icon}></i>
                <span className="ms-3 d-inline-block">{navi.title}</span>
              </Link>
            </NavItem>
          ))}
          {/* <Button
            color="danger"
            tag="a"
            target="_blank"
            className="mt-3"
            href="https://www.wrappixel.com/templates/adminpro-react-redux-admin/?ref=33"
          >
            Upgrade To Pro
          </Button> */}
        </Nav>
      </div>
    </div>
  );
};

export default Sidebar;
