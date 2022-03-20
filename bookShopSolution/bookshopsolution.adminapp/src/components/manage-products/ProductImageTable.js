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
import "./ManageProduct.css";
import DataTable from "react-data-table-component";
import styled, { keyframes } from "styled-components";
import { useNavigate, Link, useParams } from "react-router-dom";
import ImageResquest from "../../requests/ImageRequest";
import ImageModel from "../../models/ImageModel";
import { BaseAddress } from "../../utils/Constant";

const thumbStyle = { width: "100%", padding: "20px 0px" };
const columns = [
  {
    name: "Id",
    selector: (row) => row.ImageId,
    sortable: true,
  },
  {
    name: "Image",
    selector: (row) => (
      <img
        style={thumbStyle}
        src={BaseAddress + "/user-content/" + row.ImagePath}
        alt="pic"
      ></img>
    ),
    maxWidth: "100px",
  },
  {
    name: "Path",
    selector: (row) => row.ImagePath,
    sortable: true,
  },
  {
    name: "Caption",
    selector: (row) => row.Caption,
    sortable: true,
  },
  {
    name: "IsDefault",
    selector: (row) => row.IsDefault,
    sortable: true,
  },
  {
    name: "FileSize",
    selector: (row) => row.FileSize,
    sortable: true,
  },
  {
    cell: (row, index, column, id) => (
      <Link to={`${row.ImageId}`} replace>
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

function ProductImageTable() {
  const [images, setImages] = useState([]);
  const [loading, setLoading] = useState(true);
  const [toggleCleared, setToggleCleared] = useState(false);
  const [selectedData, setSelectedData] = useState([]);
  let params = useParams();
  var productIdParam = params.idProduct ? params.idProduct : "";
  let navigate = useNavigate();

  useEffect(() => {
    GetListImages();
  }, []);

  const AddImage = () => {
    navigate(`/manage-products/${productIdParam}/images/add`, {
      replace: true,
    });
  };

  const DeleteImages = async (ids) => {
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
              let response = await ImageResquest.delete(
                productIdParam,
                id,
                token
              );
              if (response.status === 200) {
                var imageData = images.filter(function (e) {
                  return id !== e.ImageId;
                });

                setImages(imageData);
                await Swal.fire({
                  icon: "success",
                  title: "Delete success",
                });
                navigate(`/manage-products/${productIdParam}/images`, {
                  replace: true,
                });
                return;
              }
              await Swal.fire({
                icon: "error",
                title: "Error: Can not detele Image",
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

  const actions = (
    <Button onClick={AddImage} className="btn" color="primary">
      Add
    </Button>
  );

  async function GetListImages() {
    try {
      if (!productIdParam) {
        await Swal.fire({
          icon: "error",
          title: "Error: Product not found",
          showConfirmButton: true,
        });

        return;
      }
      setLoading(true);

      let token = localStorage.getItem("token");
      if (token) {
        var listImages = [];

        let response = await ImageResquest.getAll(productIdParam, token);
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
            var imageResponse = responseData.items ? responseData.items : [];
            imageResponse.forEach((element) => {
              var image = new ImageModel();
              image.ImageId = element.imageId ? element.imageId : -1;
              image.Caption = element.caption ? element.caption : "";
              image.FileSize = element.fileSize ? element.fileSize : -1;
              image.ImagePath = element.imagePath ? element.imagePath : "";
              image.IsDefault = element.isDefault
                ? element.isDefault.toString()
                : false;
              image.SortOrder = element.sortOrder ? element.sortOrder : 1;

              listImages.push(image);
            });

            setImages(listImages);
          }

          setLoading(false);
        } else {
          await Swal.fire({
            icon: "error",
            title: "Can not get images of product",
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

  const deleteImage = async () => {
    const ids = selectedData.map((r) => r.ImageId);
    //delete
    await DeleteImages(ids);
    setToggleCleared(!toggleCleared);
  };

  return (
    <div>
      <Card>
        <CardHeader>
          <CardTitle tag="h5">Manage Images</CardTitle>
          <CardSubtitle className="mb-2 text-muted" tag="h6">
            List Image Of Product
          </CardSubtitle>
        </CardHeader>
        <CardBody>
          <DataTable
            columns={columns}
            data={images}
            selectableRows
            pagination
            progressPending={loading}
            progressComponent={<CustomLoader />}
            highlightOnHover
            pointerOnHover
            actions={actions}
            contextActions={contextActions(deleteImage)}
            onSelectedRowsChange={handleRowSelected}
            clearSelectedRows={toggleCleared}
          />
        </CardBody>
      </Card>
    </div>
  );
}

export default ProductImageTable;
