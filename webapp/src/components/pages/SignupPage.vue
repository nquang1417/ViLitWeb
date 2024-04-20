<script lang="js">
import { mapActions, mapGetters } from 'vuex'

export default {
    name: 'SignupPage',
    data() {
        return {
            form: {
                username: '',
                displayName: '',
                password: '',
                email: '',
                confirmPass: '',
            },
            loginFailed: false,
            rules: {
                username: [
                    { required: true, message: 'Điền tên đăng nhập', trigger: 'blur' },
                    // { max: 150, message: 'Độ dài tối đa 200 ký tự', trigger: 'blur' }
                ],
                email: [
                    { required: true, message: 'Điền địa chỉ Email', trigger: 'blur' },
                    { required: true, message: 'Địa chỉ Email không đúng định dạng', pattern:'.+@gmail\.com', trigger: 'blur' },
                ],
                password: [
                    { required: true, message: 'Điền mật khẩu', trigger: 'blur' },
                ],
                confirmPass: [
                    { required: true, message: 'Mật khẩu không trùng khớp', trigger: 'blur' },
                ]
            },
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
            console.log(this.getterLoginStatus)
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
    <el-container class="signup-layout">
        <div class="background-img"></div>
        <el-header class="signup-header" :height="40">
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

                <el-menu-item index="/signup">Đăng nhập</el-menu-item>

            </el-menu>  
        </el-header>
        <el-main>
            <div class="signup-form">
                <div class="signup-form__header">Đăng ký</div>
                <div class="signup-form__body">
                    <el-form ref="ruleFormRef" :model="this.form" label-width="150px" :rules="this.rules" status-icon>
                        <!-- <el-form-item v-if="this.loginFailed">
                            <el-alert title="Đã có lỗi xảy ra!" type="error"
                                description="Tên đăng nhập hoặc mật khẩu không đúng" show-icon />
                        </el-form-item> -->
                        <el-form-item label="Username" label-position="right" prop="username">
                            <el-input v-model="form.username" autocomplete="off" />
                        </el-form-item>
                        <el-form-item label="Email" label-position="right" prop="email">
                            <el-input v-model="form.email" autocomplete="off" />
                        </el-form-item>
                        <el-form-item label="Password" label-position="right" prop="password">
                            <el-input type="password" show-password v-model="form.password" autocomplete="off" />
                        </el-form-item>
                        <el-form-item label="Confirm Password" label-position="right" prop="confirmPass">
                            <el-input type="password" show-password v-model="form.password" autocomplete="off" />
                        </el-form-item>
                    </el-form>
                </div>

                <div class="signup-form__footer">
                    <el-button @click="cancel">Hủy</el-button>
                    <el-button type="primary" @click="login">
                        Đăng ký
                    </el-button>
                </div>
            </div>
        </el-main>
        <!-- <the-footer></the-footer> -->
    </el-container>
</template>

<style scoped>

.signup-header .el-menu {
    height: 40px
}

.flex-grow {
    flex-grow: 1;
}

.signup-form {
    display: flex;
    flex-direction: column;
    width: 50%;
    position: absolute;
    top: 10%;
    left: 50%;
    transform: translate(-50%, 0);
    gap: 10px;
    margin: 15px;
    border: 1px solid #ccc;
    border-radius: 8px;
    /* padding: 20px; */
}

.signup-form__header {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: left;
    border-radius: 8px 8px 0 0;
    border-bottom: 1px solid #ccc;
}

.signup-form__body {
    padding: 10px 15px;
}

.signup-form__footer {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: right;
    border-radius: 0 0 8px 8px;
    border-top: 1px solid #ccc;
}
</style>