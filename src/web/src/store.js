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
    selected_chat_id: '',
    current_route: '',
  },
  mutations: {
    set_chat_id(state, chat_id)
    {
      state.selected_chat_id = chat_id;
    },
    save_current_route(state, route)
    {
      state.current_route = route;
    }
  }
})