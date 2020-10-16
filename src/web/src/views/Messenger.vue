<template>
    <div class="messenger">
        <h1>Общение</h1>
        <el-button type="primary" v-on:click="load_chats">Load chats</el-button>
    </div>
</template>

<script>
import axios from "axios";
import store from "../store"
import { api_url } from "../store"

export default {
    data() {
      return {
      };
    },
    methods: {
      load_chats: function() {
          axios.post(api_url + "/chat/get_chat_list", {
            access_token: store.state.access_token,
          })
          .then((response) => {
            console.log(response);
          })
          .catch((error) => {
            console.log(error);
          });
        },
    },
  };
</script>