<template>
  <div class="chat_component">
    <!--<h1>DEBUG: selected {{chat_id}}</h1>-->
    <div class="chat_holder">
      <div class="message" v-for="message in messages.slice().reverse()" :key="message.unixTime">
        <div class="user_photo">
          <el-avatar size="large" :src="getImgUrl(message.authorImage)">
            <img src="../assets/notfound.png"/>
          </el-avatar>
        </div>
        <div class="user_name">
          <b>{{message.authorName}} {{message.authorSurname}}</b>
        </div>
        <div class="message_text">
          {{message.text}}
        </div>
        <div class="message_date">
          {{unixTimeToHumanReadable(message.unixTime)}}
        </div>
      </div>
    </div>
    <div class="input_area">
      <div class="input_flex">
        <el-button type="text" icon="el-icon-paperclip"></el-button>
        <el-input
          type="textarea"
          autosize
          placeholder="Напишите что-нибудь..."
          v-model="user_input"
          class="user-input">
        </el-input>
        <el-button type="text" icon="el-icon-s-promotion"></el-button>
      </div>
    </div>
  </div>
</template>

<script>
import store from "../store";
import { api_url } from "../store"
const {GetLastMessagesRequest} = require('./../gRPC/messenger_pb.js');
const {MessengerServiceClient} = require('./../gRPC/messenger_grpc_web_pb.js');

const monthNames = ["января", "февраля", "марта", "апреля", "мая", "июня",
  "июля", "августа", "сентября", "октября", "ноября", "декабря"
];

export default {
  data() {
    return {
      user_input: '',
      messages: [

      ],
    };
  },
  created: function() {
    this.messengerService = new MessengerServiceClient(api_url);
    this.chat_id = store.state.selected_chat_id;
    this.on_chat_id_changed();
  },
  computed: {
    chat_id: {
      get: function() {
        return store.state.selected_chat_id;
      },
      set: function(newValue) {
        store.commit('set_chat_id', newValue);
      }
    }
  },
  methods: {
    on_chat_id_changed: function() {
      console.log("chat id is changed");
      this.messages = [];
      this.load_last_messages();
    },
    load_last_messages: function() {
        var request = new GetLastMessagesRequest();
        request.setAccessToken(localStorage.getItem("access_token"));
        request.setChatId(this.chat_id);

        console.log("sending message");
        this.messengerService.getLastMessages(request, {}, this.on_message_received);
    },
    on_message_received: function(err, response) {
      if (err != null)
      {
        console.log(err);
        this.$notify.error({
            title: 'Error',
            message: err
          });
      }
      else if (response != null)
      {
        if (response.hasErrorMessage())
        {
          console.log(response.getErrorMessage());
          this.$notify.error({
            title: 'Error',
            message: response.getErrorMessage()
          });
        }
        else if (response.hasMessageList())
        {
          if (response.getMessageList().getChatId() != this.chat_id)
          {
            console.log("Response was received, but chat have changed. Ignoring");
          }
          else
          {
            response.getMessageList().getMessagesList().forEach(protoMessage => {
              var message = new Object();
              message.unixTime = protoMessage.getUnixTime();
              message.authorName = protoMessage.getAuthorName();
              message.authorSurname = protoMessage.getAuthorSurname();
              message.authorImage = protoMessage.getAuthorImage();
              message.text = protoMessage.getText();

              this.messages.push(message);
            });
          }
        }
        else
        {
          console.log("Unexpected response");
          this.$notify.error({
            title: 'Error',
            message: "Unexpected response"
          });
        }
      }
    },
    on_result: function(err, response) {
      if (err != null)
      {
        console.log(err);
      }
      else if (response != null)
      {
        console.log(response);
      }
    },
    getImgUrl(pic) {
        return require('../assets/'+pic)
    },
    unixTimeToHumanReadable(unixTime) {
      var date = new Date(unixTime * 1000);
      var hours = date.getHours();
      var minutes = "0" + date.getMinutes();
      
      var today = new Date().setHours(0,0,0,0);

      if (date < today)
      {
        var day = "0" + date.getDate();
        var currentYear = new Date(new Date().getFullYear(), 1, 1);

        if (date < currentYear)
        {
          var year = date.getFullYear();
          return hours + ':' + minutes.substr(-2) + ' ' + day.substr(-2) + ' ' + monthNames[date.getMonth()] + ' ' + year;
        }
        
        return hours + ':' + minutes.substr(-2) + ' ' + day.substr(-2) + ' ' + monthNames[date.getMonth()];
      }

      return hours + ':' + minutes.substr(-2);
    }
  },
  watch: {
    'chat_id': function() {
      this.on_chat_id_changed();
    }
  }
}
</script>

<style lang="scss">

.chat_component {
  display: table;
  width: 100%;
  table-layout: fixed;
  height: 100%;

  .chat_holder {
    display: table-row;
    height: 100%;
    width: 100%;
    position: relative;
    background: white;
    max-height: 100%;

    .message {
      display: grid;
      grid-template-columns: auto 1.3fr 1fr;
      grid-template-rows: 0.6fr 1.4fr;
      gap: 0px 0px;
      grid-template-areas:
      "user_photo user_name message_date"
      "user_photo message_text message_text";

      .user_photo {
        grid-area: user_photo;
        padding: 5px;
      }

      .user_name {
        grid-area: user_name;
        padding: 5px;
        text-align: left;
      }

      .message_text {
        grid-area: message_text;
        padding: 5px;
        text-align: left;
      }

      .message_date {
        grid-area: message_date;
        text-align: right;
      }
    }
  }

  .input_area {
    display: table-row;
    width: 100%;
    background: rgb(250, 250, 250);

    .input_flex {
      display: flex;
      flex-direction: row;
      border: 1px solid rgb(190,190,190);
    }

    button {
      font-size: 1.5em;
      margin: 0 20px 0 20px;
      color: rgb(84, 92, 100);
    }

    .user-input {
      height: 100%;
      padding: 20px 0 20px 0;
    }
  }

  .send_button {
    grid-column: 2;
    grid-row: 2;
  }
}
</style>
