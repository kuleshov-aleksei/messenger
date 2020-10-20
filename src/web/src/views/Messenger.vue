<template>
    <div class="messenger">
        <el-dialog
            title="Создать новый чат"
            :visible.sync="dialogVisible"
            width="30%">
            <el-input
                placeholder="Введите название чата"
                v-model="new_chat_title"
                clearable>
            </el-input>
            <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">Отмена</el-button>
                <el-button type="primary" @click="dialogVisible = false" v-on:click="create_new_chat">Создать</el-button>
            </span>
        </el-dialog>

        <div id="grid">
            <div class="chat-list-header">
                <el-input placeholder="Search" prefix-icon="el-icon-search" v-model="search_input" clearable></el-input>
                <el-button class="add-button" icon="el-icon-plus" circle @click="dialogVisible = true"></el-button>
            </div>
            <div class="chat-header">Title placeholder</div>
            <div class="chat-list">
                <ul v-if="chats.length > 0">
                    <li v-for="chat in chats" :key="chat.title">
                        <div class="chat-item">
                            <el-avatar size="medium" :src="chat.image_medium"></el-avatar><div class="chat-title">{{ chat.title }}</div>
                        </div>
                        <hr class="hr-chat-breaker">
                    </li>
                </ul>
                <div v-if="chats.length == 0" class="chat-not-found">Бесед пока нет<br><br>
                Создайте новый или попросите друзей вас пригласить.</div>
            </div>
            <div class="chat">Выберите чат или создайте новый</div>
        </div>
    </div>
</template>

<script>
import axios from "axios";
import store from "../store"
import { api_url } from "../store"

export default {
    data() {
      return {
        search_input: '',
        new_chat_title: '',
        dialogVisible: false,
        chats: [
        ]
      };
    },
    methods: {
        load_chats: function() {
            axios.post(api_url + "/chat/get_chat_list", {
                access_token: store.state.access_token,
            })
            .then((response) => {
                console.log(response);
                this.chats = response.data.chats;
                for (var i = 0; i < this.chats.length; i++) {
                    if (this.chats[i].image_medium == null)
                    {
                        this.chats[i].image_medium = "https://help-zte.ru/assets/img/icon/notfound.png"
                    }
                }
            })
            .catch((error) => {
                console.log(error);
            });
        },
        create_new_chat: function() {
            axios.post(api_url + "/chat/create_chat", {
                access_token: store.state.access_token,
                title: this.new_chat_title,
            })
            .then(() => {
                this.notify(true, "Создание чата", "Чат \"" + this.new_chat_title + "\" создан");
                this.new_chat_title = '';
                this.load_chats();
            })
            .catch((error) => {
                this.notify(false, "Создание чата", "Ошибка при создании чата.<br>Код " + error.response.status + " " + error.response.statusText);
            });
        },
        notify: function(success, title, message){
            if (success) {
                this.$notify({
                    title: title,
                    dangerouslyUseHTMLString: true,
                    message: message,
                    type: 'success'
                });
            }
            else {
                this.$notify.error({
                    title: title,
                    dangerouslyUseHTMLString: true,
                    message: message,
                });
            }
        },
    },
    mounted() {
        this.load_chats();
    }
  };
</script>

<style>
.messenger {
    max-width: 70%;
    margin-left: auto;
    margin-right: auto;
}

.add-button {
    margin-left: 5px;
}

.chat-list-header {
    margin: auto;
    display: flex;
}

.chat-list > ul {
    list-style-type: none;
    text-align: left;
}

.chat-not-found {
    text-align: center;
}

.chat-item {
    display: flex;
    padding: 10px 0 10px 0;
}

.chat-item > .chat-title {
    font-size: 14px;
    margin-top: auto;
    margin-bottom: auto;
    text-align: left;
    padding-left: 10px;
}

.hr-chat-breaker {
    color: gray;
    background-color: gray;
    border: none;
    height: 1px;
    opacity: .5;
}

#grid { 
    display: grid;
    grid-template-areas: 
        "chat-list-header chat-header"
        "chat-list chat";
    grid-template-rows: 60px 100%;
    grid-template-columns: 20% 1fr;
    grid-gap: 1px;
    height: 90vh;
    margin: 0;
    margin-top: 10px;
  }
#grid > div {
  font-size: 18px;
  text-align: center;
}

ul {
    padding-left: 0px;
}
</style>