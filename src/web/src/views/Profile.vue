<template>
    <div class="profile background-body">
        <h1>Профиль</h1>
        <i v-if="userData == null" class="el-icon-loading"></i>
        <div v-if="userData != null">
            <div class="profile-info">
                <div class="profile-container">
                    <table class="profileTable">
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
                                <td>
                                    {{ userData.name }}
                                </td>
                                <td>
                                    <el-button type="text">Изменить</el-button>
                                </td>
                            </tr>
                            <tr>
                                <td>Фамилия:</td>
                                <td>
                                    {{ userData.surname }}
                                </td>
                                <td>
                                    <el-button type="text">Изменить</el-button>
                                </td>
                            </tr>
                            <tr>
                                <td>Почта:</td>
                                <td>
                                    {{ userData.email }}
                                </td>
                                <td>
                                    <el-button type="text">Изменить</el-button>
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
                                <td>Логин назначившего пользователя</td>
                                <td>Имя назначившего пользователя</td>
                                <td>Фамилия назначившего пользователя</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="role in userData.roles" :key="role.title">
                                <td>{{ role.title }}</td>
                                <td>{{ role.description }}</td>
                                <td>{{ role.date_assigned }}</td>
                                <td>{{ role.assigned_by_username }}</td>
                                <td>{{ role.assigned_by_name }}</td>
                                <td>{{ role.assigned_by_surname }}</td>
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
        };
    },
    methods: {
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
    max-width: 50%;
    margin: 0 auto 0 auto;
    padding-right: 200px;
    text-align: left;
    max-height: 300px;
}

.profileTable {
    display: table;
    border-collapse: collapse;
    margin: 25px 0;
    font-size: 0.9em;
    min-width: 400px;
    max-width: 700px;
    text-align: left;

    td {
        padding: 5px;
        border-bottom: 1px solid $border-color;
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

.username {
    grid-area: username;
}

.name {
    grid-area: name;
}

.surname {
    grid-area: surname;
}
</style>
