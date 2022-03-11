import { lazy } from "react";
import { Navigate } from "react-router-dom";

/****Layouts*****/
const FullLayout = lazy(() => import("../layouts/FullLayout.js"));

/***** Pages ****/
const Login = lazy(() => import("../views/login/Login.js"));
const Starter = lazy(() => import("../views/Starter.js"));
const Account = lazy(() => import("../components/account/Account.js"));
const ManageUsers = lazy(() => import("../views/ManageUsers"));
const ManageProducts = lazy(() => import("../views/ManageProducts"));
const ManageCategories = lazy(() => import("../views/ManageCategories"));
const UserFormUpdate = lazy(() =>
  import("../components/manage-users/UserFormUpdate.js")
);
const UserFormCreate = lazy(() =>
  import("../components/manage-users/UserFormCreate.js")
);
const ProductFormCreate = lazy(() =>
  import("../components/manage-products/ProductFormCreate.js")
);
const ProductFormUpdate = lazy(() =>
  import("../components/manage-products/ProductFormUpdate.js")
);
const ProductAssignCategory = lazy(() =>
  import("../components/manage-products/ProductAssignCategory.js")
);
const ProductImageCreate = lazy(() =>
  import("../components/manage-products/ProductImageCreate.js")
);
const ProductImageUpdate = lazy(() =>
  import("../components/manage-products/ProductImageUpdate.js")
);
const ProductImageTable = lazy(() =>
  import("../components/manage-products/ProductImageTable.js")
);
const CategoryFormUpdate = lazy(() =>
  import("../components/manage-categories/CategoryFormUpdate.js")
);
const CategoryFormCreate = lazy(() =>
  import("../components/manage-categories/CategoryFormCreate.js")
);

const ErrorPage = lazy(() => import("../views/ErrorPage"));

/*****Routes******/

const ThemeRoutes = [
  {
    path: "/",
    element: <Login />,
    children: [{ path: "login", element: <Login /> }],
  },
  {
    path: "/home",
    element: <FullLayout />,
    children: [{ path: "", element: <Starter /> }],
  },
  {
    path: "/manage-categories",
    element: <FullLayout />,
    children: [
      {
        path: "",
        element: <ManageCategories />,
      },
      { path: "add", element: <CategoryFormCreate /> },
      {
        path: ":id",
        element: <CategoryFormUpdate />,
      },
    ],
  },

  {
    path: "/account",
    element: <FullLayout />,
    children: [
      {
        path: "",
        element: <Account />,
      },
    ],
  },
  {
    path: "/manage-users",
    element: <FullLayout />,
    children: [
      {
        path: "",
        element: <ManageUsers />,
      },
      { path: "add", element: <UserFormCreate /> },
      { path: ":id", element: <UserFormUpdate /> },
    ],
  },
  {
    path: "/manage-products",
    element: <FullLayout />,
    children: [
      {
        path: "",
        element: <ManageProducts />,
      },
      { path: "add", element: <ProductFormCreate /> },
      {
        path: ":idProduct",
        element: <Navigate to="/manage-products/:idProduct" />,
      },
    ],
  },
  {
    path: "/manage-products/:idProduct",
    element: <FullLayout />,
    children: [
      {
        path: "",
        element: <ProductFormUpdate />,
      },
      {
        path: "categories",
        element: <ProductAssignCategory />,
      },
      {
        path: "images",
        element: <Navigate to="/manage-products/:idProduct/images" />,
      },
    ],
  },
  {
    path: "/manage-products/:idProduct/images",
    element: <FullLayout />,
    children: [
      {
        path: "",
        element: <ProductImageTable />,
      },
      {
        path: ":idImage",
        element: <ProductImageUpdate />,
      },
      {
        path: "add",
        element: <ProductImageCreate />,
      },
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
