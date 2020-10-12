<template>
    <div id="header-wrapper">
        <el-menu
            :default-active="activeIndex"
            class="el-menu"
            mode="horizontal"
            @select="handleSelect"
            background-color="#545c64"
            text-color="#fff"
            active-text-color="#ffd04b"
        >
            <a href="/messenger" target="_self"><el-menu-item class="el-menu-item" index="1"><i class="el-icon-chat-line-square"/>Общение</el-menu-item></a>
            <a href="/profile" target="_self" class="first-right-el"><el-menu-item class="el-menu-item" index="2"><i class="el-icon-user-solid"/>Профиль</el-menu-item></a>
            <a href="/settings" target="_self"><el-menu-item class="el-menu-item" index="3"><i class="el-icon-setting"/>Настройки</el-menu-item></a>
        </el-menu>
    </div>
</template>

<script>
export default {
    data() {
        return {
            activeIndex: "1",
        };
    },
    methods: {
        handleSelect(key, keyPath) {
            console.log(key, keyPath);
        },
    },
};
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
}

.first-right-el {
    margin-left: 40vw;
}

.el-menu-item {
  margin: 0 20px;
}

#header-wrapper > ul > a{
    color: #FFFFFF;
    text-decoration: none;
}

</style>