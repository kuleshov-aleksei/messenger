<template>
  <div class="admin">
    <el-tabs v-model="activeTabName">
      <el-tab-pane label="Статус сервисов" name="status">
        <ServiceStatus />
      </el-tab-pane>
      <el-tab-pane label="Пользователи" name="users">Пользователи</el-tab-pane>
      <el-tab-pane label="Чаты" name="chats">Чаты</el-tab-pane>
    </el-tabs>
  </div>
</template>


<script>
import store from "../store"
import sessionStore from "../sessionStore";
//import router from "index";

export default {
    data() {
      return {
        activeTabName: 'status'
      };
    },
    components: {
      ServiceStatus: () => import("../components/ServiceStatus.vue"),
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

<style lang="scss">
@import "../styles/variables.scss";

.admin {
  margin-top: 10px;
  min-width: 50%;
  margin: 10px auto 0 auto;
  padding: 10px;
  background: white;
  border-radius: 4px;
  box-shadow: 0 1px 0 0 var(--steel_gray_120),0 0 0 1px var(--steel_gray_80);

  @include respond-to(mobile) { max-width: 100%; }
  @include respond-to(medium-screens) { max-width: 100%; }
  @include respond-to(regular) { max-width: 60% }
  @include respond-to(wide-screens) { max-width: 50% }
}

</style>
