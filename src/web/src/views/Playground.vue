<template>
  <div class="playground">
    <div class="dev">
      <el-input placeholder="Please input" v-model="input"></el-input>
      <el-button type="primary" class="send_button" v-on:click="do_smth"
        >Send Echo</el-button
      >
    </div>
    <JistiWrapper />
  </div>
</template>

<script>
//import store from "../store"
import JistiWrapper from "../components/JitsiWrapper";

export default {
  components: {
    JistiWrapper
  },
  data() {
    return {
      input: "Hello World!",
    };
  },
  created: function () {
    
  },
  mounted() {

  },
  methods: {
    do_smth: function() {

    },
  }
};
</script>

<style lang="scss">
.dev {
  margin-top: 40px;
  max-width: 400px;
  margin: auto;
  margin-top: 20px;

  .send_button {
    margin-top: 20px;
  }
}
</style>
