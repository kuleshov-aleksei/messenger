<template>
    <div id="app">
        <MainPageComponent />
    </div>
</template>

<script>
import MainPageComponent from "./components/MainPageComponent.vue";

export default {
    name: "App",
    components: {
        MainPageComponent,
    },
};
</script>

<style>
#app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
}
body {
    margin: 0 !important;
    padding: 0 !important;
}
</style>
