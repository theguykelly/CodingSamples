import axios from "axios";
import * as helpers from "./serviceHelpers";

const insert = data => {
  const config = {
    method: "POST",
    url: helpers.API_HOST_PREFIX + `/api/influencers/`,
    data: data,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const selectAll = () => {
  const config = {
    method: "GET",
    url: helpers.API_HOST_PREFIX + `/api/influencers`,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const get10 = index => {
  const config = {
    method: "GET",
    url:
      helpers.API_HOST_PREFIX +
      `/api/influencers/paginate?pageIndex=${index}&pageSize=10`,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const paginateSearch = (search, index) => {
  const config = {
    method: "GET",
    url:
      helpers.API_HOST_PREFIX +
      `/api/influencers/search?search=${search}&pageIndex=${index}&pageSize=10`,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const selectById = id => {
  const config = {
    method: "GET",
    url: helpers.API_HOST_PREFIX + `/api/influencers/${id}`,
    data: id,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const selectByUserId = id => {
  const config = {
    method: "GET",
    url: helpers.API_HOST_PREFIX + `/api/influencers/userId/${id}`,
    data: id,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const update = (payload, id) => {
  const config = {
    method: "PUT",
    url: helpers.API_HOST_PREFIX + `/api/influencers/${id}`,
    data: payload,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const deleteById = id => {
  const config = {
    method: "DELETE",
    url: helpers.API_HOST_PREFIX + `/api/influencers/${id}`,
    data: id,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const activate = id => {
  const config = {
    method: "UPDATE",
    url: helpers.API_HOST_PREFIX + `/api/influencers/activate/${id}`,
    data: id,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

const deactivate = id => {
  const config = {
    method: "UPDATE",
    url: helpers.API_HOST_PREFIX + `/api/influencers/deactivate/${id}`,
    data: id,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

export {
  get10,
  insert,
  selectAll,
  selectById,
  update,
  deleteById,
  selectByUserId,
  activate,
  deactivate,
  paginateSearch
};
