import AxiosClient from "./AxiosClient";

const config = {
  headers: {
    Authorization: "Bearer ",
  },
};

const productResquest = {
  async getPaging(pageIndex, pageSize, keyword, languageId, categoryId, token) {
    const url = `api/Products/paging?PageIndex=${pageIndex}&PageSize=${pageSize}&Keyword=${keyword}&LanguageId=${languageId}&CategoryId=${categoryId}`;
    config.headers.Authorization = "Bearer " + token;
    const response = await AxiosClient.get(url, config);
    return response;
  },

  getProductInfo(id, languageId, token) {
    config.headers.Authorization = "Bearer " + token;
    const url = `api/products/${id}/${languageId}`;
    return AxiosClient.get(url, config);
  },

  create(data, token) {
    const url = `api/products/`;
    config.headers = {
      Authorization: "Bearer " + token,
      "Content-Type": "multipart/form-data",
    };
    return AxiosClient.post(url, data, config);
  },

  assignCategory(id, data, token) {
    const url = `/api/Products/${id}/categories`;
    config.headers = {
      Authorization: "Bearer " + token,
      "Content-Type": "application/json",
    };
    return AxiosClient.put(url, data, config);
  },

  delete(productId, token) {
    const url = `/api/products/${productId}`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.delete(url, config);
  },

  update(id, data, token) {
    const url = `/api/products/${id}/`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.put(url, data, config);
  },
};

export default productResquest;
