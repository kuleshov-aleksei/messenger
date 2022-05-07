<template>
  <div>
    <TemplateEditor/>
    <TemplateVariables/>
  </div>
</template>

<script>
import TemplateEditor from "../components/TemplateEditor.vue";
import TemplateVariables from "../components/TemplateVariables.vue";

export default {
  components: {
    TemplateEditor,
    TemplateVariables,
  },
}
</script>
