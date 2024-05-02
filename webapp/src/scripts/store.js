import { createStore } from "vuex"
import authModule from './modules/auth'
import novelModule from './modules/novel'
import { inject } from "vue"

const store = createStore({
    modules: {
        auth: authModule,
        novel: novelModule,       
    },
})

export default store