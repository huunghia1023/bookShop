import { Outlet } from "react-router-dom";
import Sidebar from "./Sidebar";
import Header from "./Header";
import { Container } from "reactstrap";
import { useNavigate } from 'react-router-dom';

const FullLayout = () => {
  let navigate = useNavigate(); 

    var token = window.sessionStorage.getItem("token");
    if (token == null) {

        // return <Navigate to='/login'/>
        navigate("/login", {replace: true});
    }

    function Logout() {
        
        localStorage.removeItem("token");
        navigate("/login", {replace: true});
    }
  return (
    <main>
      {/********header**********/}
      <Header />
      <div className="pageWrapper d-lg-flex">
        {/********Sidebar**********/}
        <aside className="sidebarArea shadow" id="sidebarArea">
          <Sidebar />
        </aside>
        {/********Content Area**********/}
        <div className="contentArea">
          {/********Middle Content**********/}
          <Container className="p-4" fluid>
            <Outlet />
          </Container>
        </div>
      </div>
    </main>
  );
};

export default FullLayout;
