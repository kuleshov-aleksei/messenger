<template>
    <div class="profile background-body">
        <h1>Профиль</h1>
        <i v-if="userData == null" class="el-icon-loading"></i>
        <div v-if="userData != null">
            <div class="profile-info">
                <div class="profile-container">
                    <table class="profileTable center-horizonally">
                        <thead>
                            <tr class="invisible">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td style="min-width:120px;"></td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td rowspan="4" class="picture-td">
                                    <div class="profile-picture">
                                        <el-avatar size="large" :src="getImgUrl(userData.image_large)">
                                            <img src="../assets/notfound.png" />
                                        </el-avatar>
                                    </div>
                                </td>
                                <td>Логин:</td>
                                <td>
                                    <h2>@{{ userData.username }}</h2>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>Имя:</td>
                                <td v-if="changingName === false">
                                    {{ userData.name }}
                                </td>
                                <td v-if="changingName === true">
                                    <el-input :placeholder="userData.name" v-model="newName" @input="nameChanged"></el-input>
                                </td>

                                <td v-if="changingName === false">
                                    <el-button type="text" @click="changingName = true">Изменить</el-button>
                                </td>
                                <td v-if="changingName === true">
                                    <el-button-group>
                                        <el-button type="success" icon="el-icon-check" @click="changeName" :disabled="!newNameValid"></el-button>
                                        <el-button type="warning" icon="el-icon-close" @click="changingName = false"></el-button>
                                    </el-button-group>
                                </td>
                            </tr>
                            <tr>
                                <td>Фамилия:</td>
                                <td v-if="changingSurname === false">
                                    {{ userData.surname }}
                                </td>
                                <td v-if="changingSurname === true">
                                    <el-input :placeholder="userData.surname" v-model="newSurname" @input="surnameChanged"></el-input>
                                </td>

                                <td v-if="changingSurname === false">
                                    <el-button type="text" @click="changingSurname = true">Изменить</el-button>
                                </td>
                                <td v-if="changingSurname === true">
                                    <el-button-group>
                                        <el-button type="success" icon="el-icon-check" @click="changeSurname" :disabled="!newSurnameValid"></el-button>
                                        <el-button type="warning" icon="el-icon-close" @click="changingSurname = false"></el-button>
                                    </el-button-group>
                                </td>
                            </tr>
                            <tr>
                                <td>Почта:</td>
                                <td v-if="changingEmail === false">
                                    {{ userData.email }}
                                </td>
                                <td v-if="changingEmail === true">
                                    <el-input :placeholder="userData.email" v-model="newEmail" @input="emailChanged"></el-input>
                                </td>

                                <td v-if="changingEmail === false">
                                    <el-button type="text" @click="changingEmail = true">Изменить</el-button>
                                </td>
                                <td v-if="changingEmail === true">
                                    <el-button-group>
                                        <el-button type="success" icon="el-icon-check" @click="changeEmail" :disabled="!newEmailValid"></el-button>
                                        <el-button type="warning" icon="el-icon-close" @click="changingEmail = false"></el-button>
                                    </el-button-group>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="roles-container">
                    <h2>Роли пользователя</h2>
                    <div class="center-horizonally">
                        <thead>
                            <tr>
                                <td>Название роли</td>
                                <td>Описание роли</td>
                                <td>Дата получения</td>
                                <td>Назначивший пользователь</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="role in userData.roles" :key="role.title">
                                <td>{{ role.title }}</td>
                                <td>{{ role.description }}</td>
                                <td>{{ role.date_assigned }}</td>
                                <td>
                                    <el-popover
                                        placement="top-start"
                                        title="Пользователь"
                                        width="200"
                                        trigger="hover">
                                        <div class="popover-content">{{ join(role.assigned_by_name, role.assigned_by_surname) }}</div>
                                        <el-button slot="reference">@{{ role.assigned_by_username }}</el-button>
                                    </el-popover>
                                </td>
                            </tr>
                        </tbody>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import store from "../store";
import axios from "axios";
import { api_url } from "../store";

export default {
    data() {
        return {
            userData: null,
            changingName: false,
            newName: '',
            newNameValid: false,
            changingSurname: false,
            newSurname: '',
            newSurnameValid: false,
            changingEmail: false,
            newEmail: '',
            newEmailValid: false,
        };
    },
    methods: {
        changeName: function() {
            this.changingName = false;
            this.userData.name = this.newName;
            //TODO: Send request
        },
        changeSurname: function() {
            this.changingSurname = false;
            this.userData.surname = this.newSurname;
            //TODO: Send request
        },
        changeEmail: function() {
            this.changingEmail = false;
            this.userData.email = this.newEmail;
            //TODO: Send request
        },
        nameChanged: function() {
            this.newNameValid = this.validateSimple(this.newName);
        },
        surnameChanged: function() {
            this.newSurnameValid = this.validateSimple(this.newSurname);
        },
        emailChanged: function() {
            this.newEmailValid = this.validateEmail(this.newEmail);
        },
        validateSimple: function(input) {
            return /\S/.test(input);
        },
        validateEmail: function(input) {
            return /\S+@\S+\.\S+/.test(String(input).toLowerCase());
        },
        loadUserProfile: function () {
            axios
                .post(api_url + "/user/get_info", {
                    access_token: localStorage.getItem("access_token"),
                })
                .then((response) => {
                    this.userData = response.data;

                    this.userData.roles.forEach(role => {
                        if (role.assigned_by_username == "SYSTEM")
                        {
                            role.assigned_by_username = "Система";
                        }
                        
                        if (role.assigned_by_name == "SYSTEM")
                        {
                            role.assigned_by_name = "Система";
                        }

                        if (role.assigned_by_surname == "SYSTEM")
                        {
                            role.assigned_by_surname = "Система";
                        }
                    });
                })
                .catch((error) => {
                    this.$notify.error({
                        title: "Не удалось получить информацию о пользователе",
                        message: error,
                    });
                });
        },
        getImgUrl(pic) {
            return require("../assets/" + pic);
        },
        join: function (first, second) {
            if (first == second) {
                return first;
            }
            else {
                return first + " " + second;
            }
        }
    },
    mounted() {
        store.commit("save_current_route", "/profile");
        this.loadUserProfile();
    },
};
</script>

<style lang="scss">
@import "../styles/variables.scss";

.profile-info {
    height: 700px;
}

.profile-container {
    margin: 0 auto 0 auto;
    text-align: left;
    max-height: 350px;
}

.profileTable {
    display: table;
    border-collapse: collapse;
    font-size: 0.9em;
    min-width: 400px;
    max-width: 700px;
    text-align: left;

    td {
        padding: 5px;
        border-bottom: 1px solid $border-color;
    }

    thead {
        td {
            border-bottom: none;
        }
    }

    .picture-td {
        border-bottom: 0;
    }
}

.roles-container {
    margin-top: 50px;
    display: inline-block;

    td {
        padding: 5px;
        border-bottom: 1px solid $border-color;
    }
}

.profile-picture {
    > span {
        width: 300px;
        height: 300px;
    }
}

.popover-content {
    text-align: center;
    margin: 10px 5px 5px 5px;
}

.edit-button-group {
    min-width: 150px;
}
</style>
