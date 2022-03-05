import "bootstrap";
/*import "./assets/styles/styles.scss";*/
/*import "./assets/styles/demo.scss";*/
import {
    BrowserRouter as Router,
    Routes,
    Route,
    Link
} from "react-router-dom";
//import AdminLayout from "./components/AdminLayout";
//import Todo from "./pages/Todo";
//import data from "./data/data";
//import BootstrapComponent from "./pages/BootstrapComponent";

function App() {
    return (
        <Router>
            <div>
                <nav>
                    <ul>
                        <li>
                            <a href="/">Home</a>
                        </li>
                        <li>
                            <a href="/about">About</a>
                        </li>
                        <li>
                            <a href="/users">Users</a>
                        </li>
                    </ul>
                </nav>

                {/* A <Switch> looks through its children <Route>s and
            renders the first one that matches the current URL. */}
                <Routes>
                    <Route path="/about">
                        <About />
                    </Route>
                    <Route path="/users">
                        <Users />
                    </Route>
                    <Route path="/">
                        <Home />
                    </Route>
                </Routes>
            </div>
        </Router>
    );
}

export default App;