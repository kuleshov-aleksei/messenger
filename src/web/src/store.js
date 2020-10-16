import Vue from 'vue'
import { createLogger } from 'vuex'
import Vuex from 'vuex'
import createPersistedState from 'vuex-persistedstate'
import * as Cookies from 'js-cookie'

Vue.use(Vuex)
Vue.config.debug = true

const debug = process.env.NODE_ENV !== 'production'

export const api_url = 'http://api.encamy.keenetic.pro';

export default new Vuex.Store({
  plugins: [createPersistedState({ 
    getState: (key) => Cookies.getJSON(key), 
    setState: (key, state) => Cookies.set(key, state, { expires: 60, secure: false }) 
 })],
  modules: {
  },
  strict: debug,
  middlewares: debug ? [createLogger()] : [],  
  state: {
    access_token: '',
    refresh_token: '',
  },
  mutations: {
    save_access_token(state, token)
    {
      state.access_token = token;
    },
    save_refresh_token(state, token)
    {
      state.refresh_token = token;
    }
  }
})