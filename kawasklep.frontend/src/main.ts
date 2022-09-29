import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import VueApexCharts from "vue3-apexcharts";

const app = createApp(App);

app.use(VueApexCharts);

app.config.globalProperties.$store = store;
app.use(store);
app.use(router);
app.mount("#app");
