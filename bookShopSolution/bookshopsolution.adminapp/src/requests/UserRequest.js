import AxiosClient from "./AxiosClient";

const config = {
  headers: {
    Authorization: "Bearer ",
  },
};

// const convertParams = (params) => {
//   if (!params) return {};
//   var qs = require("qs");
//   return {
//     params: {
//       ...params,
//     },
//     paramsSerializer: (params) => {
//       //ví dụ với trường hợp size=[1,2] => &size=1&size=2
//       return qs.stringify(params, { arrayFormat: "repeat" });
//     },
//   };
// };

const userResquest = {
  getUserInfo(id, token) {
    config.headers.Authorization = "Bearer " + token;
    const url = `api/users/${id}/`;
    return AxiosClient.get(url, config);
  },

  authenticate(data) {
    const url = `api/users/authenticate/`;
    return AxiosClient.post(url, data);
  },

  getAccountInfo(token) {
    config.headers.Authorization = "Bearer " + token;
    const url = `api/users/info/`;
    return AxiosClient.get(url, config);
  },

  async getPaging(pageIndex, pageSize, keyword, token) {
    const url = `api/users/paging?PageIndex=${pageIndex}&PageSize=${pageSize}&Keyword=${keyword}`;
    config.headers.Authorization = "Bearer " + token;
    const response = await AxiosClient.get(url, config);
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

  deleteMultiple(params, token) {
    const url = `/api/users/`;
    config.headers.Authorization = "Bearer " + token;
    return AxiosClient.patch(url, params, config);
  },
};

export default userResquest;
