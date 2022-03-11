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
import { useNavigate, useParams } from "react-router-dom";
import { useState } from "react";
import Swal from "sweetalert2";
import ImageRequestModel from "../../models/ImageRequestModel";
import ImageResquest from "../../requests/ImageRequest";

const ProductImageUpdate = () => {
  const [caption, setCaption] = useState("");
  const [isDefault, setIsDefault] = useState(false);
  const [order, setOrder] = useState(1);
  const [image, setImage] = useState("");

  let { productIdParam, imageIdParam } = useParams();

  let navigate = useNavigate();

  const UpdateImage = async () => {
    try {
      if (!productIdParam || !imageIdParam) {
        await Swal.fire({
          icon: "error",
          title: "Error: Product or image not found",
          showConfirmButton: true,
        });

        return;
      }
      let token = localStorage.getItem("token");
      if (token) {
        let requestModel = new ImageRequestModel();
        let requestData = requestModel.GetCreateImageFormData(
          caption,
          isDefault,
          order,
          image
        );
        let response = await ImageResquest.update(
          productIdParam,
          imageIdParam,
          requestData,
          token
        );
        if (response.status === 201) {
          await Swal.fire({
            icon: "success",
            title: "Update image success",
          });
          navigate(`/manage-products/${productIdParam}/images`, {
            replace: true,
          });
          return;
        }
        await Swal.fire({
          icon: "error",
          title: "Error: Upload image Failed",
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

  return (
    <div>
      <Card>
        <CardBody className="position-relative form-width-center">
          <CardTitle tag="h2" className="center margin-bottom-40">
            Update Image for Product
          </CardTitle>
          <Form>
            <FormGroup>
              <Label for="addimagecaption">Caption</Label>
              <Input
                onChange={(e) => setCaption(e.target.value)}
                id="addimagecaption"
                name="addimagecaption"
                type="text"
              />
            </FormGroup>

            <FormGroup check>
              <Label for="addimagedefault" check>
                Thumbnail
              </Label>
              <Input
                onChange={(e) => setIsDefault(e.target.checked)}
                id="addimagedefault"
                name="addimagedefault"
                type="checkbox"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addimageorder">Order</Label>
              <Input
                onChange={(e) => setOrder(e.target.value)}
                id="addimageorder"
                name="addimageorder"
                type="number"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addimagefile">Select Image</Label>
              <Input
                onChange={(e) => setImage(e.target.files[0])}
                id="addimagefile"
                name="addimagefile"
                type="file"
              />
            </FormGroup>
            <br />
            <Button active block onClick={UpdateImage}>
              Update
            </Button>
          </Form>
        </CardBody>
      </Card>
    </div>
  );
};
export default ProductImageUpdate;
