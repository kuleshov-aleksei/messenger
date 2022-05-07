<template>
  <div class="background-body">
    <el-tabs v-model="activeTabName">
      <el-tab-pane label="Статус сервисов" name="status">
        <ServiceStatus />
      </el-tab-pane>
      <el-tab-pane label="Пользователи" name="users">Пользователи</el-tab-pane>
      <el-tab-pane label="Чаты" name="chats">Чаты</el-tab-pane>
      <el-tab-pane label="Шаблоны" name="templates">
        <Templates />
      </el-tab-pane>
    </el-tabs>
  </div>
</template>


<script>
import store from "../store";
import sessionStore from "../sessionStore";
//import router from "index";

export default {
  data() {
    return {
      activeTabName: "status",
    };
  },
  components: {
    ServiceStatus: () => import("../components/ServiceStatus.vue"),
    Templates: () => import("../views/Templates.vue"),
  },
  methods: {},
  mounted() {
    store.commit("save_current_route", "/admin");

    if (!sessionStore.state.roles.includes("admin")) {
      this.$notify.error({
        title: "Отказано в доступе",
        message: "Вы не имеете доступа к данному разделу",
      });
      this.$router.push("/");
    }
  },
};
</script>

<style lang="scss">
</style>
