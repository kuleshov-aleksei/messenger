<template>
    <div class="add-user">
        <el-button type="text" icon="el-icon-circle-plus-outline" @click="dialogVisible = true"></el-button>

        <el-dialog
            title="Пригласить пользователя"
            :visible.sync="dialogVisible"
            width="30%">
                <el-input
                    placeholder="Введите никнейм пользователя"
                    v-model="invitedUserName"
                    clearable>
                </el-input>
                <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">Отмена</el-button>
                <el-button type="primary" @click="dialogVisible = false" v-on:click="invite_user">Пригласить</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
import axios from "axios";
import { api_url } from "../store"

export default {
    props: {
        chatId: String,
    },
    data() {
        return {
            dialogVisible: false,
            invitedUserName: '',
        };
    },
    methods: {
        invite_user: function() {
            axios.post(api_url + "/chat/invite_to_chat_username", {
                access_token: localStorage.getItem("access_token"),
                invited_username: this.invitedUserName,
                chat_id: this.chatId,
            })
            .then(() => {
                this.invitedUserName = '';
                this.$notify({
                    title: "Приглашение пользователя в чат",
                    message: "Пользователь приглашен",
                    type: 'success'
                });
            })
            .catch((error) => {
                let notificationMessage = error.response.data != null ? error.response.data : error;
                this.$notify.error({
                    title: "Не удалось пригласить пользователя в чат",
                    message: notificationMessage.error_message,
                });
            });
        },
    }
}
</script>

<style lang="scss">

.add-user {

    > button {
        font-size: 1.5em;
        margin: 0 20px 0 20px;
        color: rgb(84, 92, 100);
    }

    .el-dialog__header {
        text-align: center;
    }
}

</style>