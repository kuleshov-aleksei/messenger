import Vue from 'vue'
import { createLogger } from 'vuex'
import Vuex from 'vuex'

Vue.use(Vuex)
Vue.config.debug = true

const debug = process.env.NODE_ENV !== 'production'

export default new Vuex.Store({
  strict: debug,
  middlewares: debug ? [createLogger()] : [],
  state: {
    roles: [],
  },
  mutations: {
    set_roles(state, roles) {
      state.roles = roles;
    },
  }
});