import Vue from 'vue'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import App from './App.vue';
import router from './router'

import { library } from '@fortawesome/fontawesome-svg-core'
import { faUserSecret, faCircleXmark, faBolt, faSignOutAlt, faCog, faUser } from '@fortawesome/free-solid-svg-icons'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

library.add(faUserSecret, faCircleXmark, faBolt, faSignOutAlt, faCog, faUser);
Vue.component('font-awesome-icon', FontAwesomeIcon);

Vue.config.productionTip = false;
Vue.use(ElementUI);

import VueChatScroll from 'vue-chat-scroll'
Vue.use(VueChatScroll)

new Vue({
  el: '#app',
  router,
  render: h => h(App)
});
