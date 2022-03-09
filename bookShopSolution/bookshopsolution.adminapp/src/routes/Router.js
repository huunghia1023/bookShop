import { lazy } from "react";
import { Navigate } from "react-router-dom";

/****Layouts*****/
const FullLayout = lazy(() => import("../layouts/FullLayout.js"));

/***** Pages ****/
const Login = lazy(() => import("../views/login/Login.js"));
const Starter = lazy(() => import("../views/Starter.js"));
const Account = lazy(() => import("../components/account/Account.js"));
const ManageUser = lazy(() => import("../views/ManageUser"));
const UserFormUpdate = lazy(() =>
  import("../components/manage-user/UserFormUpdate.js")
);
const UserFormCreate = lazy(() =>
  import("../components/manage-user/UserFormCreate.js")
);
const About = lazy(() => import("../views/About.js"));
const Alerts = lazy(() => import("../views/ui/Alerts"));
const Badges = lazy(() => import("../views/ui/Badges"));
const Buttons = lazy(() => import("../views/ui/Buttons"));
const Cards = lazy(() => import("../views/ui/Cards"));
const Grid = lazy(() => import("../views/ui/Grid"));
const Tables = lazy(() => import("../views/ui/Tables"));
const Forms = lazy(() => import("../views/ui/Forms"));
const Breadcrumbs = lazy(() => import("../views/ui/Breadcrumbs"));
const ErrorPage = lazy(() => import("../views/ErrorPage"));

/*****Routes******/

const ThemeRoutes = [
  {
    path: "/",
    element: <Login />,
  },
  {
    path: "/login",
    element: <Login />,
    children: [{ path: "/login/", element: <Navigate to="/" /> }],
  },
  {
    path: "/home",
    element: <FullLayout />,
    children: [
      { path: "/home/", element: <Navigate to="/home/starter" /> },
      { path: "/home/starter", element: <Starter /> },
      { path: "/home/account", element: <Account /> },
      {
        path: "/home/manage-user",
        element: <ManageUser />,
      },
      { path: "/home/manage-user/add", element: <UserFormCreate /> },
      { path: `/home/manage-user/edit/:id`, element: <UserFormUpdate /> },
      { path: "/home/about", element: <About /> },
      { path: "/home/alerts", element: <Alerts /> },
      { path: "/home/badges", element: <Badges /> },
      { path: "/home/buttons", element: <Buttons /> },
      { path: "/home/cards", element: <Cards /> },
      { path: "/home/grid", element: <Grid /> },
      { path: "/home/table", element: <Tables /> },
      { path: "/home/forms", element: <Forms /> },
      { path: "/home/breadcrumbs", element: <Breadcrumbs /> },
    ],
  },
  {
    path: "/404",
    element: <ErrorPage />,
  },
  {
    path: "*",
    element: <Navigate to="/404" replace />,
  },
];

export default ThemeRoutes;
