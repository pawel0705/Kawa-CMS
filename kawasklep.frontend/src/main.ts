import { createApp } from "vue";
import { Vue } from "vue-class-component";
import App from "./App.vue";
import router from "./router";
import store from "./store";

const app = createApp(App);

app.config.globalProperties.$store = store;
app.use(store);
app.use(router);
app.mount("#app");
