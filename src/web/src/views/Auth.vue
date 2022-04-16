<template>
  <div class="auth-holder">
    <div class="auth-form">
      <el-tabs type="border-card">
        <el-tab-pane label="Авторизация">
          <div class="input-line login-element">
            <el-input
              placeholder="Логин или E-mail"
              v-model="m_loginForm.inputLogin.text"
              @input="validateInputSimple(m_loginForm.inputLogin)"
            ></el-input>
            <i
              class="icon valid el-icon-check"
              v-if="m_loginForm.inputLogin.isValid"
            ></i>
            <i
              class="icon not-valid el-icon-close"
              v-if="!m_loginForm.inputLogin.isValid"
            ></i>
          </div>
          <div class="input-line login-element">
            <el-input
              placeholder="Пароль"
              v-model="m_loginForm.inputPassword.text"
              show-password
              @input="validateInputSimple(m_loginForm.inputPassword)"
            ></el-input>
            <i
              class="icon valid el-icon-check"
              v-if="m_loginForm.inputPassword.isValid"
            ></i>
            <i
              class="icon not-valid el-icon-close"
              v-if="!m_loginForm.inputPassword.isValid"
            ></i>
          </div>
          <el-button
            class="login-element"
            type="primary"
            v-on:click="authorize"
            :disabled="!IsLoginFormValid"
            >Авторизоваться</el-button
          >
        </el-tab-pane>
        <el-tab-pane label="Регистрация">
          <div class="input-line login-element">
            <el-input
              name="fname"
              placeholder="Имя"
              v-model="m_registerForm.inputName.text"
              @input="validateInputSimple(m_registerForm.inputName)"
            ></el-input>
            <i
              class="icon valid el-icon-check"
              v-if="m_registerForm.inputName.isValid"
            ></i>
            <i
              class="icon not-valid el-icon-close"
              v-if="!m_registerForm.inputName.isValid"
            ></i>
          </div>
          <div class="input-line login-element">
            <el-input
              name="lname"
              placeholder="Фамилия"
              v-model="m_registerForm.inputSurname.text"
              @input="validateInputSimple(m_registerForm.inputSurname)"
            ></el-input>
            <i
              class="icon valid el-icon-check"
              v-if="m_registerForm.inputSurname.isValid"
            ></i>
            <i
              class="icon not-valid el-icon-close"
              v-if="!m_registerForm.inputSurname.isValid"
            ></i>
          </div>
          <div class="input-line login-element">
            <el-input
              name="login"
              placeholder="Логин"
              v-model="m_registerForm.inputLogin.text"
              @input="validateInputSimple(m_registerForm.inputLogin)"
            ></el-input>
            <i
              class="icon valid el-icon-check"
              v-if="m_registerForm.inputLogin.isValid"
            ></i>
            <i
              class="icon not-valid el-icon-close"
              v-if="!m_registerForm.inputLogin.isValid"
            ></i>
          </div>
          <div class="input-line login-element">
            <el-input
              name="email"
              placeholder="E-mail"
              v-model="m_registerForm.inputEmail.text"
              @input="validateInputEmail(m_registerForm.inputEmail)"
            ></el-input>
            <i
              class="icon valid el-icon-check"
              v-if="m_registerForm.inputEmail.isValid"
            ></i>
            <i
              class="icon not-valid el-icon-close"
              v-if="!m_registerForm.inputEmail.isValid"
            ></i>
          </div>
          <div class="input-line login-element">
            <el-input
              placeholder="Пароль"
              v-model="m_registerForm.inputPassword.text"
              show-password
              @input="validateInputSimple(m_registerForm.inputPassword)"
            ></el-input>
            <i
              class="icon valid el-icon-check"
              v-if="m_registerForm.inputPassword.isValid"
            ></i>
            <i
              class="icon not-valid el-icon-close"
              v-if="!m_registerForm.inputPassword.isValid"
            ></i>
          </div>
          <div class="input-line login-element">
            <el-input
              placeholder="Повторите пароль"
              v-model="m_registerForm.inputPasswordRepeat.text"
              show-password
              @input="
                validatePasswordRepeat(m_registerForm.inputPasswordRepeat)
              "
            ></el-input>
            <i
              class="icon valid el-icon-check"
              v-if="m_registerForm.inputPasswordRepeat.isValid"
            ></i>
            <i
              class="icon not-valid el-icon-close"
              v-if="!m_registerForm.inputPasswordRepeat.isValid"
            ></i>
          </div>
          <el-button
            class="login-element"
            type="primary"
            v-on:click="register"
            :disabled="!IsRegisterFormValid"
            >Зарегистрироваться</el-button
          >
        </el-tab-pane>
      </el-tabs>
    </div>

    <el-card class="box-card dev">
      <div slot="header" class="clearfix">
        <span>DEV</span>
      </div>
      <el-button type="primary" v-on:click="simpleAuth"
        >Авторизация как "Тестовый пользователь"</el-button
      >
      <el-button class="dev" type="primary" v-on:click="simpleAuthIgor"
        >Авторизация как "И Горь"</el-button
      >
    </el-card>
  </div>
</template>

<script>
import axios from "axios";
import { api_url } from "../store";
import { detect } from "detect-browser";
import crypto from "crypto";

export default {
  data() {
    return {
      m_loginForm: {
        inputLogin: {
          text: "",
          isValid: false,
        },
        inputPassword: {
          text: "",
          isValid: false,
        },
      },
      m_registerForm: {
        inputLogin: {
          text: "",
          isValid: false,
        },
        inputPassword: {
          text: "",
          isValid: false,
        },
        inputPasswordRepeat: {
          text: "",
          isValid: false,
        },
        inputName: {
          text: "",
          isValid: false,
        },
        inputSurname: {
          text: "",
          isValid: false,
        },
        inputEmail: {
          text: "",
          isValid: false,
        },
      },
      is_register_form_valid: false,
    };
  },
  computed: {
    IsLoginFormValid: {
      get: function () {
        return (
          this.m_loginForm.inputLogin.isValid &&
          this.m_loginForm.inputPassword.isValid
        );
      },
    },
    IsRegisterFormValid: {
      get: function () {
        return (
          this.m_registerForm.inputLogin.isValid &&
          this.m_registerForm.inputPassword.isValid &&
          this.m_registerForm.inputPasswordRepeat.isValid &&
          this.m_registerForm.inputName.isValid &&
          this.m_registerForm.inputSurname.isValid &&
          this.m_registerForm.inputEmail.isValid
        );
      },
    },
  },
  methods: {
    validateSimple: function (input) {
      return /\S/.test(input);
    },
    validateEmail: function (input) {
      return /\S+@\S+\.\S+/.test(String(input).toLowerCase());
    },
    validateInputSimple: function (input) {
      input.isValid = this.validateSimple(input.text);
    },
    validateInputEmail: function (input) {
      input.isValid = this.validateEmail(input.text);
    },
    validatePasswordRepeat: function (input) {
      var containsChar = this.validateSimple(input.text);
      if (!containsChar) {
        input.isValid = false;
        return;
      }

      input.isValid = input.text === this.m_registerForm.inputPassword.text;
    },
    authorize: function () {
      this.auth(
        this.m_loginForm.inputLogin.text,
        this.m_loginForm.inputPassword.text
      );
    },
    register: function () {
      let hash = crypto
        .createHash("sha")
        .update(this.m_registerForm.inputPassword.text)
        .digest("hex");

      axios
        .post(api_url + "/register/register", {
          username: this.m_registerForm.inputLogin.text,
          name: this.m_registerForm.inputName.text,
          surname: this.m_registerForm.inputSurname.text,
          password: hash,
          email: this.m_registerForm.inputEmail.text,
        })
        .then(() => {
          this.$notify({
            title: "Регистрация",
            dangerouslyUseHTMLString: false,
            message: "Регистрация прошла успешно",
            type: "success",
          });

          this.auth(
            this.m_registerForm.inputLogin.text,
            this.m_registerForm.inputPassword.text
          );
        })
        .catch((error) => {
          this.$notify.error({
            title: "Регистрация",
            message: error.response.data.error_message,
          });
        });
    },
    simpleAuth: function () {
      this.auth("testuser@example.com", "super_strong_password");
    },
    simpleAuthIgor: function () {
      this.auth("i_gor@example.com", "my_password");
    },
    auth: function (login, password) {
      let hash = crypto.createHash("sha").update(password).digest("hex");

      axios
        .post(api_url + "/auth/auth", {
          login: login,
          password: hash,
          device_name: this.getDeviceDescription(),
        })
        .then((response) => {
          localStorage.setItem("refresh_token", response.data["refresh_token"]);
          localStorage.setItem("access_token", response.data["access_token"]);
          this.$root.$emit("authorized");
          this.$notify({
            title: "Авторизация",
            dangerouslyUseHTMLString: false,
            message: "Авторизация прошла успешно",
            type: "success",
          });
        })
        .catch((error) => {
          console.log(error);
        });
    },
    getDeviceDescription: function () {
      var browser = detect();
      return browser.name + " " + browser.version + " " + browser.os;
    },
  },
};
</script>

<style lang="scss">
@import "../styles/variables.scss";

.auth-holder {
  max-width: 400px;
  margin: auto;
  margin-top: 20px;
}

.dev {
  margin-top: 20px;
}

.login-element {
  margin-top: 20px;
}

.input-line {
  display: flex;
  justify-content: center;
  align-items: center;

  .icon {
    margin-left: 10px;
  }

  .valid {
    color: #67c23a;
  }

  .not-valid {
    color: #f56c6c;
  }
}
</style>
