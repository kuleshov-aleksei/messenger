<template>
    <div class="playground">
      <div class="dev">
        <el-button type="primary" v-on:click="send_ping">Send Ping</el-button>
      </div>
    </div>
</template>

<script>
//import store from "../store"

export default {
    data() {
      return {
      };
    },
    methods: {
      send_ping: function() {
        console.log("test");

      },
    },
  };
</script>

<style>
.dev {
  margin-top: 40px;
}
</style>
