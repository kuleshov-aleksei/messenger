<template>
  <div class="settings">
    <h1>Настройки</h1>
    <el-button type="primary" v-on:click="ping">Кнопка которая отправляет POST запрос</el-button>
  </div>
</template>

<script>
import axios from "axios";
import store from "../store"

export default {
    methods: {
        ping: function () {
            axios
                .get("http://192.168.40.43:16281/ping")
                .then((response) => {
                    console.log(response.data);
                })
                .catch((error) => {
                    console.log(error);
                });
        },
    },
    mounted() {
        store.commit('save_current_route', '/settings');
    }
};
</script>