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
import { useState } from "react";
import Swal from "sweetalert2";
import ProductRequestModel from "../../models/ProductRequestModel";
import productResquest from "../../requests/ProductRequest";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import "./ManageProduct.css";

const ProductFormCreate = () => {
  const [description, setDescription] = useState("");
  const [productName, setProductName] = useState("");
  const [stock, setStock] = useState(0);
  const [originalPrice, setOriginalPrice] = useState(0);
  const [price, setPrice] = useState(0);
  const [details, setDetails] = useState("");
  const [seoDescription, setSeoDescription] = useState("");
  const [seoTitle, setSeoTitle] = useState("");
  const [seoAlias, setSeoAlias] = useState("");
  const [languageId, setLanguageId] = useState("en");
  const [thumbnail, setThumbnail] = useState("");
  const [isFeatured, setIsFeatured] = useState(true);

  const CreateProduct = async () => {
    try {
      let token = localStorage.getItem("token");
      if (token) {
        let requestModel = new ProductRequestModel();
        let requestFormData = requestModel.GetCreateProductFormData(
          description,
          productName,
          stock,
          originalPrice,
          price,
          details,
          seoDescription,
          seoTitle,
          seoAlias,
          languageId,
          thumbnail,
          isFeatured
        );
        let response = await productResquest.create(requestFormData, token);
        if (response.status === 201) {
          var responseData = response.data ? response.data : "";
          if (!responseData) {
            await Swal.fire({
              icon: "error",
              title: "Error: Can not create user",
              showConfirmButton: true,
            });

            return;
          }
          var idProductResponse = responseData.id ? responseData.id : "";
          await Swal.fire({
            icon: "success",
            title: "Create product success",
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
    <div>
      <Card>
        <CardBody className="position-relative form-width-center">
          <CardTitle tag="h2" className="center margin-bottom-40">
            Create Product
          </CardTitle>
          <Form>
            <FormGroup>
              <Label for="addproductname">Product Name</Label>
              <Input
                onChange={(e) => setProductName(e.target.value)}
                id="addproductname"
                name="addproductname"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductdescription">Description</Label>
              <CKEditor
                editor={ClassicEditor}
                data={description}
                onReady={(editor) => {
                  // You can store the "editor" and use when it is needed.
                  console.log("Editor is ready to use!", editor);
                }}
                onChange={(event, editor) => {
                  const data = editor.getData();
                  setDescription(data);
                }}
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductdetails">Details</Label>
              <Input
                onChange={(e) => setDetails(e.target.value)}
                id="addproductdetails"
                name="addproductdetails"
                type="textarea"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductprice">Price</Label>
              <Input
                onChange={(e) => setPrice(e.target.value)}
                id="addproductprice"
                name="addproductprice"
                type="number"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductoriginprice">Original Price</Label>
              <Input
                onChange={(e) => setOriginalPrice(e.target.value)}
                id="addproductoriginprice"
                name="addproductoriginprice"
                type="number"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductstock">Stock</Label>
              <Input
                onChange={(e) => setStock(e.target.value)}
                id="addproductstock"
                name="addproductstock"
                type="number"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductseodescription">Seo Description</Label>
              <Input
                onChange={(e) => setSeoDescription(e.target.value)}
                id="addproductseodescription"
                name="addproductseodescription"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductseotitle">Seo Title</Label>
              <Input
                onChange={(e) => setSeoTitle(e.target.value)}
                id="addproductseotitle"
                name="addproductseotitle"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductseoalias">Seo Alias</Label>
              <Input
                onChange={(e) => setSeoAlias(e.target.value)}
                id="addproductseoalias"
                name="addproductseoalias"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductthumbnail">Thumbnail</Label>
              <Input
                onChange={(e) => setThumbnail(e.target.files[0])}
                id="addproductthumbnail"
                name="addproductthumbnail"
                type="file"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductfeature">Featured</Label>
              <Input
                checked={isFeatured}
                onChange={(e) => setIsFeatured(e.target.checked)}
                id="addproductfeature"
                name="addproductfeature"
                type="checkbox"
              />
            </FormGroup>
            <br />
            <Button active block onClick={CreateProduct}>
              Create
            </Button>
          </Form>
        </CardBody>
      </Card>
    </div>
  );
};
export default ProductFormCreate;
