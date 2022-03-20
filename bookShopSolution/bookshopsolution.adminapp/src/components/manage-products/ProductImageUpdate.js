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
import { useState, useEffect } from "react";
import Swal from "sweetalert2";
import ImageRequestModel from "../../models/ImageRequestModel";
import ImageRequest from "../../requests/ImageRequest";
import ImageModel from "../../models/ImageModel";

const ProductImageUpdate = () => {
  const [caption, setCaption] = useState("");
  const [isDefault, setIsDefault] = useState(false);
  const [order, setOrder] = useState(1);
  const [image, setImage] = useState("");

  let { idProduct, idImage } = useParams();

  let navigate = useNavigate();

  useEffect(() => {
    GetImageInfo(idProduct, idImage);
  }, []);

  const GetImageInfo = async (productId, imageId) => {
    try {
      let token = localStorage.getItem("token");
      if (!token) {
        navigate("/login", { replace: true });
        return;
      }
      var image = new ImageModel();
      let response = await ImageRequest.getImageInfo(productId, imageId);
      if (response.status !== 200 || !response.data) {
        await Swal.fire({
          icon: "error",
          title: "Can not get image with id " + imageId,
          showConfirmButton: true,
        });
        return;
      }

      var imageResponse = response.data;
      if (imageResponse) {
        image.ImageId = imageResponse.imageId;
        image.Caption = imageResponse.caption;
        image.DateCreated = imageResponse.dateCreated;
        image.FileSize = imageResponse.fileSize;
        image.ImagePath = imageResponse.imagePath;
        image.IsDefault = imageResponse.isDefault;
        image.SortOrder = imageResponse.sortOrder;

        setCaption(image.Caption);
        setImage(image.ImagePath);
        setIsDefault(image.IsDefault);
        setOrder(image.SortOrder);
      }
    } catch (error) {
      await Swal.fire({
        icon: "error",
        title: error,
        showConfirmButton: true,
      });
    }
  };

  const UpdateImage = async () => {
    try {
      if (!idProduct || !idImage) {
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
        let response = await ImageRequest.update(
          idProduct,
          idImage,
          requestData,
          token
        );
        if (response.status === 200) {
          await Swal.fire({
            icon: "success",
            title: "Update image success",
          });
          navigate(`/manage-products/${idProduct}/images`, {
            replace: true,
          });
          return;
        }
        await Swal.fire({
          icon: "error",
          title: "Error: Update image Failed",
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
                value={caption}
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
                checked={isDefault}
                onChange={(e) => setIsDefault(e.target.checked)}
                id="addimagedefault"
                name="addimagedefault"
                type="checkbox"
              />
            </FormGroup>
            <FormGroup>
              <Label for="addimageorder">Order</Label>
              <Input
                value={order}
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
