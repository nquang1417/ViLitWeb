import { createStore } from "vuex"
import { comparePassword } from "./utils/passwordUtils"
import authModule from './modules/auth'
import novelModule from './modules/novel'

const store = createStore({
    modules: {
        auth: authModule,
        novel: novelModule,        
    },
})

export default store