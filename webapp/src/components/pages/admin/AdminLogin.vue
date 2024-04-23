<script>
import { mapActions, mapGetters } from 'vuex'

export default {
    name: 'AdminLogin',
    data() {
        return {
            form: {
                username: '',
                password: '',
            },
            loginFailed: false,
            unauthorized: false,

        }
    },
    computed: {
        ...mapGetters('auth', {
            getterLoginStatus: 'getLoginStatus',
            gettersAuthData: 'getAuthData',            
        }),
    },
    watch: {
        getterLoginStatus(value) {
            console.log(value)
        }
    },
    methods: {
        ...mapActions('auth', {
            actionLogin: 'login',
            actionLogout: 'logout'            
        }),
        async login() {
            await this.actionLogin({ username: this.form.username, password: this.form.password });
            console.log(this.getterLoginStatus)
            if (this.getterLoginStatus && this.gettersAuthData.role == 'Admin') {
                // alert('login sucess');
                
                this.loginFailed = false
                this.$router.push('/admin')
            } else {
                // alert('failed to login')\
                // debugger
                if (this.getterLoginStatus && this.gettersAuthData.role != 'Admin') {
                    this.unauthorized = true
                    this.actionLogout()
                } else {
                    this.loginFailed = true                       
                } 
            }
        },
        cancel() {
            this.$router.push('/')
        },

    },
}
</script>

<template>
    <el-container class="login-layout">
        <div class="background-img"></div>
        <el-header class="login-header" :height="40">
            <!-- <img src="@/assets/logo/logo.png" :height="40" @click="this.$router.push('/')">
            <el-button>Đăng</el-button> -->
            <el-menu :default-active="this.$route.path"  
                class="el-menu-demo" mode="horizontal" 
                :ellipsis="false" 
                :router="true">
                <el-menu-item index="/" class="logo">
                    <img src="@/assets/logo/logo.png" :height="40">
                </el-menu-item>
                <div class="flex-grow"></div>


            </el-menu>  
        </el-header>
        <el-main>
            <el-form class="login-form" :model="form">
                <el-form-item v-if="this.loginFailed">
                    <el-alert title="Đã có lỗi xảy ra!" 
                        type="error" 
                        description="Tên đăng nhập hoặc mật khẩu không đúng"
                        show-icon />
                </el-form-item>
                <el-form-item v-if="this.unauthorized">
                    <el-alert title="Đã có lỗi xảy ra!" 
                        type="error" 
                        description="Bạn không có quyền truy cập"
                        show-icon />
                </el-form-item>
                <el-form-item label="Username">
                    <el-input v-model="form.username" autocomplete="off" />
                </el-form-item>
                <el-form-item label="Password" label-position="right">
                    <el-input type="password" show-password v-model="form.password" autocomplete="off" />
                </el-form-item>
                <el-form-item>
                    <div class="form-footer">
                        <el-button @click="cancel">Hủy</el-button>
                        <el-button type="primary" @click="login">
                            Đăng nhập
                        </el-button>
                    </div>
                </el-form-item>
            </el-form>
        </el-main>
        <!-- <the-footer></the-footer> -->
    </el-container>
</template>

<style scoped>
.login-header .el-menu {
    height: 40px
}

.flex-grow {
    flex-grow: 1;
}
.login-form {
    border: 1px solid #ccc;
    width: 30%;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    padding: 20px;
    background: aliceblue;
}
</style>