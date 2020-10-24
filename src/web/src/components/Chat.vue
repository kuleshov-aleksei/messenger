<template>
  <div class="chat_component">
    <h1>DEBUG: selected {{chat_id}}</h1>
  </div>
</template>

<script>
import store from "../store";

export default {
  data() {
    return {
    };
  },
  created: function() {
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
    }
  },
  watch: {
    'chat_id': function() {
      this.on_chat_id_changed();
    }
  }
}
</script>

<style>

</style>