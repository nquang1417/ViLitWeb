<script lang="js">
import { mapActions, mapGetters } from 'vuex'

export default {
    name: 'LoginPage',
    data() {
        return {
            form: {
                username: '',
                password: '',
            },
            loginFailed: false,
        }
    },
    computed: {
        ...mapGetters('auth', {
            getterLoginStatus: 'getLoginStatus',
            gettersAuthData: 'getAuthData'
        })
    },
    watch: {
        getterLoginStatus(value) {
            console.log(value)
        }
    },
    methods: {
        ...mapActions('auth', {
            actionLogin: 'login'
        }),
        async login() {
            await this.actionLogin({ username: this.form.username, password: this.form.password });
            if (this.getterLoginStatus == true) {
                // alert('login sucess');
                this.loginFailed = false
                this.$router.push('/dashboard')
            } else {
                // alert('failed to login')
                this.loginFailed = true
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
            <el-menu :default-active="this.$route.path" class="el-menu-demo" mode="horizontal" :ellipsis="false"
                :router="true">
                <el-menu-item index="/" class="logo">
                    <img src="@/assets/logo/logo.png" :height="40">
                </el-menu-item>
                <div class="flex-grow"></div>

                <el-menu-item index="/signup">Đăng ký</el-menu-item>

            </el-menu>
        </el-header>
        <el-main>
            <div class="login-form">
                <div class="login-form__header">Đăng nhập</div>
                <div class="login-form__body">
                    <el-form :model="form">
                        <el-form-item v-if="this.loginFailed">
                            <el-alert title="Đã có lỗi xảy ra!" type="error"
                                description="Tên đăng nhập hoặc mật khẩu không đúng" show-icon />
                        </el-form-item>
                        <el-form-item label="Username">
                            <el-input v-model="form.username" autocomplete="off" />
                        </el-form-item>
                        <el-form-item label="Password" label-position="right">
                            <el-input type="password" show-password v-model="form.password" autocomplete="off" />
                        </el-form-item>
                    </el-form>
                </div>

                <div class="login-form__footer">
                    <el-button @click="cancel">Hủy</el-button>
                    <el-button type="primary" @click="login">
                        Đăng nhập
                    </el-button>
                </div>
            </div>
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
    display: flex;
    flex-direction: column;
    width: 30%;
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    gap: 10px;
    margin: 15px;
    border: 1px solid #ccc;
    border-radius: 8px;
    /* padding: 20px; */
}

.login-form__header {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: left;
    border-radius: 8px 8px 0 0;
    border-bottom: 1px solid #ccc;
}

.login-form__body {
    padding: 10px 15px;
}

.login-form__footer {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: right;
    border-radius: 0 0 8px 8px;
    border-top: 1px solid #ccc;
}
</style>