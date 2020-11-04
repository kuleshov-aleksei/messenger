<template>
  <div id="app">
    <HeaderComponent />
    
    <router-view />
  </div>
</template>

<script>
import HeaderComponent from "./components/HeaderComponent.vue";
import axios from "axios";
import { api_url } from "./store"

var REFRESH_INTERVAL = 10 * 60 * 1000;

export default {
    name: "App",
    components: {
        HeaderComponent,
    },
    data() {
      return {
        refreshTimer: null
      };
    },
    methods: {
      refreshToken: function() {
        if (localStorage.getItem("refresh_token") === null)
        {
          clearInterval(this.refreshTimer);
          return;
        }
        console.log("Refreshing access token");

        axios.post(api_url + "/auth/refresh", {
          refresh_token: localStorage.getItem("refresh_token"),
          access_token: localStorage.getItem("access_token")
        })
        .then((response) => {
          localStorage.setItem("refresh_token", response.data["refresh_token"]);
          localStorage.setItem("access_token", response.data["access_token"]);
        })
        .catch((error) => {
          console.log(error);
        });
      },
      createRefreshTimer: function() {
        this.refreshTimer = setInterval(this.refreshToken, REFRESH_INTERVAL);
      }
    },
    mounted: function() {
      this.$root.$on('authorized', this.createRefreshTimer);

      if (localStorage.getItem("refresh_token") !== null)
      {
        this.createRefreshTimer();
      }
    }
};
</script>

<style lang="scss">

body {
    margin: 0 !important;
    padding: 0 !important;
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    background: rgb(236, 236, 236);
}

#app {
  text-align: center;
  color: #2c3e50;
}

.el-notification__content, .el-notification__title {
  color: #2c3e50;
}

#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}

* {
  scrollbar-width: thin;
  scrollbar-color: #555 #F5F5F5;
}

/* Works on Chrome/Edge/Safari */
*::-webkit-scrollbar {
  width: 8px;
  background-color: #F5F5F5;
}
*::-webkit-scrollbar-track {
  border-radius: 10px;
  background-color: #F5F5F5;
}
*::-webkit-scrollbar-thumb {
  border-radius: 10px;
  background-color: rgb(84, 92, 100);
}

.center-vertically {
  margin-top: auto;
  margin-bottom: auto;
}
</style>
