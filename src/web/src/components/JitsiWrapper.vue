<template>
    <div class="jitsi">
      <vue-jitsi-meet
        ref="jitsiRef"
        domain="meet.jit.si"
        :options="jitsiOptions"
      ></vue-jitsi-meet>
    </div>
</template>

<script>
import { JitsiMeet } from '@mycure/vue-jitsi-meet';

export default {
  props: {
    userName: String,
    roomId: String,
  },
  components: {
    VueJitsiMeet: JitsiMeet
  },
  computed: {
    jitsiOptions () {
      return {
        roomName: this.roomId,
        noSSL: false,
        userInfo: {
          email: 'user@email.com',
          displayName: this.userName,
        },
        configOverwrite: {
          enableNoisyMicDetection: false
        },
        interfaceConfigOverwrite: {
          SHOW_JITSI_WATERMARK: true,
          SHOW_WATERMARK_FOR_GUESTS: true,
          SHOW_CHROME_EXTENSION_BANNER: false
        },
      };
    },
  }
};
</script>

<style lang="scss">
.jitsi {
    height: 37rem;
    width: 76rem;
    margin: 0 auto;
}
</style>
