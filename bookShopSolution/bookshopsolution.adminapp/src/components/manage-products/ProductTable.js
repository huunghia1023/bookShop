import {
  Card,
  CardBody,
  CardTitle,
  CardSubtitle,
  Button,
  CardHeader,
  InputGroup,
  Input,
  Container,
  Row,
  Col,
} from "reactstrap";
import { Dropdown, DropdownButton } from "react-bootstrap";
import React, { useEffect, useState } from "react";
import Swal from "sweetalert2";
import "./ManageProduct.css";
import DataTable from "react-data-table-component";
import styled, { keyframes } from "styled-components";
import { useNavigate, Link } from "react-router-dom";
import productResquest from "../../requests/ProductRequest";
import ProductModel from "../../models/ProductModel";
import categoryResquest from "../../requests/CategoryRequest";
import CategorytModel from "../../models/CategoryModel";
import { Markup } from "interweave";
import { BaseAddress } from "../../utils/Constant";

const thumbStyle = { width: "100%", padding: "20px 0px" };
const columns = [
  {
    name: "Id",
    selector: (row) => row.id,
    sortable: true,
    maxWidth: "20px",
  },
  {
    name: "Thumbnail",
    selector: (row) => (
      <img style={thumbStyle} src={row.thumbnail} alt="thumbnail"></img>
    ),
    maxWidth: "100px",
  },
  {
    name: "Name",
    selector: (row) => row.name,
    sortable: true,
    maxWidth: "300px",
  },
  {
    name: "Description",
    selector: (row) => <Markup content={row.description} />,
    sortable: true,
    maxWidth: "500px",
  },
  {
    name: "Original Price",
    selector: (row) => row.originalPrice,
    sortable: true,
    maxWidth: "100px",
  },
  {
    name: "Price",
    selector: (row) => row.price,
    sortable: true,
    maxWidth: "100px",
  },
  {
    name: "Stock",
    selector: (row) => row.stock,
    sortable: true,
    maxWidth: "20px",
  },
  {
    name: "ViewCount",
    selector: (row) => row.viewCount,
    sortable: true,
    maxWidth: "20px",
  },
  {
    name: "Date Created",
    selector: (row) => row.dateCreated,
    sortable: true,
    maxWidth: "200px",
  },
  {
    cell: (row, index, column, id) => (
      <Link to={`/manage-products/${row.id}`} replace>
        <Button variant="contained" color="warning">
          Edit
        </Button>
      </Link>
    ),
    button: true,
  },
  {
    cell: (row, index, column, id) => (
      <Link to={`/manage-products/${row.id}/categories`} replace key={id}>
        <Button variant="contained" color="warning">
          Category
        </Button>
      </Link>
    ),
    button: true,
  },
  {
    cell: (row, index, column, id) => (
      <Link to={`/manage-products/${row.id}/images`} replace key={id}>
        <Button variant="contained" color="warning">
          Image
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

function ProductTable() {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [keyword, setKeyword] = useState("");
  const [loading, setLoading] = useState(true);
  const [selectedData, setSelectedData] = useState([]);
  const [toggleCleared, setToggleCleared] = useState(false);
  const [totalRows, setTotalRows] = useState(0);
  const [pageSize, setPageSize] = useState(10);
  const [languageId, setLanguageId] = useState("en");
  const [categorySelected, setCategorySelected] = useState(
    new CategorytModel(0, "All Category")
  );

  let navigate = useNavigate();
  useEffect(() => {
    GetAllCategory();
  }, []);

  useEffect(() => {
    GetListProduct(1, 10);
  }, [categorySelected]);

  const addProduct = () => {
    navigate("/manage-products/add", { replace: true });
  };

  const actions = (
    <Button onClick={addProduct} className="btn" color="primary">
      Add
    </Button>
  );

  async function GetListProduct(page, size) {
    try {
      setLoading(true);
      if (!page || !size) {
        page = 1;
        size = 10;
      }
      let token = localStorage.getItem("token");
      if (token) {
        var listProduct = [];

        let response = await productResquest.getPaging(
          page,
          size,
          keyword,
          languageId,
          categorySelected.id,
          token
        );
        if (response.status === 200) {
          var responseData = response.data ? response.data : "";

          if (!responseData) {
            await Swal.fire({
              icon: "error",
              title: "Error: Can not get list products",
              showConfirmButton: true,
            });
            return;
          }
          if (responseData) {
            var totalRows = responseData.totalRecord
              ? responseData.totalRecord
              : 0;
            setTotalRows(totalRows);
            var productResponse = responseData.items ? responseData.items : [];
            productResponse.forEach((element) => {
              var product = new ProductModel();
              product.id = element.id ? element.id : -1;
              product.dateCreated = element.dateCreated
                ? element.dateCreated
                : "";
              product.description = element.description
                ? element.description
                : "";
              product.details = element.details ? element.details : "";
              product.languageId = element.languageId ? element.languageId : "";
              product.name = element.name ? element.name : "";
              product.originalPrice = element.originalPrice
                ? element.originalPrice
                : -1;
              product.price = element.price ? element.price : -1;
              product.seoAlias = element.seoAlias ? element.seoAlias : "";
              product.seoDescription = element.seoDescription
                ? element.seoDescription
                : "";
              product.seoTitle = element.seoTitle ? element.seoTitle : "";
              product.stock = element.stock ? element.stock : 0;
              product.viewCount = element.viewCount ? element.viewCount : 0;
              product.thumbnail = element.thumbnail
                ? BaseAddress + "/user-content/" + element.thumbnail
                : "";
              listProduct.push(product);
            });

            setProducts(listProduct);
          }

          setLoading(false);
        } else {
          await Swal.fire({
            icon: "error",
            title: "Can not get list product",
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

  async function GetAllCategory() {
    try {
      setLoading(true);
      var listCategory = [];
      var defaultCategory = new CategorytModel();
      defaultCategory.id = 0;
      defaultCategory.name = "All Category";
      defaultCategory.isShowOnHome = true;
      listCategory.push(defaultCategory);
      let token = localStorage.getItem("token");
      if (token) {
        let response = await categoryResquest.getAll(token, languageId);
        if (response.status === 200) {
          var categoryResponse = response.data ? response.data : [];

          categoryResponse.forEach((element) => {
            var category = new CategorytModel();
            category.id = element.id;
            category.name = element.name;
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

  const DeleteProducts = async (ids) => {
    try {
      Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
      }).then((result) => {
        if (result.isConfirmed) {
          let token = localStorage.getItem("token");
          if (token) {
            ids.forEach(async (id) => {
              let response = await productResquest.delete(id, token);
              if (response.status === 200) {
                var productData = products.filter(function (e) {
                  return (
                    ids.filter(function (ae) {
                      return ae === e.id;
                    }).length === 0
                  );
                });

                setProducts(productData);
                await Swal.fire({
                  icon: "success",
                  title: "Delete success",
                });

                return;
              }
              await Swal.fire({
                icon: "error",
                title: "Error: Can not detele Product",
                showConfirmButton: true,
              });
            });
          }
        }
      });
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error,
        showConfirmButton: true,
      });
    }
  };

  const handlePageChange = (page) => {
    GetListProduct(page, pageSize);
  };

  const handlePerRowsChange = async (newPageSize, page) => {
    // call api
    GetListProduct(page, newPageSize);

    setPageSize(newPageSize);
    setLoading(false);
  };

  const handleRowSelected = (state) => {
    setSelectedData(state.selectedRows);
  };

  const deleteUser = async () => {
    const ids = selectedData.map((r) => r.id);
    //delete
    await DeleteProducts(ids);
    setToggleCleared(!toggleCleared);
  };

  return (
    <div>
      <Card>
        <CardHeader>
          <CardTitle tag="h5">Manage Products</CardTitle>
          <CardSubtitle className="mb-2 text-muted" tag="h6">
            List Product In System BookShop
          </CardSubtitle>
        </CardHeader>
        <CardBody>
          <Container>
            <Row className="mt-3">
              <Col xs="6">
                <InputGroup>
                  <Input
                    onChange={(e) => setKeyword(e.target.value)}
                    placeholder="search for product name"
                  />
                  <Button onClick={GetListProduct} color="primary">
                    Search
                  </Button>
                </InputGroup>
              </Col>
              <Col className="position-relative" xs="6">
                <DropdownButton
                  id="dropdown-category"
                  title={categorySelected.name}
                >
                  {categories.map((item) => {
                    return (
                      <Dropdown.Item
                        key={item.id}
                        onClick={() => setCategorySelected(item)}
                      >
                        {item.name}
                      </Dropdown.Item>
                    );
                  })}
                </DropdownButton>
              </Col>
            </Row>
          </Container>

          <DataTable
            columns={columns}
            data={products}
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

export default ProductTable;
