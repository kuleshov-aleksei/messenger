<template>
  <div class="settings">
    <h1>Настройки</h1>
    <h1>!!! Under constuction !!!</h1>
  </div>
</template>

<script>
import store from "../store";

export default {
  methods: {},
  mounted() {
    store.commit("save_current_route", "/settings");
  },
};
</script>