<template>
    <div class="playground">
      <div class="dev">
        <el-input placeholder="Please input" v-model="input"></el-input>
        <el-button type="primary" class="send_button" v-on:click="send_echo">Send Echo</el-button>
      </div>
    </div>
</template>

<script>
//import store from "../store"
const {EchoRequest} = require('./../gRPC/echo_pb.js');
const {EchoServiceClient} = require('./../gRPC/echo_grpc_web_pb.js');

export default {
    data() {
      return {
        input: 'Hello World!',
      };
    },
    created: function() {
      //this.echoService = new EchoServiceClient('http://192.168.40.43:7813');
      //this.echoService = new EchoServiceClient('http://192.168.40.76:10000');
      this.echoService = new EchoServiceClient('http://api.encamy.keenetic.pro');
    },
    methods: {
      send_echo: function() {
        var request = new EchoRequest();
        request.setMessage(this.input);

        console.log("sending message");
        this.echoService.echo(request, {}, this.on_result);
      },
      on_result: function(err, response) {
        if (err != null)
        {
          this.$notify.error({
            title: 'Error',
            message: err
          });
        }
        else if (response != null)
        {
          this.$notify({
            title: 'Success',
            message: response,
            type: 'success'
          });
        }
      }
    },
  };
</script>

<style lang="scss">
.dev {
  margin-top: 40px;
  max-width: 400px;
  margin: auto;
  margin-top: 20px;

  .send_button {
    margin-top: 20px;
  }
}
</style>
