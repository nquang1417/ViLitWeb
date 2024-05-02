import { inject } from "vue";
import { tokenAlive } from "../../shared/jwtHelper";
import { jwtDecrypt } from "../../shared/jwtHelper";

const state = () => {
  const token = localStorage.getItem('access_token');
  var jwtDecodedValue = token ? jwtDecrypt(token) : null;    
  // var jwtDecodedValue = token ? jwtDecode(token) : null;

  if (jwtDecodedValue && !tokenAlive(jwtDecodedValue.exp, jwtDecodedValue.role)) {
    jwtDecodedValue = null;
    localStorage.removeItem('access_token');
  }

  return {
    authData: {
      token: jwtDecodedValue ? token : "",
      tokenExp: jwtDecodedValue ? jwtDecodedValue.exp : "",
      userId: jwtDecodedValue ? jwtDecodedValue.sub : "",
      username: jwtDecodedValue ? jwtDecodedValue.username : "",
      name: jwtDecodedValue ? jwtDecodedValue.name : "",
      role: jwtDecodedValue ? jwtDecodedValue.role : "",
    },
    loginStatus: jwtDecodedValue ? tokenAlive(jwtDecodedValue.exp, jwtDecodedValue.role) : false,
    
  }
};

const getters = {
  getLoginStatus(state) {
    return state.loginStatus;
  },
  getAuthData(state) {
    return state.authData;
  },
  isTokenActive(state) {
    if (!state.authData.tokenExp) {
      return false;
    }
    return tokenAlive(state.authData.tokenExp, state.authData.role);
  },
};

import axios from 'axios'


const actions = {
  async login({ commit }, payload) {
    const response = await axios
      .post("http://localhost:10454/api/Login", payload)
      .catch((err) => { console.log(err) })
    if (response && response.data) {
      commit("saveTokenData", response.data);
      commit("setLoginStatus", true);
    } else {
      commit("setLoginStatus", false);
    }
  },
  logout({ commit }) {
    commit("clearTokenData");
    commit("setLoginStatus", false);
  }
};

const mutations = {
  saveTokenData(state, data) {

    localStorage.setItem("access_token", data.token);

    const jwtDecodedValue = jwtDecrypt(data.token);
    // const jwtDecodedValue = jwtDecode(data.token);
    const newTokenData = {
      token: data.token,
      tokenExp: jwtDecodedValue.exp,
      userId: jwtDecodedValue.sub,
      username: jwtDecodedValue.username,
      name: jwtDecodedValue.name,
      role: jwtDecodedValue.role
    };
    state.authData = newTokenData;
  },
  setLoginStatus(state, value) {
    state.loginStatus = value;
  },
  clearTokenData(state) {
    const emptyToken = {
      token: '',
      tokenExp: '',
      userId: '',
      username: '',
      name: '',
      role: ''
    }
    state.authData = emptyToken;
    localStorage.removeItem("access_token");
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
}