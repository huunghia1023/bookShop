import {
  Card,
  CardBody,
  CardTitle,
  CardSubtitle,
  Button,
  CardHeader,
} from "reactstrap";
import React, { useEffect, useState } from "react";
import Swal from "sweetalert2";
import "./ManageCategory.css";
import DataTable from "react-data-table-component";
import styled, { keyframes } from "styled-components";
import { useNavigate, Link } from "react-router-dom";
import categoryResquest from "../../requests/CategoryRequest";
import CategorytModel from "../../models/CategoryModel";
import CategoryRequestModel from "../../models/CategoryRequestModel";

const columns = [
  {
    name: "Id",
    selector: (row) => row.id,
    sortable: true,
  },
  {
    name: "Name",
    selector: (row) => row.name,
    sortable: true,
  },
  {
    name: "Seo Description",
    selector: (row) => row.seoDescription,
    sortable: true,
  },
  {
    name: "Seo Title",
    selector: (row) => row.seoTitle,
    sortable: true,
  },
  {
    name: "Seo Alias",
    selector: (row) => row.seoAlias,
    sortable: true,
  },
  {
    name: "Show On Home",
    selector: (row) => row.isShowOnHome,
    sortable: true,
  },
  {
    cell: (row, index, column, id) => (
      <Link to={`/manage-categories/${row.id}`} replace>
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

function CategoryTable() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [selectedData, setSelectedData] = useState([]);
  const [toggleCleared, setToggleCleared] = useState(false);
  const [totalRows, setTotalRows] = useState(0);
  const [languageId, setLanguageId] = useState("en");

  let navigate = useNavigate();
  useEffect(() => {
    GetAllCategory();
  }, []);

  const addCategory = () => {
    navigate("/manage-categories/add", { replace: true });
  };

  const actions = (
    <Button onClick={addCategory} className="btn" color="primary">
      Add
    </Button>
  );

  const DeleteCategory = async (ids) => {
    try {
      let token = localStorage.getItem("token");
      if (token) {
        let requestModel = new CategoryRequestModel();
        let requestFormData = requestModel.GetDeleteCategoryFormData(ids);
        let response = await categoryResquest.deleteMultiple(
          requestFormData,
          token
        );
        if (response.status === 200) {
          var categoryData = categories.filter(function (e) {
            return (
              ids.filter(function (ae) {
                return ae === e.id;
              }).length === 0
            );
          });

          setCategories(categoryData);
          await Swal.fire({
            icon: "success",
            title: "Delete success",
          });
          return;
        }
        await Swal.fire({
          icon: "error",
          title: "Error: Can not detele category",
          showConfirmButton: true,
        });
      }
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error,
        showConfirmButton: true,
      });
    }
  };

  async function GetAllCategory() {
    try {
      setLoading(true);
      var listCategory = [];
      let token = localStorage.getItem("token");
      if (token) {
        let response = await categoryResquest.getAll(token, languageId);
        if (response.status === 200) {
          var categoryResponse = response.data ? response.data : [];

          categoryResponse.forEach((element) => {
            var category = new CategorytModel();
            category.id = element.id;
            category.name = element.name;
            category.isShowOnHome = element.isShowOnHome.toString();
            category.seoAlias = element.seoAlias;
            category.seoDescription = element.seoDescription;
            category.seoTitle = element.seoTitle;
            listCategory.push(category);
          });

          setCategories(listCategory);
          setLoading(false);
        } else {
          await Swal.fire({
            icon: "error",
            title: "Can not get category",
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

  const handleRowSelected = (state) => {
    setSelectedData(state.selectedRows);
  };

  const deleteCategory = async () => {
    const ids = selectedData.map((r) => r.id);
    //delete
    await DeleteCategory(ids);
    setToggleCleared(!toggleCleared);
  };

  return (
    <div>
      <Card>
        <CardHeader>
          <CardTitle tag="h5">Manage Categories</CardTitle>
          <CardSubtitle className="mb-2 text-muted" tag="h6">
            List Categories In System BookShop
          </CardSubtitle>
        </CardHeader>
        <CardBody>
          <DataTable
            columns={columns}
            data={categories}
            selectableRows
            pagination
            paginationTotalRows={totalRows}
            progressPending={loading}
            progressComponent={<CustomLoader />}
            highlightOnHover
            pointerOnHover
            actions={actions}
            contextActions={contextActions(deleteCategory)}
            onSelectedRowsChange={handleRowSelected}
            clearSelectedRows={toggleCleared}
          />
        </CardBody>
      </Card>
    </div>
  );
}

export default CategoryTable;
