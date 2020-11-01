<template>
  <div class="admin">
    <h1>Администрирование</h1>
  </div>
</template>


<script>
import store from "../store"
import sessionStore from "../sessionStore";
//import router from "index";

export default {
    data() {
      return {
      };
    },
    methods: {
        
    },
    mounted() {
      store.commit('save_current_route', '/admin');

      if (!sessionStore.state.roles.includes('admin'))
      {
        this.$notify.error({
            title: 'Отказано в доступе',
            message: "Вы не имеете доступа к данному разделу"
        });
        this.$router.push('/');
      }
    }
  };
</script>

<style>

</style>
