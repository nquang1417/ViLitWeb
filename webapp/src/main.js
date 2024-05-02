import * as Vue from 'vue'
import './style.css'
import App from './App.vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import "./assets/fontawesome/css/all.min.css"
import router from './scripts/router'
import store from './scripts/store'
import * as ElementPlusIconsVue from '@element-plus/icons-vue'
import AudioPlayer from '@liripeng/vue-audio-player'
// ES6
import { QuillEditor } from '@vueup/vue-quill'
import '@vueup/vue-quill/dist/vue-quill.snow.css'
import axios from 'axios'
import VueAxios from 'vue-axios'
import api from './scripts/api/api.js'

const app = Vue.createApp(App)
app.use(VueAxios, axios)
const factories = api(app.axios).getApiFactories()
app.provide('$api', factories)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}
app.component('QuillEditor', QuillEditor)

app.use(router)
app.use(store)
app.use(ElementPlus)
app.use(AudioPlayer)

app.mount('#app')
