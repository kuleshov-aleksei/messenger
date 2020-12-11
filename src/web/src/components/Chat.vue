<template>
  <div class="chat_component">
    <div class="chat_holder" v-chat-scroll="{always: false, smooth: false}" @v-chat-scroll-top-reached="topReached">
      <div class="" v-for="(message, i) in messages.slice().reverse()" :key="message.unixTime + i">
        <el-card shadow="never" class="box-card" v-bind:class="{message_other: message.isAuthor != true}">
          <div class="message">
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
        </el-card>
      </div>
    </div>
    <div class="input_area">
      <div class="input_flex">
        <el-button type="text" icon="el-icon-paperclip"></el-button>
        <div class="user-input el-textarea">
          <textarea
            placeholder="Напишите что-нибудь..."
            class="el-textarea__inner"
            :min-height="33"
            :max-height="350"
            @keydown.enter.exact.prevent
            @keyup.enter.exact="send_message()"
            @keydown.enter.shift.exact="newline"
            v-model="user_input">
          </textarea>
        </div>
        <el-button type="text" icon="el-icon-s-promotion" @click="send_message"></el-button>
      </div>
    </div>
  </div>
</template>

<script>
import store from "../store";
import { api_url } from "../store"
const {GetLastMessagesRequest, MessageRequest, SubscribeRequest, GetMessagesFromRequest} = require('./../gRPC/messenger_pb.js');
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
  mounted: function() {
    const tx = document.getElementsByClassName('el-textarea__inner');
    for (let i = 0; i < tx.length; i++) {
      tx[i].setAttribute('style', 'height:' + (tx[i].scrollHeight) + 'px;overflow-y:hidden;');
      tx[i].addEventListener("input", OnInput, false);
    }

    function OnInput() {
      this.style.height = 'auto';
      this.style.height = (this.scrollHeight) + 'px';
    }
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
    newline() {
      this.user_input = `${this.user_input}\n`;
    },
    send_message: function() {
      var request = new MessageRequest();
      request.setAccessToken(localStorage.getItem("access_token"));
      request.setMessage(this.user_input);
      request.setChatId(this.chat_id);

      console.log("sending message: " + this.user_input);
      this.messengerService.sendMessage(request, {}, this.onMessageSentResponse);
      this.user_input = '';
    },
    onMessageSentResponse: function(err, response)
    {
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
      }
    },
    on_chat_id_changed: function() {
      this.messages = [];
      this.load_last_messages();
      this.subscribe_to_messages();
    },
    load_last_messages: function() {
      var request = new GetLastMessagesRequest();
      request.setAccessToken(localStorage.getItem("access_token"));
      request.setChatId(this.chat_id);

      this.messengerService.getLastMessages(request, {}, (error, response) => {
        this.on_message_received(error, response, true)
      });
    },
    subscribe_to_messages: function() {
      console.log("subscribing to updates");
      var request = new SubscribeRequest();
      request.setAccessToken(localStorage.getItem("access_token"));
      request.setChatId(this.chat_id);

      this.messengerService.subscribeToMessages(request)
        .on("data", (response) => {
          this.on_message_received(null, response, false);
        })
        .on("error", (error) => {
          this.on_message_received(error, null);
        })
        .on("end", () => {
          console.log("end");
        });
    },
    on_message_received: function(err, response, initial) {
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
              message.isAuthor = protoMessage.getIsAuthor();

              if (initial == true)
              {
                this.messages.push(message);
              }
              else
              {
                this.messages.unshift(message);
              }
            });
          }
        }
        else if (response.hasEmpty())
        {
          // do nothing
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
    },
    topReached: function() {
      var request = new GetMessagesFromRequest();
      request.setAccessToken(localStorage.getItem("access_token"));
      request.setChatId(this.chat_id);
      let timestamps = this.messages.map(message => message.unixTime);
      request.setUnixTime(Math.min(...timestamps));

      this.messengerService.getMessagesFrom(request, {}, (error, response) => {
        this.on_message_received(error, response, true)
      });
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

@import "../styles/variables.scss";

.chat_component {
  display: table;
  width: 100%;
  table-layout: fixed;

  .chat_holder {
    max-height: 75vh;
    height: 75vh;
    width: 100%;
    position: relative;
    background: white;
    overflow-y: scroll;

    .message_self {
      margin-left: auto;
      margin-right: 0;
    }

    .box-card {
      max-width: 70%;
      margin: 5px 5px 2px auto;
    }

    .message_other {
      margin-left: 10px;
    }

    .message {
      display: grid;
      grid-template-columns: auto 1.3fr 0.8fr;
      grid-template-rows: 30px auto;
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
        white-space: pre-line;
      }

      .message_date {
        grid-area: message_date;
        text-align: right;
        margin-right: 10px;
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
      border: 1px solid $border-color;
      border-left: 0px;
    }

    button {
      font-size: 1.5em;
      margin: 0 20px 0 20px;
      color: rgb(84, 92, 100);
    }

    .user-input {
      height: 100%;
      padding: 20px 0 20px 0;
      overflow: hidden;

      .el-textarea__inner {
        line-height: 0.8;
      }
    }
  }

  .send_button {
    grid-column: 2;
    grid-row: 2;
  }
}
</style>
