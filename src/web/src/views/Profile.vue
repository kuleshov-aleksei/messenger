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
                                <td rowspan="5" class="picture-td">
                                    <div class="profile-picture">
                                        <el-avatar size="large" :src="getImgUrl(userData.image_large)">
                                            <!--<img src="../assets/notfound.png" />-->
                                            <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQlP4HLuTjGwgPOJO2j85GEOgq__zHHJttpwA&usqp=CAU" />
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
                            <tr>
                                <td>Пароль:</td>
                                <td>
                                    ***********
                                </td>
                                <td>
                                    <el-button type="text" @click="changingPassword = true">Изменить</el-button>
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

        <el-dialog
            title="Изменение пароля"
            :visible.sync="changingPassword"
            width="30%">
            <table class="center-horizonally password-table">
                <tbody>
                    <tr>
                        <td>Текущий пароль</td>
                        <td><el-input placeholder="Введите текущий пароль" show-password v-model="passwordData.currentPassword" @input="validatePasswordForm"></el-input></td>
                    </tr>
                    <tr>
                        <td>Новый пароль</td>
                        <td><el-input placeholder="Введите новый пароль" show-password v-model="passwordData.newPassword" @input="validatePasswordForm"></el-input></td>
                    </tr>
                    <tr>
                        <td>Повторите новый пароль</td>
                        <td><el-input placeholder="Повторите новый пароль" show-password v-model="passwordData.repeatNewPassword" @input="validatePasswordForm"></el-input></td>
                    </tr>
                </tbody>
            </table>
            <span slot="footer" class="dialog-footer">
                <el-button @click="changingPassword = false">Отмена</el-button>
                <el-button type="primary" @click="changePassword" :disabled="!passwordData.validForm">Подтвердить</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
import store from "../store";
import axios from "axios";
import { api_url } from "../store";
import crypto from "crypto";

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
            changingPassword: false,
            passwordData: {
                currentPassword: '',
                newPassword: '',
                repeatNewPassword: '',
                validForm: false
            }
        };
    },
    methods: {
        changeName: function()
        {
            this.changingName = false;
            this.userData.name = this.newName;

            axios.post(api_url + "/user/change_name",
                {
                    access_token: localStorage.getItem("access_token"),
                    new_name: this.newName
                })
                .then(() =>
                {
                    this.showSuccess("Изменение данных", "Имя пользователя успешно изменено!");
                })
                .catch(() =>
                {
                    this.showError("Изменение данных", "Не удалось изменить имя пользоваля");
                });
        },
        changeSurname: function()
        {
            this.changingSurname = false;
            this.userData.surname = this.newSurname;
            
            axios.post(api_url + "/user/change_surname",
                {
                    access_token: localStorage.getItem("access_token"),
                    new_surname: this.newSurname
                })
                .then(() =>
                {
                    this.showSuccess("Изменение данных", "Фамилия пользователя успешно изменено!");
                })
                .catch(() =>
                {
                    this.showError("Изменение данных", "Не удалось изменить фамилию пользоваля");
                });
        },
        changeEmail: function()
        {
            this.changingEmail = false;
            this.userData.email = this.newEmail;
            
            axios.post(api_url + "/user/change_email",
                {
                    access_token: localStorage.getItem("access_token"),
                    new_email: this.newEmail
                })
                .then(() =>
                {
                    this.showSuccess("Изменение данных", "Почта пользователя успешно изменена!");
                })
                .catch(() =>
                {
                    this.showError("Изменение данных", "Не удалось изменить почту пользоваля");
                });
        },
        changePassword: function()
        {
            let hash = crypto.createHash('sha').update(this.passwordData.newPassword).digest('hex');

            axios.post(api_url + "/user/change_password",
                {
                    access_token: localStorage.getItem("access_token"),
                    current_password: this.passwordData.currentPassword,
                    new_password: hash
                })
                .then(() =>
                {
                    this.showSuccess("Изменение данных", "Пароль успешно изменен!");
                })
                .catch((error) =>
                {
                    if (error.response == null)
                    {
                        this.showError("Изменение данных", "Не удалось изменить пароль");
                        return;
                    }

                    let notificationMessage = error.response.data != null ? error.response.data : error;
                    this.showError("Изменение данных", notificationMessage.error_message);
                });

            this.changingPassword = false;

            this.passwordData.currentPassword = '';
            this.passwordData.newPassword = '';
            this.passwordData.repeatNewPassword = '';
            this.passwordData.validForm = false;
        },
        nameChanged: function()
        {
            this.newNameValid = this.validateSimple(this.newName);
        },
        surnameChanged: function()
        {
            this.newSurnameValid = this.validateSimple(this.newSurname);
        },
        emailChanged: function()
        {
            this.newEmailValid = this.validateEmail(this.newEmail);
        },
        validateSimple: function(input)
        {
            return /\S/.test(input);
        },
        validateEmail: function(input)
        {
            return /\S+@\S+\.\S+/.test(String(input).toLowerCase());
        },
        validatePasswordForm: function()
        {
            if (this.validateSimple(this.passwordData.currentPassword) && 
                this.validateSimple(this.passwordData.newPassword) && 
                this.validateSimple(this.passwordData.repeatNewPassword) &&
                this.passwordData.newPassword === this.passwordData.repeatNewPassword)
            {
                this.passwordData.validForm = true;
            }
            else
            {
                this.passwordData.validForm = false;
            }
        },
        loadUserProfile: function ()
        {
            axios.post(api_url + "/user/get_info",
                {
                    access_token: localStorage.getItem("access_token"),
                })
                .then((response) =>
                {
                    this.userData = response.data;

                    this.userData.roles.forEach(role =>
                    {
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
                .catch((error) =>
                {
                    this.showError("Не удалось получить информацию о пользователе", error);
                    if (error.response.status === 403 || error.response.status === 401)
                    {
                        store.commit('save_current_route', '/auth');
                        this.$router.push('/auth');
                    }
                });
        },
        getImgUrl(pic)
        {
            if (pic === null || pic === '') {
                return null;
            }
            else {
                return require("../assets/" + pic);
            }
        },
        join: function (first, second)
        {
            if (first == second)
            {
                return first;
            }
            else
            {
                return first + " " + second;
            }
        },
        showSuccess: function(title, message)
        {
            this.$notify(
            {
                title: title,
                message: message,
                type: 'success'
            });
        },
        showError: function(title, message)
        {
            this.$notify.error(
            {
                title: title,
                message: message,
            });
        }
    },
    mounted()
    {
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

.password-table {
    text-align: left;

    td {
        padding: 5px;
    }
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
