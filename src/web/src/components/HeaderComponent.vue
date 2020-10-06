<template>
    <div id="header-wrapper">
        <el-menu
            :default-active="activeIndex"
            class="el-menu-demo"
            mode="horizontal"
            @select="handleSelect"
            background-color="#545c64"
            text-color="#fff"
            active-text-color="#ffd04b"
        >
            <el-menu-item index="1">Placeholder</el-menu-item>
            <el-menu-item index="2">Placeholder</el-menu-item>
            <el-menu-item index="3">Placeholder</el-menu-item>
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
</style>