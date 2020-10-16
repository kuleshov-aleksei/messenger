<template>
    <div id="header-wrapper">
        <el-menu
            class="el-menu"
            mode="horizontal"
            background-color="#545c64"
            text-color="#fff"
            active-text-color="#ffd04b"
        >
            <router-link to="/">
                <el-menu-item index="messenger" class="el-menu-item">
                    <i class="el-icon-chat-line-square" />Общение
                </el-menu-item>
            </router-link>
            <router-link to="/profile">
                <div class="first-right-el">
                    <el-menu-item index="profile" class="el-menu-item">
                        <i class="el-icon-user-solid" />Профиль
                    </el-menu-item>
                </div>
            </router-link>
            <router-link to="/settings">
                <el-menu-item index="settings" class="el-menu-item">
                    <i class="el-icon-setting" />Настройки
                </el-menu-item>
            </router-link>
        </el-menu>
    </div>
</template>

<script>
export default {};
</script>

<style>
.el-header {
    padding: 0 !important;
}

.el-menu {
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
    list-style: none;
    margin: 0;
    padding: 0;
    border-width: 0px !important;
}

.first-right-el {
    margin-left: 40vw;
}

.el-menu-item {
    margin: 0 20px;
}

#header-wrapper > ul > a {
    color: #ffffff;
    text-decoration: none;
}
</style>