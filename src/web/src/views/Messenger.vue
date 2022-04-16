<template>
  <div class="background-body messenger-body">
    <div id="grid">
      <div class="chat-list-header-container">
        <div class="chat-list-header">
          <el-input
            placeholder="Search"
            prefix-icon="el-icon-search"
            v-model="search_input"
            clearable
          ></el-input>
          <el-button
            class="add-button"
            icon="el-icon-plus"
            circle
            @click="dialogVisible = true"
          ></el-button>
        </div>
      </div>
      <div class="chat-header-container">
        <div class="chat-header">
          <div class="center-vertically">
            <h3>
              {{ current_chat ? current_chat.title : chat_title_placeholder }}
            </h3>
          </div>
          <el-button
            class="chat-members"
            v-if="current_chat_members.length > 1"
            type="text"
            @click="show_chat_info"
            >{{ current_chat_members.length }}
            {{ get_declination(current_chat_members.length) }}</el-button
          >
          <InviteUserToChat
            v-if="current_chat != null"
            class="center-vertically invite-component"
            :chatId="current_chat.id"
            :onSuccess="onSuccessfullInvitation"
          />
        </div>
      </div>
      <div class="chat-list-container">
        <div class="chat-list">
          <ul v-if="chats.length > 0">
            <li v-for="chat in chats" :key="chat.title">
              <div class="chat-item" @click="on_chat_selected(chat)">
                <el-avatar class="chat-medium-avatar" :src="chat.image_medium">
                  <img src="../assets/notfound.png" />
                </el-avatar>
                <div class="chat-title">{{ chat.title }}</div>
              </div>
              <hr class="hr-chat-breaker" />
            </li>
          </ul>
          <div v-if="chats.length == 0" class="chat-not-found">
            Бесед пока нет<br /><br />
            Создайте новую или попросите друзей вас пригласить.
          </div>
        </div>
      </div>
      <div class="chat">
        <Chat />
      </div>
    </div>

    <el-dialog
      title="Создать новую беседу"
      :visible.sync="dialogVisible"
      width="30%"
    >
      <el-input
        placeholder="Введите название беседы"
        v-model="new_chat_title"
        clearable
      >
      </el-input>
      <span slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">Отмена</el-button>
        <el-button
          type="primary"
          @click="dialogVisible = false"
          v-on:click="create_new_chat"
          >Создать</el-button
        >
      </span>
    </el-dialog>

    <el-dialog
      title="Информация о беседе"
      :visible.sync="chat_info_visible"
      width="50%"
      v-if="current_chat != null"
    >
      <div class="chat-info-header">
        <el-avatar class="avatar" :src="current_chat.image_medium">
          <img src="../assets/notfound.png" />
        </el-avatar>
        <div class="title-container">
          <h1>{{ current_chat.title }}</h1>
          <h2>
            {{ current_chat_members.length }}
            {{ get_declination(current_chat_members.length) }}
          </h2>
        </div>
      </div>
      <hr />
      <div class="chat-members-list">
        <ul>
          <li v-for="member in current_chat_members" :key="member.id">
            <el-avatar size="large" :src="getImgUrl(member.image_medium)">
              <img src="../assets/notfound.png" />
            </el-avatar>
            <el-button slot="reference" type="text"
              >{{ member.name }} {{ member.surname }}</el-button
            >
            <div class="username">@{{ member.username }}</div>
            <div class="member-info" v-if="member.invited_by_name == null">
              Создатель беседы
            </div>
            <div class="member-info" v-if="member.invited_by_name != null">
              Пригласил {{ member.invited_by_name }}
              {{ member.invited_by_surname }}
            </div>
          </li>
        </ul>
      </div>
      <span slot="footer" class="dialog-footer"> </span>
    </el-dialog>
  </div>
</template>

<script>
import axios from "axios";
import { api_url } from "../store";
import store from "../store";
import Chat from "../components/Chat.vue";
import InviteUserToChat from "../components/InviteUserToChat.vue";

export default {
  components: {
    Chat,
    InviteUserToChat,
  },
  data() {
    return {
      search_input: "",
      new_chat_title: "",
      dialogVisible: false,
      chat_title_placeholder: "Выберите беседу или создайте новую",
      chat_info_visible: false,
      current_chat_members: [],
      current_chat: null,
      chats: [],
    };
  },
  methods: {
    load_chats: function () {
      axios
        .post(api_url + "/chat/get_chat_list", {
          access_token: localStorage.getItem("access_token"),
        })
        .then((response) => {
          this.chats = response.data.chats;

          this.chats.forEach((chat) => {
            if (chat.id === store.state.selected_chat_id) {
              this.current_chat = chat;
              this.get_chat_members(this.current_chat.id);
            }
          });
        })
        .catch((error) => {
          console.log(error);
          if (error.response.status === 403 || error.response.status === 401) {
            this.current_chat = null;
            store.commit("save_current_route", "/auth");
            this.$router.push("/auth");
          }
        });
    },
    create_new_chat: function () {
      axios
        .post(api_url + "/chat/create_chat", {
          access_token: localStorage.getItem("access_token"),
          title: this.new_chat_title,
        })
        .then(() => {
          this.notify(
            true,
            "Создание чата",
            'Чат "' + this.new_chat_title + '" создан'
          );
          this.new_chat_title = "";
          this.load_chats();
        })
        .catch((error) => {
          this.notify(
            false,
            "Создание чата",
            "Ошибка при создании чата.<br>Код " +
              error.response.status +
              " " +
              error.response.statusText
          );
          if (error.response.status === 403 || error.response.status === 401) {
            this.current_chat = null;
            store.commit("save_current_route", "/auth");
            this.$router.push("/auth");
          }
        });
    },
    notify: function (success, title, message) {
      if (success) {
        this.$notify({
          title: title,
          dangerouslyUseHTMLString: true,
          message: message,
          type: "success",
        });
      } else {
        this.$notify.error({
          title: title,
          dangerouslyUseHTMLString: true,
          message: message,
        });
      }
    },
    on_chat_selected: function (chat) {
      this.current_chat = chat;
      this.get_chat_members(chat.id);
      store.commit("set_chat_id", chat.id);
    },
    get_chat_members: function (chat_id) {
      this.current_chat_members = [];
      axios
        .post(api_url + "/chat/get_chat_members", {
          access_token: localStorage.getItem("access_token"),
          chat_id: chat_id,
        })
        .then((response) => {
          this.current_chat_members = response.data.chat_members;
        })
        .catch((error) => {
          console.log(error);
          if (error.response.status === 403 || error.response.status === 401) {
            this.current_chat = null;
            store.commit("save_current_route", "/auth");
            this.$router.push("/auth");
          }
        });
    },
    get_declination: function (count) {
      var cases = [2, 0, 1, 1, 1, 2];
      var titles = ["участник", "участника", "участников"];
      return titles[
        count % 100 > 4 && count % 100 < 20
          ? 2
          : cases[count % 10 < 5 ? count % 10 : 5]
      ];
    },
    show_chat_info: function () {
      this.chat_info_visible = true;
    },
    onSuccessfullInvitation: function () {
      this.get_chat_members(this.current_chat.id);
    },
    getImgUrl(pic) {
      if (pic === null) {
        return null;
      }

      return require("../assets/" + pic);
    },
  },
  mounted() {
    store.commit("save_current_route", "/");
    this.load_chats();
  },
};
</script>

<style lang="scss">
@import "../styles/variables.scss";

.messenger-body {
  padding: 0;
}

.add-button {
  margin-left: 5px;
}

.chat-list-header {
  margin: auto;
  display: flex;
  padding: 0 10px 0 10px;
  margin-top: 20px;
}

.chat-list {
  padding: 0 10px 0 10px;

  ul {
    list-style-type: none;
    text-align: left;
  }
}

.chat-not-found {
  text-align: center;
}

.chat-item {
  display: flex;
  padding: 10px 0 10px 0;

  .chat-medium-avatar {
    width: 48px;
    height: 48px;
    min-width: 48px;
  }
}

.chat-item > .chat-title {
  font-size: 16px;
  margin-top: auto;
  margin-bottom: auto;
  text-align: left;
  padding-left: 10px;
}

.hr-chat-breaker {
  background-color: $border-color;
  border: none;
  height: 1px;
  opacity: 0.5;
}

#grid {
  display: grid;
  grid-template-areas:
    "chat-list-header chat-header"
    "chat-list chat";
  grid-template-rows: 80px 90%;
  grid-template-columns: 25% auto;
  grid-gap: 1px;
  margin: 0;
  margin-top: 10px;
  grid-gap: 0px;
  position: relative;
}

#grid > div {
  font-size: 18px;
  border-right: 1px solid $border-color;
  border-bottom: 1px solid $border-color;
  margin: 0;
  padding: 0;
  -webkit-box-sizing: border-box;
  -moz-box-sizing: border-box;
  box-sizing: border-box;
}

#grid div:nth-child(2n) {
  border-right: none;
}
#grid div:nth-child(2) ~ div {
  border-bottom: none;
}

.chat {
  margin-left: 20px;
}

.chat-header {
  text-align: left;
  margin-top: auto;
  margin-bottom: auto;
  display: flex;
  flex-wrap: wrap;
  height: 100%;

  .chat-members {
    margin-top: auto;
    margin-bottom: auto;
    padding-left: 20px;
  }

  > button > span {
    color: gray;
  }

  h3 {
    padding: 0 0 0 50px;
  }
}

ul {
  padding-left: 0px;
}

.chat-info-header {
  display: flex;
  margin-bottom: 20px;

  .avatar {
    height: 120px;
    width: 120px;
    margin-left: 20px;
  }

  .title-container {
    margin: auto 0 auto 0;
    margin-left: 50px;

    h1 {
      color: black;
      text-align: left;
    }

    h2 {
      color: gray;
      font-size: 1em;
      text-align: left;
    }
  }
}

.chat-members-list {
  li {
    text-align: left;
    display: flex;
    justify-content: left;
    align-items: center;
    margin-bottom: 10px;

    button {
      font-size: 1.5em;
      padding-left: 15px;
    }

    .member-info {
      margin-left: auto;
    }

    .username {
      padding-left: 12px;
    }
  }

  ul {
    list-style-type: none;
  }
}

.el-popover__title {
  margin-bottom: 0;
  text-align: center;
}

.invite-component {
  margin-left: auto;
}
</style>