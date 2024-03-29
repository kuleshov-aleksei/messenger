import Vue from 'vue'
import VueRouter from 'vue-router'
import Messenger from '../views/Messenger.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Messenger',
    component: Messenger
  },
  {
    path: '/settings',
    name: 'Settings',
    component: () => import('../views/Settings.vue')
  },
  {
    path: '/logout',
    name: 'Logout',
    component: () => import('../views/Logout.vue')
  },
  {
    path: '/auth',
    name: 'Auth',
    component: () => import('../views/Auth.vue')
  },
  {
    path: '/profile',
    name: 'Profile',
    component: () => import('../views/Profile.vue')
  },
  {
    path: '/playground',
    name: 'Playground',
    component: () => import('../views/Playground.vue')
  },
  {
    path: '/admin',
    name: 'Administration',
    component: () => import('../views/Admin.vue')
  },
  { 
    path: '/404', 
    name: '404', 
    component: () => import('../views/NotFound.vue'), 
  }, { 
    path: '*', 
    redirect: '/404' 
  }
]

const router = new VueRouter({
  routes
})

export default router
