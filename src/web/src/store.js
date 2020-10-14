import Vue from 'vue'
import { createLogger } from 'vuex'
import Vuex from 'vuex'
import Auth from './pages/auth/Auth.vue';

Vue.use(Vuex)
Vue.config.debug = true

const debug = process.env.NODE_ENV !== 'production'

export default new Vuex.Store({
  modules: {
    Auth
  },
  strict: debug,
  middlewares: debug ? [createLogger()] : []
})