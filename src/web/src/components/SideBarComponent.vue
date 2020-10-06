<template>
    <el-menu
        default-active="search"
        class="el-menu-vertical-demo"
        @open="handleOpen"
        @close="handleClose"
        @select="handleSelect"
        :collapse="isCollapse"
    >
        <el-menu-item index="messenger">
            <i class="el-icon-chat-line-square"></i>
            <span slot="title">Мессенджер</span>
        </el-menu-item>
        <el-menu-item index="settings">
            <i class="el-icon-setting"></i>
            <span slot="title">Настройки</span>
        </el-menu-item>
        <el-menu-item index="collapse" class="bottom-element">
            <i v-bind:class="collapseIcon"></i>
            <span slot="title">Свернуть</span>
        </el-menu-item>
    </el-menu>
</template>

<script>
export default {
    data() {
        return {
            isCollapse: false,
            collapseIcon: "el-icon-s-fold",
        };
    },
    methods: {
        handleOpen(key, keyPath) {
            console.log(key, keyPath);
        },
        handleClose(key, keyPath) {
            console.log(key, keyPath);
        },
        handleSelect(key, keyPath) {
            if (keyPath == "collapse") {
                this.isCollapse = !this.isCollapse;
                if (this.isCollapse === true) {
                    this.collapseIcon = "el-icon-s-unfold";
                } else {
                    this.collapseIcon = "el-icon-s-fold";
                }
            } else {
                this.$root.$emit("sidebar-click", keyPath);
            }
        },
    },
};
</script>

<style>
.el-menu-vertical-demo {
    text-align: left;
    height: 93vh;
    position: relative;
}
.el-menu-vertical-demo:not(.el-menu--collapse) {
    width: 200px;
}
.bottom-element {
    position: absolute;
    bottom: 0;
    width: 100%;
}
span {
    font-family: Avenir, Helvetica, Arial, sans-serif;
}
</style>