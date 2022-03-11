import AxiosClient from "./AxiosClient";

const config = {
  headers: {
    Authorization: "Bearer ",
  },
};

const ImageResquest = {
  async getAll(id, token) {
    const url = `api/Products/${id}/images`;
    config.headers.Authorization = "Bearer " + token;
    const response = await AxiosClient.get(url, config);
    return response;
  },

  create(id, data, token) {
    const url = `api/products/${id}/images`;
    config.headers = {
      Authorization: "Bearer " + token,
      "Content-Type": "multipart/form-data",
    };
    return AxiosClient.post(url, data, config);
  },

  update(idProduct, idImage, data, token) {
    const url = `/api/products/${idProduct}/images/${idImage}`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.put(url, data, config);
  },

  delete(productId, imageId, token) {
    const url = `/api/products/${productId}/images/${imageId}`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.delete(url, config);
  },
};

export default ImageResquest;
