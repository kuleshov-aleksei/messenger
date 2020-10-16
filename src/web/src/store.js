import Vue from 'vue'
import { createLogger } from 'vuex'
import Vuex from 'vuex'

Vue.use(Vuex)
Vue.config.debug = true

const debug = process.env.NODE_ENV !== 'production'

export default new Vuex.Store({
  modules: {
  },
  strict: debug,
  middlewares: debug ? [createLogger()] : [],
  state: {
    api_url: 'http://api.encamy.keenetic.pro',
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