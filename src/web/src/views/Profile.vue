<template>
  <div class="profile">
    <h1>Профиль</h1>
  </div>
</template>


<script>
import store from "../store"

export default {
    data() {
      return {
      };
    },
    methods: {
        
    },
    mounted() {
        store.commit('save_current_route', '/profile');
    }
  };
</script>

<style>

</style>
