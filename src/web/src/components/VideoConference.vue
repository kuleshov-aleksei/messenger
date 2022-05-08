<template>
  <div class="video-conference">
    <el-button
      type="text"
      icon="el-icon-phone"
      @click="dialogVisible = true"
    ></el-button>

    <div class="video-conference-panel-wrapper" v-if="dialogVisible === true">
        <el-card class="video-conference-panel">
        <div slot="header" class="clearfix">
            <span>Конференция чата {{this.chatTitle}}</span>
            <el-button style="float: right; padding: 3px 0" type="text" @click="dialogVisible = false">Закрыть</el-button>
        </div>
        <JistiWrapper
            :userName="userData.name + ' ' + userData.surname"
            :roomId="'calamity-room-' + chatId"
        />
        </el-card>
    </div>
  </div>
</template>

<script>
import JistiWrapper from "../components/JitsiWrapper";
import axios from "axios";
import { api_url } from "../store";

export default {
  components: {
    JistiWrapper,
  },
  props: {
    chatId: String,
    chatTitle: String,
    userName: String,
  },
  data() {
    return {
      dialogVisible: false,
      userData: null,
    };
  },
  mounted() {
      this.loadUserData();
  },
  methods: {
      loadUserData() {
        axios
        .post(api_url + "/user/get_info", {
          access_token: localStorage.getItem("access_token"),
        })
        .then((response) => {
          this.userData = response.data;
        })
      }
  }
};
</script>

<style lang="scss">
@import "../styles/variables.scss";

.video-conference {
    > button {
        font-size: 1.5em;
        margin: 0 20px 0 20px;
        color: rgb(84, 92, 100);
    }
}

.video-conference-panel-wrapper {
    width: 70%;
    height: 70%;
    position: fixed;
    z-index: 20;
    left: 0;
    top: 10%;
    left: 50%;
    bottom: 15px;
    transform: translateX(-50%);

    .video-conference-panel {
        width: 100%;
        height: 100%;
        background: white;

        .el-card__body {
            padding: 0;
        }
    }
}

</style>