<template>
  <div class="template-editor">
    <div class="template-selector">
      <el-dropdown @command="handleCommand">
        <el-button type="primary">
            Выберите шаблон<i class="el-icon-arrow-down el-icon--right"></i>
        </el-button>
        <el-dropdown-menu slot="dropdown">
            <el-dropdown-item command="switchToRegister">Шаблон сообщения о регистрации</el-dropdown-item>
            <el-dropdown-item command="switchToPasswordReset">Шаблон восстановления пароля</el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>
    </div>
    <el-row class="block-col-2">
        <el-col :span="12">
            <div class="editor">
                <h2>Редактор</h2>
                <el-input
                    type="textarea"
                    :autosize="{ minRows: 4, maxRows: 10}"
                    maxlength="512"
                    show-word-limit
                    placeholder="Выберите шаблон из меню выше"
                    v-model="currentTemplate">
                </el-input>
            </div>
        </el-col>
        <el-col :span="12">
            <div class="preview">
                <h2>Предпросмотр</h2>
                <el-input
                    type="textarea"
                    placeholder="Выберите шаблон из меню выше"
                    :autosize="{ minRows: 4, maxRows: 10}"
                    v-model="templatePreview"
                    :disabled="true">
                </el-input>
            </div>
        </el-col>
    </el-row>
    <el-button type="primary" v-on:click="saveTemplate">
        Сохранить шаблон
    </el-button>
  </div>
</template>

<script>
import axios from "axios";
import { api_url } from "../store";

export default {
  data() {
    return {
      currentTemplate: "Выберите шаблон из меню выше",
      templateVariables: {},
      currentTemplateName: "",
    };
  },
  computed: {
      templatePreview() {
          if (this.templateVariables === undefined || Object.keys(this.templateVariables).length == 0) {
              return this.currentTemplate;
          }

          //var resultPreview = [];
          //var words = this.currentTemplate.split(/[\s,]+/);
          //for (var i = 0; i < words.length; i++) {
          //  var word = words[i];
          //  if (word in this.templateVariables) {
          //      var replacement = this.templateVariables[word];
          //      resultPreview.push(replacement);
          //  } else {
          //      resultPreview.push(word);
          //  }
          //}
//
          //return resultPreview.join(" ");

          var resultPreview = this.currentTemplate;
          for (const [key, value] of Object.entries(this.templateVariables)) {
            resultPreview = resultPreview.replaceAll(key, value);
          }

          return resultPreview;
      }
  },
  mounted() {
    this.$root.$on("template_variables_loaded", this.templateVariablesLoaded);
  },
  methods: {
      handleCommand(command) {
        switch (command) {
            case 'switchToRegister':
                this.currentTemplateName = "register";
                this.loadTemplate(this.currentTemplateName);
                break;
            case 'switchToPasswordReset':
                this.currentTemplateName = "password_reset";
                this.loadTemplate(this.currentTemplateName);
                break;
            default:
                console.log("unknown command");
                break;
        }
      },
      loadTemplate(templateName) {
          let url = api_url + "/email/template/" + templateName;
          axios
            .get(
                url,
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("access_token")}`,
                    },
                }
            )
            .then(response => (this.currentTemplate = response.data.template));
      },
      templateVariablesLoaded(variables) {
          this.templateVariables = variables;
      },
      saveTemplate() {
          axios
            .post(
              api_url + "/email/template/" + this.currentTemplateName,
              {
                template: this.currentTemplate,
              },
              {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("access_token")}`,
                },
              }
            )
            .then(
                this.$notify({
                    title: "Шаблон изменен",
                    dangerouslyUseHTMLString: false,
                    message: "Шаблон " + this.currentTemplateName + " успешно изменен",
                    type: "success",
                })
            )
      }
  },
};
</script>

<style lang="scss">
@import "../styles/variables.scss";

  .el-dropdown-link {
    cursor: pointer;
    color: #409EFF;
  }
  .el-icon-arrow-down {
    font-size: 12px;
  }

  .preview, .editor {
      padding: 0 20px 10px 20px;
  }

</style>