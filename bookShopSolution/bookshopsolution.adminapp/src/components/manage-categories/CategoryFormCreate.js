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
import React, { useState } from "react";
import "./ManageCategory.css";
import CategoryRequestModel from "../../models/CategoryRequestModel";
import categoryResquest from "../../requests/CategoryRequest";
import Swal from "sweetalert2";

const CategoryFormCreate = (props) => {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [seoDescription, setSeoDescription] = useState("");
  const [seoTitle, setSeoTitle] = useState("");
  const [seoAlias, setSeoAlias] = useState("");
  const [isShowOnHome, setIsShowOnHome] = useState(true);
  const [languageId, setLanguageId] = useState("en");

  const updateCategory = async () => {
    try {
      let token = localStorage.getItem("token");
      if (token) {
        let requestModel = new CategoryRequestModel();
        let requestFormData = requestModel.GetCreateCategoryBody(
          name,
          seoDescription,
          seoTitle,
          seoAlias,
          isShowOnHome,
          languageId
        );
        let response = await categoryResquest.create(requestFormData, token);
        if (response.status === 201) {
          var responseData = response.data ? response.data : "";
          if (!responseData) {
            await Swal.fire({
              icon: "error",
              title: "Error: Can not create category",
              showConfirmButton: true,
            });

            return;
          }
          await Swal.fire({
            icon: "success",
            title: "Create category success",
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
              <Label for="updatecategorydescription">Descriptions</Label>
              <Input
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                id="updatecategorydescription"
                name="updatecategorydescription"
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
            <Button onClick={updateCategory} active block>
              Create
            </Button>
          </Form>
        </CardBody>
      </Card>
    </React.Fragment>
  );
};
export default CategoryFormCreate;
