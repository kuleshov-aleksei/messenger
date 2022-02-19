<template>
    <div>
    </div>
</template>

<script>
//import store from "../store";
//import sessionStore from "../sessionStore";
//import jwt_decode from "jwt-decode";

export default {
  data() {
    return {
        alreadySubscribed: false,
    };
  },
  mounted: function() {
    this.$root.$on('token_valid', this.handleAuthorized);
    this.$root.$on('authorized', this.handleAuthorized);
  },
  methods: {
      handleAuthorized: function() {
          if (!this.alreadySubscribed)
          {
            this.subscribeToEvents();
            this.alreadySubscribed = true;
          }
      },
      subscribeToEvents: function() {
        //if (localStorage.getItem("refresh_token") === null)
        console.log("wanted to subscribe to events");
      }
  },
}
</script>

<style lang="scss">
@import "../styles/variables.scss";

</style>