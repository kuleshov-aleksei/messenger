<template>
  <div>
        <h1>Статус</h1>
        <ul v-if="services != null && services.length > 0">
            <li v-for="service in services" :key="service.name">
                <div class="service-holder">
                    <div class="title">
                        <h2>{{service.name}}</h2>
                        <h3>{{service.description}}</h3>
                    </div>
                    <div class="center">
                        <div v-if="service.status == null">
                            <div class="status">
                                <i class="el-icon-lightning offline"/><a>Оффлайн</a>
                            </div>
                        </div>
                        <div v-if="service.status != null">
                            <div class="status">
                                <i class="el-icon-sunny online"/><a>Онлайн</a>
                            </div>
                            <div class="statusTable">
                                <div class="statusTableRow">
                                    <div class="statusTableCell">Название</div>
                                    <div class="statusTableCell">{{service.name}} ({{service.description}})</div>
                                </div>
                                <div class="statusTableRow">
                                    <div class="statusTableCell">Название исполняемого файла</div>
                                    <div class="statusTableCell">{{service.status.name}} v{{service.status.version}}</div>
                                </div>
                                <div class="statusTableRow">
                                    <div class="statusTableCell">ОС</div>
                                    <div class="statusTableCell">{{service.status.machine_name}} ({{service.status.os_version}}) {{service.status.processor_count}} {{get_declination(service.status.processor_count)}}</div>
                                </div>
                                <div class="statusTableRow">
                                    <div class="statusTableCell">Использование памяти</div>
                                    <div class="statusTableCell">
                                        <div class="memoryUsage">
                                            <el-progress type="dashboard" :percentage="service.status.used_memory_percentage" :color="colors"></el-progress>
                                            Использовано {{service.status.used_memory}} из {{service.status.total_memory}}. Свободно {{service.status.free_memory}}
                                        </div>
                                    </div>
                                </div>
                                <div class="statusTableRow">
                                    <div class="statusTableCell">Время запуска</div>
                                    <div class="statusTableCell">{{service.status.start_time}} (работает {{service.status.working_time}})</div>
                                </div>
                                <div class="statusTableRow">
                                    <div class="statusTableCell">Данные обновлены в</div>
                                    <div class="statusTableCell">{{service.status.creation_date}}</div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <hr class="hr-chat-breaker dark-hr">
            </li>
        </ul>
  </div>
</template>

<script>
import axios from "axios";
import { api_url } from "../store"
import store from "../store";

var REFRESH_INTERVAL = 20 * 1000;

export default {
data() {
        return {
            services: [],
            serviceData: [],
            colors: [
                {color: '#4d70f0', percentage: 20},
                {color: '#4d70f0', percentage: 40},
                {color: '#5cb87a', percentage: 60},
                {color: '#ffb70f', percentage: 80},
                {color: '#ff7700', percentage: 100}
            ],
        };
    },
    mounted: function() {
        this.load_service_status();
        setInterval(this.load_service_status, REFRESH_INTERVAL);
    },
    methods: {
        load_service_status: function() {
            axios.post(api_url + "/orchestrator/service_status", {
                access_token: localStorage.getItem("access_token")
            })
            .then((response) => {
                this.services = response.data["services"];
            })
            .catch((error) => {
                console.log(error);
                if (error.response.status === 403 || error.response.status === 401)
                {
                    store.commit('save_current_route', '/auth');
                    this.$router.push('/auth');
                }
            });
        },
        get_declination: function(count) {
            var cases = [2, 0, 1, 1, 1, 2];
            var titles = ['ядро', 'ядра', 'ядер'];
            return titles[ (count % 100 > 4 && count % 100 < 20) ? 2 : cases[(count % 10 < 5) ? count % 10 : 5] ];
        },
    }
};
</script>

<style lang="scss">

@import "../styles/variables.scss";

.dark-hr {
    background-color: #383838;
}

.service-holder {
    padding-top: 20px;
    display: flex;

    .title {
        padding: 10px;
        min-width: 300px;
    }

    .status {
        a {
            font-size: 1.6em;
            margin-left: 5px;
        }
    }

    .center {
        margin: 0 auto 0 auto;
    }

    .online {
        font-size: 2em;
        color: rgb(30, 180, 105);
    }

    .offline {
        font-size: 2em;
        color: rgb(180, 110, 30);
    }
}

.statusTable{
    display: table;
    border-collapse: collapse;
    margin: 25px 0;
    font-size: 0.9em;
    font-family: sans-serif;
    min-width: 400px;
    max-width: 700px;
    text-align: left;

    .memoryUsage {
        display: flex;
        align-items: center;
        justify-content: center;

        .el-progress-circle {
            margin: 10px;
        }
    }
}

.statusTableRow {
    display: table-row;
}

.statusTableRow:nth-of-type(odd) {
    display: table-row;
    background-color: #f3f3f3;
}

.statusTableCell, .statusTableHead {
    border-bottom: 1px solid $border-color;
    display: table-cell;
    padding: 12px 15px;
    vertical-align: middle;
}

</style>