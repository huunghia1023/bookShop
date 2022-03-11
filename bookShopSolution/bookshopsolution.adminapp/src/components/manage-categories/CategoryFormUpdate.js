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
import React, { useState, useEffect } from "react";
import "./ManageCategory.css";
import CategoryRequestModel from "../../models/CategoryRequestModel";
import categoryResquest from "../../requests/CategoryRequest";
import Swal from "sweetalert2";
import { useNavigate, useParams } from "react-router-dom";
import CategorytModel from "../../models/CategoryModel";

const CategoryFormUpdate = (props) => {
  const [name, setName] = useState("");
  const [seoDescription, setSeoDescription] = useState("");
  const [seoTitle, setSeoTitle] = useState("");
  const [seoAlias, setSeoAlias] = useState("");
  const [isShowOnHome, setIsShowOnHome] = useState(true);
  const [languageId, setLanguageId] = useState("en");

  let params = useParams();
  var categoryIdParam = params.id ? params.id : "";
  let navigate = useNavigate();

  useEffect(() => {
    if (categoryIdParam) GetCategoryInfo(categoryIdParam);
  }, []);

  const UpdateCategory = async () => {
    try {
      if (!categoryIdParam) {
        await Swal.fire({
          icon: "error",
          title: "Error: Category not found",
          showConfirmButton: true,
        });

        return;
      }
      let token = localStorage.getItem("token");
      if (token) {
        let requestModel = new CategoryRequestModel();
        let requestFormData = requestModel.GetUpdateCategoryFormData(
          name,
          seoDescription,
          seoTitle,
          seoAlias,
          isShowOnHome,
          languageId
        );
        let response = await categoryResquest.update(
          categoryIdParam,
          requestFormData,
          token
        );
        if (response.status === 200) {
          var responseData = response.data ? response.data : "";
          if (!responseData) {
            await Swal.fire({
              icon: "error",
              title: "Error: Can not update category",
              showConfirmButton: true,
            });

            return;
          }
          if (responseData && responseData.isSuccessed) {
            await Swal.fire({
              icon: "success",
              title: "Create category success",
            });
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
  };

  const GetCategoryInfo = async (id) => {
    try {
      let token = localStorage.getItem("token");
      if (!token) {
        navigate("/login", { replace: true });
        return;
      }
      var category = new CategorytModel();
      let response = await categoryResquest.getCategoryInfo(
        id,
        languageId,
        token
      );
      if (response.status !== 200 || !response.data) {
        await Swal.fire({
          icon: "error",
          title: "Can not get category",
          showConfirmButton: true,
        });
        return;
      }

      var categoryResponse = response.data;
      if (categoryResponse) {
        category.id = categoryResponse.id;
        category.name = categoryResponse.name;
        category.seoAlias = categoryResponse.seoAlias;
        category.seoDescription = categoryResponse.seoDescription;
        category.seoTitle = categoryResponse.seoTitle;
        category.isShowOnHome = categoryResponse.isShowOnHome;

        setIsShowOnHome(category.isShowOnHome);
        setName(category.name);
        setSeoAlias(category.seoAlias);
        setSeoDescription(category.seoDescription);
        setSeoTitle(category.seoTitle);
      }
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error,
        showConfirmButton: true,
      });
    }
  };

  return (
    <React.Fragment>
      <Card>
        <CardBody className="position-relative form-width-center">
          <CardTitle tag="h2" className="center margin-bottom-40">
            Category
          </CardTitle>
          <Form>
            <FormGroup>
              <Label for="updatecategoryname">Name</Label>
              <Input
                value={name}
                onChange={(e) => setName(e.target.value)}
                id="updatecategoryname"
                name="updatecategoryname"
                type="text"
              />
            </FormGroup>

            <FormGroup>
              <Label for="updatecategoryseodescription">Seo Descriptions</Label>
              <Input
                value={seoDescription}
                onChange={(e) => setSeoDescription(e.target.value)}
                id="updatecategoryseodescription"
                name="updatecategoryseodescription"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="updatecategoryseotitle">Seo Title</Label>
              <Input
                value={seoTitle}
                onChange={(e) => setSeoTitle(e.target.value)}
                id="updatecategoryseotitle"
                name="updatecategoryseotitle"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="updatecategoryseoalias">Seo Alias</Label>
              <Input
                value={seoAlias}
                onChange={(e) => setSeoAlias(e.target.value)}
                id="updatecategoryseoalias"
                name="updatecategoryseoalias"
                type="text"
              />
            </FormGroup>
            <FormGroup check>
              <Label check>Show On Home</Label>
              <Input
                value={isShowOnHome}
                onChange={(e) => setIsShowOnHome(e.target.value)}
                type="checkbox"
              />
            </FormGroup>
            <br />
            <Button onClick={UpdateCategory} active block>
              Update
            </Button>
          </Form>
        </CardBody>
      </Card>
    </React.Fragment>
  );
};
export default CategoryFormUpdate;
