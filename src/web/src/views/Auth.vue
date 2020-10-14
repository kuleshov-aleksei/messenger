<template>
    <el-container>
        <el-header>
            <HeaderComponent />
        </el-header>
        <el-container>
            <div>
                <h1>Test Auth</h1>
            </div>
        </el-container>
    </el-container>
</template>

<script>
import HeaderComponent from "../../components/HeaderComponent.vue";

export default {
    name: "MainPageComponent",
    components: {
        HeaderComponent,
    }
};
</script>

<style>
</style>
