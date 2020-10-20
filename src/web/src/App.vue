<template>
  <div id="app">
    <HeaderComponent />
    
    <router-view />
  </div>
</template>

<script>
import HeaderComponent from "./components/HeaderComponent.vue";

export default {
    name: "App",
    components: {
        HeaderComponent,
    }
};
</script>

<style>

body {
    margin: 0 !important;
    padding: 0 !important;
}

#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.el-notification__content, .el-notification__title {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
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
</style>
