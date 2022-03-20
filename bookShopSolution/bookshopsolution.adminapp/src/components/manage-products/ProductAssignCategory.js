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
import "./ManageProduct.css";
import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import categoryResquest from "../../requests/CategoryRequest";
import CategorytModel from "../../models/CategoryModel";
import Swal from "sweetalert2";
import ProductAssignCategoryModel from "../../models/ProductAssignCategoryModel";
import ProductRequestModel from "../../models/ProductRequestModel";
import productResquest from "../../requests/ProductRequest";
import ProductModel from "../../models/ProductModel";

const ProductAssignCategory = () => {
  const [languageId, setLanguageId] = useState("en");
  const [categories, setCategories] = useState([]);
  const [productAssigns, setProductAssigns] = useState([]);

  let params = useParams();
  var productIdParam = params.idProduct ? params.idProduct : "";
  let navigate = useNavigate();

  useEffect(() => {
    GetAllCategory();
  }, []);
  const handleCategoryChange = (e, category) => {
    if (productAssigns.length === 0) {
      var categoryAssign = new ProductAssignCategoryModel();
      categoryAssign.id = category.id;
      categoryAssign.name = category.name;
      categoryAssign.selected = e.target.checked;
      productAssigns.push(categoryAssign);
      setProductAssigns(productAssigns);
      return;
    }
    for (var i = 0; i < productAssigns.length; i++) {
      if (productAssigns[i].id === category.id) {
        productAssigns[i].selected = e.target.checked;
        setProductAssigns(productAssigns);
        return;
      }
      if (
        i === productAssigns.length - 1 &&
        productAssigns[i].id !== category.id
      ) {
        var categoryAssignn = new ProductAssignCategoryModel();
        categoryAssignn.id = category.id;
        categoryAssignn.name = category.name;
        categoryAssignn.selected = e.target.checked;
        productAssigns.push(categoryAssignn);
        setProductAssigns(productAssigns);
        return;
      }
    }
  };

  async function updateProductCategory() {
    try {
      if (!productIdParam) {
        await Swal.fire({
          icon: "error",
          title: "Error: Product not found",
          showConfirmButton: true,
        });

        return;
      }
      let token = localStorage.getItem("token");
      if (token) {
        let requestModel = new ProductRequestModel();
        let requestData = requestModel.GetAssignProductBody(productAssigns);
        let response = await productResquest.assignCategory(
          productIdParam,
          requestData,
          token
        );
        if (response.status === 200) {
          var responseData = response.data ? response.data : "";
          if (responseData.isSuccessed) {
            await Swal.fire({
              icon: "success",
              title: "Update category success",
            });
            navigate("/manage-products", { replace: true });
            return;
          }

          await Swal.fire({
            icon: "error",
            title: "Error: Update Failed",
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
      var listCategory = [];
      var listCateChecked = [];
      let token = localStorage.getItem("token");
      if (token) {
        let response = await categoryResquest.getAll(token, languageId);
        if (response.status === 200) {
          var categoryResponse = response.data ? response.data : [];
          // var categoryChecked = await GetProductCategory(productIdParam);
          categoryResponse.forEach((element) => {
            var category = new CategorytModel();
            category.id = element.id;
            category.name = element.name;
            // category.selected = false;
            // for (let i = 0; i < categoryChecked.length; i++) {
            //   const c = categoryChecked[i];
            //   if (element.name === c) {
            //     category.selected = true;
            //     listCateChecked.push(category);
            //     break;
            //   }
            // }

            listCategory.push(category);
          });
          // setProductAssigns(listCateChecked);
          setCategories(listCategory);
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

  const GetProductCategory = async (id) => {
    try {
      let token = localStorage.getItem("token");
      if (!token) {
        navigate("/login", { replace: true });
        return;
      }

      let response = await productResquest.getProductInfo(
        id,
        languageId,
        token
      );
      if (response.status !== 200 || !response.data) {
        await Swal.fire({
          icon: "error",
          title: "Can not get product category",
          showConfirmButton: true,
        });
        return;
      }

      var productResponse = response.data;
      if (productResponse) {
        return productResponse.categories;
      }
      return [];
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error,
        showConfirmButton: true,
      });
    }
  };

  return (
    <div>
      <Card>
        <CardBody className="position-relative form-width-center">
          <CardTitle tag="h2" className="center margin-bottom-40">
            Assign Category For Product
          </CardTitle>
          <Form>
            {categories.map((x) => {
              return (
                <React.Fragment>
                  <FormGroup check key={x.id}>
                    <Input
                      type="checkbox"
                      onChange={(e) => {
                        handleCategoryChange(e, x);
                      }}
                    />
                    <Label check>{x.name}</Label>
                  </FormGroup>
                </React.Fragment>
              );
            })}

            <br />
            <Button active block onClick={updateProductCategory}>
              Update
            </Button>
          </Form>
        </CardBody>
      </Card>
    </div>
  );
};
export default ProductAssignCategory;
