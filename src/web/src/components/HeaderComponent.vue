<template>
    <div class="header-wrapper">
        <div class="header-container">
            <el-menu
                class="el-menu"
                mode="horizontal"
                background-color="#545c64"
                text-color="#fff"
                active-text-color="#ffd04b"
                router
                :default-active="current_route"
            >
                <el-menu-item class="el-menu-item" index="/">
                    <img src="@/assets/logo.png" alt="logo" width="40" height="40" class="logo-img" />Calamity
                </el-menu-item>
                <el-submenu index="/profile">
                    <template slot="title" class="dropdown">Профиль</template>
                    <el-menu-item index="/profile"><i class="el-icon-user-solid" />Аккаунт</el-menu-item>
                    <el-menu-item index="/settings"><i class="el-icon-setting" />Настройки</el-menu-item>
                </el-submenu>
                <el-menu-item v-if="showAdmin" class="el-menu-item" index="/admin">
                    <i class="el-icon-user-solid" />Администрирование
                </el-menu-item>
            </el-menu>
        </div>
    </div>
</template>

<script>
import store from "../store";
import sessionStore from "../sessionStore";
import jwt_decode from "jwt-decode";

export default {
  data() {
    return {
        showAdmin: false,
    };
  },
  mounted: function() {
    this.checkPermission();
    this.$root.$on('authorized', this.checkPermission);
  },
  methods: {
      checkPermission: function() {
        if (localStorage.getItem("access_token") !== null)
        {
            var decodedJWT = jwt_decode(localStorage.getItem("access_token"));
            sessionStore.commit('set_roles', decodedJWT.role.split(','));
        }

        if (sessionStore.state.roles.includes('admin'))
        {
            this.showAdmin = true;
        }
        else
        {
            this.showAdmin = false;
        }
      }
  },
  computed: {
      current_route: {
      get: function() {
        return store.state.current_route;
      },
      set: function(newRoute) {
        store.commit('current_route', newRoute);
      }
    }
  }
}
</script>

<style lang="scss">
@import "../styles/variables.scss";

.el-header {
    padding: 0 !important;
}

.dropdown {
    flex-direction: row;
}

.el-menu {
    border-width: 0px !important;
}

.header-wrapper {
    width: 100%;
    background: #545c64;
}

.header-container {
    margin: auto;
    width: 50%;
}

.first-right-el {
    margin-left: 30vw;
    @include respond-to(mobile) { margin-left: 0; }
    @include respond-to(medium-screens) { margin-left: 20vw; }
}

.logo-img {
    padding-right: 20px;
}

</style>