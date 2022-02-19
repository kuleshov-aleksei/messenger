<template>
    <div class="incoming-messages-wrapper">
        <div>
            IM Server connection status:
        </div>
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
import { API_HOSTNAME } from "../store"

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
    computed: {
        connected: {
            get: function() {
                return store.state.ws_connected;
            },
            set: function(newValue) {
                store.commit('save_ws_state', newValue);
            }
        }
    },
    methods: {
        handleAuthorized: function()
        {
            if (!this.alreadySubscribed)
            {
                this.subscribeToEvents();
                this.alreadySubscribed = true;
            }
        },
        subscribeToEvents: function()
        {
            var accessToken = localStorage.getItem("access_token");
            var url = 'ws://' + API_HOSTNAME + ':5000/messenger/subscribe/ws?access_token=' + accessToken;
            console.log("Connecting to notification server");
            this.connection = new WebSocket(url)

            this.connection.onmessage = function(event)
            {
                console.log(event);
            }

            this.connection.onopen = this.wsConnected;
            this.connection.onclose = this.wsDisconnected;
        },
        wsConnected: function(event)
        {
            console.log(event)
            console.log("Successfully connected to the echo websocket server...")
            this.connected = true;
        },
        wsDisconnected: function()
        {
            console.log('Disconnected! Trying to connect again in 3 second');
            this.connected = false;
            setTimeout(() => {
                this.subscribeToEvents();
            }, 3000);
        }
  },
}
</script>

<style lang="scss">
@import "../styles/variables.scss";

.incoming-messages-wrapper{
    width: 230px;
    height: 40px;
    background: white;
    position: absolute;
    bottom: 0px;
    margin-bottom: 15px;
    margin-left: 10px;
    border-radius: 4px;
    padding: 10px;

    .not-connected {
        color: #F56C6C;
    }

    .connected {
        color: #67C23A;
    }
}

</style>