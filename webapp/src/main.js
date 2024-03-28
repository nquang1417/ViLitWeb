import { createApp } from 'vue'
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
import '@vueup/vue-quill/dist/vue-quill.snow.css';


const app = createApp(App)
for (const [key, component] of Object.entries(ElementPlusIconsVue)) {
    app.component(key, component)
}
app.component('QuillEditor', QuillEditor)
app.use(router)
app.use(store)
app.use(ElementPlus)
app.use(AudioPlayer)
app.mount('#app')
