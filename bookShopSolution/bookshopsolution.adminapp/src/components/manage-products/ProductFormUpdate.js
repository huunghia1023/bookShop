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
import { useState, useEffect } from "react";
import Swal from "sweetalert2";
import ProductRequestModel from "../../models/ProductRequestModel";
import productResquest from "../../requests/ProductRequest";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import "./ManageProduct.css";
import { useNavigate, useParams } from "react-router-dom";
import ProductModel from "../../models/ProductModel";

const ProductFormUpdate = () => {
  const [description, setDescription] = useState("");
  const [productName, setProductName] = useState("");
  const [details, setDetails] = useState("");
  const [seoDescription, setSeoDescription] = useState("");
  const [seoTitle, setSeoTitle] = useState("");
  const [seoAlias, setSeoAlias] = useState("");
  const [price, setPrice] = useState(0);
  const [stock, setStock] = useState(0);
  const [languageId, setLanguageId] = useState("en");

  let params = useParams();
  var productIdParam = params.idProduct ? params.idProduct : "";
  let navigate = useNavigate();

  useEffect(() => {
    if (productIdParam) GetProductInfo(productIdParam);
  }, []);

  const UpdateProduct = async () => {
    try {
      if (!productIdParam) {
        await Swal.fire({
          icon: "error",
          title: "Error: Product not found",
          showConfirmButton: true,
        });
        navigate(`/manage-products`, {
          replace: true,
        });
        return;
      }
      let token = localStorage.getItem("token");
      if (token) {
        let requestModel = new ProductRequestModel();
        let requestFormData = requestModel.GetUpdateProductFormData(
          description,
          productName,
          details,
          seoDescription,
          seoTitle,
          seoAlias,
          price,
          stock,
          languageId
        );
        let response = await productResquest.update(
          productIdParam,
          requestFormData,
          token
        );
        if (response.status === 200) {
          await Swal.fire({
            icon: "success",
            title: "Update product success",
          });
        } else {
          await Swal.fire({
            icon: "error",
            title: "Error: Can not update product",
            showConfirmButton: true,
          });
        }
        navigate(`/manage-products`, {
          replace: true,
        });
        return;
      }
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error,
        showConfirmButton: true,
      });
    }
  };

  const GetProductInfo = async (id) => {
    try {
      let token = localStorage.getItem("token");
      if (!token) {
        navigate("/login", { replace: true });
        return;
      }
      var product = new ProductModel();
      let response = await productResquest.getProductInfo(
        id,
        languageId,
        token
      );
      if (response.status !== 200 || !response.data) {
        await Swal.fire({
          icon: "error",
          title: "Can not get product",
          showConfirmButton: true,
        });
        return;
      }

      var productResponse = response.data;
      if (productResponse) {
        product.id = productResponse.id;
        product.seoAlias = productResponse.seoAlias;
        product.seoDescription = productResponse.seoDescription;
        product.name = productResponse.name;
        product.seoTitle = productResponse.seoTitle;
        product.description = productResponse.description;
        product.details = productResponse.details;
        product.price = productResponse.price;
        product.stock = productResponse.stock;

        setProductName(product.name);
        setSeoAlias(product.seoAlias);
        setSeoDescription(product.seoDescription);
        setSeoTitle(product.seoTitle);
        setDescription(product.description);
        setDetails(product.details);
        setPrice(product.price);
        setStock(product.stock);
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
            Update Product
          </CardTitle>
          <Form>
            <FormGroup>
              <Label for="addproductname">Product Name</Label>
              <Input
                value={productName}
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
                value={details}
                onChange={(e) => setDetails(e.target.value)}
                id="addproductdetails"
                name="addproductdetails"
                type="textarea"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductseodescription">Seo Description</Label>
              <Input
                value={seoDescription}
                onChange={(e) => setSeoDescription(e.target.value)}
                id="addproductseodescription"
                name="addproductseodescription"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductseotitle">Seo Title</Label>
              <Input
                value={seoTitle}
                onChange={(e) => setSeoTitle(e.target.value)}
                id="addproductseotitle"
                name="addproductseotitle"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductseoalias">Seo Alias</Label>
              <Input
                value={seoAlias}
                onChange={(e) => setSeoAlias(e.target.value)}
                id="addproductseoalias"
                name="addproductseoalias"
                type="text"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductprice">Price</Label>
              <Input
                value={price}
                onChange={(e) => setPrice(e.target.value)}
                id="addproductprice"
                name="addproductprice"
                type="number"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addproductstock">Stock</Label>
              <Input
                value={stock}
                onChange={(e) => setStock(e.target.value)}
                id="addproductstock"
                name="addproductstock"
                type="number"
              />
            </FormGroup>
            <br />
            <Button active block onClick={UpdateProduct}>
              Update
            </Button>
          </Form>
        </CardBody>
      </Card>
    </div>
  );
};
export default ProductFormUpdate;
