import AxiosClient from "./AxiosClient";

const config = {
  headers: {
    Authorization: "Bearer ",
  },
};

const categoryResquest = {
  async getAll(token, languageId) {
    const url = `api/Categories?LanguageId=${languageId}`;
    config.headers.Authorization = "Bearer " + token;
    const response = await AxiosClient.get(url, config);
    return response;
  },

  getCategoryInfo(id, languageId, token) {
    config.headers.Authorization = "Bearer " + token;
    const url = `api/categories/${id}/${languageId}`;
    return AxiosClient.get(url, config);
  },

  create(data, token) {
    const url = `api/categories/`;
    config.headers = {
      Authorization: "Bearer " + token,
      "Content-Type": "multipart/form-data",
    };
    return AxiosClient.post(url, data, config);
  },

  update(id, data, token) {
    const url = `/api/categories/${id}/`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.put(url, data, config);
  },

  deleteMultiple(params, token) {
    const url = `/api/categories/`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.patch(url, params, config);
  },
};

export default categoryResquest;
