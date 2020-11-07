<template>
    <div class="auth-holder">
      <div class="auth-form">
        <el-input placeholder="Login" v-model="text_input_login"></el-input>
        <el-input placeholder="Password" v-model="text_input_password" show-password></el-input>
        <el-button type="primary" v-on:click="authorize">Авторизоваться</el-button>
      </div>

      <div class="dev">
        <el-button type="primary" v-on:click="simpleAuth">Авторизуй меня (DEV) postman rect</el-button>
      </div>
      <div class="dev">
        <el-button type="primary" v-on:click="simpleAuthIgor">Авторизуй меня (DEV) И Горь</el-button>
      </div>
    </div>
</template>

<script>
import axios from "axios";
import { api_url } from "../store"
import { detect } from "detect-browser";
import crypto from "crypto";

export default {
    data() {
      return {
        text_input_login: '',
        text_input_password: '',
      };
    },
    methods: {
      authorize: function() {
        this.auth(this.text_input_login, this.text_input_password);
      },
      simpleAuth: function () {
        this.auth("testuser@example.com", "super_strong_password");
      },
      simpleAuthIgor: function() {
        this.auth("i_gor@example.com", "my_password");
      },
      auth: function(login, password) {
        let hash = crypto.createHash('sha').update(password).digest('hex');

        axios.post(api_url + "/auth/auth", {
          login: login,
          password: hash,
          device_name: this.getDeviceDescription(),
        })
        .then((response) => {
          localStorage.setItem("refresh_token", response.data["refresh_token"]);
          localStorage.setItem("access_token", response.data["access_token"]);
          this.$root.$emit('authorized');
          this.$notify({
            title: "Авторизация",
            dangerouslyUseHTMLString: false,
            message: "Авторизация прошла успешно",
            type: 'success'
          });
        })
        .catch((error) => {
          console.log(error);
        });
      },
      getDeviceDescription: function() {
        var browser = detect();
        return browser.name + " " + browser.version + " " + browser.os;
      }
    }
  };
</script>

<style>
.auth-holder {
    max-width: 400px;
    margin: auto;
    margin-top: 20px;
}

.dev {
  margin-top: 40px;
}
</style>
