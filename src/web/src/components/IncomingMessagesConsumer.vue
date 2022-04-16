<template>
  <div class="incoming-messages-wrapper">
    <div>IM Server connection status:</div>
    <div>
      <div v-if="connected === false" class="not-connected">
        <font-awesome-icon icon="fa-solid fa-circle-xmark" /> Disconnected
      </div>
      <div v-if="connected === true" class="connected">
        <font-awesome-icon icon="fa-solid fa-bolt" /> Connected
      </div>
    </div>
  </div>
</template>

<script>
import store from "../store";
//import sessionStore from "../sessionStore";
//import jwt_decode from "jwt-decode";
import { API_HOSTNAME } from "../store";

export default {
  data() {
    return {
      alreadySubscribed: false,
      stopConnecting: false,
    };
  },
  mounted: function () {
    this.$root.$on("token_valid", this.handleAuthorized);
    this.$root.$on("authorized", this.handleAuthorized);
  },
  computed: {
    connected: {
      get: function () {
        return store.state.ws_connected;
      },
      set: function (newValue) {
        store.commit("save_ws_state", newValue);
      },
    },
  },
  methods: {
    handleAuthorized: function () {
      this.subscribeToEvents();
    },
    subscribeToEvents: function () {
      if (this.alreadySubscribed) {
        return;
      }
      if (this.stopConnecting) {
        console.log("There will be no attempts to connect due to error code");
        return;
      }

      var accessToken = localStorage.getItem("access_token");
      //var url = 'ws://' + API_HOSTNAME + ':5000/messenger/subscribe/ws?access_token=' + accessToken;
      var url =
        "wss://" +
        API_HOSTNAME +
        "/messenger/subscribe/ws?access_token=" +
        accessToken;
      console.log("Connecting to notification server");
      this.connection = new WebSocket(url);
      this.connection.onmessage = this.wsMessageReceived;
      this.connection.onopen = this.wsConnected;
      this.connection.onclose = this.wsDisconnected;
    },
    handleError: function (errorCode, errorMessage) {
      console.log("Received error " + errorCode + ": " + errorMessage);
      if (errorCode === 1) {
        this.stopConnecting = true;
      }
    },
    wsMessageReceived: function (event) {
      console.log("WS Server: " + event.data);
      var jsObject = JSON.parse(event.data);
      if (jsObject.error_code > 0) {
        this.handleError(jsObject.error_code, jsObject.error_message);
      } else {
        if (jsObject.incoming_messages != null) {
          this.$root.$emit(
            "incoming_message",
            jsObject.incoming_messages.message
          );
        }
      }
    },
    wsConnected: function (event) {
      console.log(event);
      console.log("Successfully connected to the echo websocket server...");
      this.connected = true;
      this.alreadySubscribed = true;
    },
    wsDisconnected: function () {
      this.alreadySubscribed = false;
      this.connected = false;
      console.log("Disconnected! Trying to connect again in 3 second");
      setTimeout(() => {
        this.subscribeToEvents();
      }, 3000);
    },
  },
};
</script>

<style lang="scss">
@import "../styles/variables.scss";

.incoming-messages-wrapper {
  width: 230px;
  height: 40px;
  background: white;
  position: fixed;
  bottom: 0px;
  margin-bottom: 15px;
  margin-left: 10px;
  border-radius: 4px;
  padding: 10px;

  .not-connected {
    color: #f56c6c;
  }

  .connected {
    color: #67c23a;
  }
}
</style>