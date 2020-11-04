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
@import "styles/variables.scss";

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

.center-horizonally {
  margin-left: auto;
  margin-right: auto;
}

.background-body {
  margin-top: 10px;
  min-width: 50%;
  margin: 10px auto 0 auto;
  padding: 10px;
  background: white;
  border-radius: 4px;
  box-shadow: 0 1px 0 0 var(--steel_gray_120),0 0 0 1px var(--steel_gray_80);

  @include respond-to(mobile) { max-width: 100%; }
  @include respond-to(medium-screens) { max-width: 100%; }
  @include respond-to(regular) { max-width: 60% }
  @include respond-to(wide-screens) { max-width: 50% }
}

.el-avatar {
  background: none;
}

</style>
