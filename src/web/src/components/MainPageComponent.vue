<template>
    <el-container>
        <el-header>
            <HeaderComponent />
        </el-header>
        <el-container>
            <el-main>
                <component v-bind:is="componentName"></component>
            </el-main>
        </el-container>
    </el-container>
</template>

<script>
import HeaderComponent from "./HeaderComponent.vue";
import MessengerComponent from "./MessengerComponent.vue";
import SettingsComponent from "./SettingsComponent.vue";
import ProfileComponent from "./ProfileComponent.vue";
import UndefinedComponent from "./UndefinedComponent.vue";

export default {
    name: "MainPageComponent",
    components: {
        HeaderComponent,
        MessengerComponent,
        SettingsComponent,
        UndefinedComponent,
    },
    data() {
        return {
            componentName: MessengerComponent,
            asideWidth: "201px",
        };
    },
    mounted: function () {
        this.$root.$on("header-click", (text) => {
            switch (text.join()) {
                case "messenger":
                    this.componentName = MessengerComponent;
                    break;
                case "settings":
                    this.componentName = SettingsComponent;
                    break;
                case "profile":
                    this.componentName = ProfileComponent;
                    break;
                case "collapse":
                    break;
                default:
                    this.componentName = UndefinedComponent;
            }
        });
    },
};
</script>

<style>
</style>
