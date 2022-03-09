import AxiosClient from "./AxiosClient";

const config = {
  headers: {
    Authorization: "Bearer token",
  },
};

const convertParams = (params) => {
  if (!params) return {};
  var qs = require("qs");
  return {
    params: {
      ...params,
    },
    paramsSerializer: (params) => {
      //ví dụ với trường hợp size=[1,2] => &size=1&size=2
      return qs.stringify(params, { arrayFormat: "repeat" });
    },
  };
};

const userResquest = {
  // get(id) {
  //     const url = `/products/${id}/`;
  //     return axiosClient.get(url);
  // },

  authenticate(data) {
    const url = `api/users/authenticate/`;
    return AxiosClient.post(url, data);
  },

  getInfo(token) {
    config.headers.Authorization = "Bearer " + token;
    const url = `api/users/info/`;
    return AxiosClient.get(url, config);
  },

  async getAll(params, token) {
    config.headers.Authorization = "Bearer " + token;
    const response = await AxiosClient.get(
      "api/users/paging",
      convertParams(params),
      config
    );
    return response;
  },

  create(data, token) {
    const url = `api/users/`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.post(url, data, config);
  },

  update(id, data, token) {
    const url = `/api/users/${id}/`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.put(url, data, config);
  },

  remove(id) {
    const url = `/products/${id}/`;
    return AxiosClient.delete(url);
  },
};

export default userResquest;
