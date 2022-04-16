<template>
  <div class="settings">
    <h1>Выйти</h1>
  </div>
</template>

<script>
import store from "../store";

export default {
  methods: {},
  mounted() {
    localStorage.setItem("refresh_token", "");
    localStorage.setItem("access_token", "");

    store.commit("save_current_route", "/auth");
    this.$router.push("/auth");
  },
};
</script>