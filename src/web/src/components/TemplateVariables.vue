<template>
  <div class="variables">
    <el-table
      :data="tableData"
      style="width: 100%">
      <el-table-column
        prop="variableName"
        label="Переменная"
        width="180">
      </el-table-column>
      <el-table-column
        prop="variableExample"
        label="Пример подстановки"
        width="320">
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import axios from "axios";
import { api_url } from "../store";

export default {
  data() {
    return {
      variables: {},
      tableData: [{}],
    };
  },
  mounted() {
    this.loadVariables();
  },
  methods: {
      loadVariables() {
          let url = api_url + "/email/template/variables";
          axios
            .get(
                url,
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("access_token")}`,
                    },
                }
            )
            .then(response => this.processResponse(response.data));
      },
      processResponse(exampleDictionary) {
        this.tableData = [];
        this.$root.$emit(
            "template_variables_loaded",
            exampleDictionary
        );
        for (const [key, value] of Object.entries(exampleDictionary)) {
            this.tableData.push({
                variableName: key,
                variableExample: value
            })
        }
      }
  },
};
</script>

<style lang="scss">
@import "../styles/variables.scss";

.variables {
    width: 50%;
    margin: 0 auto;
}

</style>