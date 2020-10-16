<template>
    <div class="auth-holder">
        <el-input placeholder="Login" v-model="input" @input="testInput"></el-input>
        <el-input placeholder="Password" v-model="input" show-password></el-input>
    </div>
</template>

<script>

export default {
    data() {
      return {
        links: [],
        state: '',
        timeout:  null
      };
    },
    methods: {
      testInput(text)
        {
            console.log(text);
        }
    },
  };
</script>

<style>
.auth-holder {
    max-width: 400px;
    margin:auto;
    margin-top:20px;
}
</style>
