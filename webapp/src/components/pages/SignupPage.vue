<script lang="js">
import { ElMessage } from 'element-plus';
import { inject } from 'vue';
import { mapActions, mapGetters } from 'vuex'

export default {
    name: 'SignupPage',
    data() {
        const validateConfirmPass = (rule, value, callback) => {
            if (value !== this.form.password) {
                callback(new Error('Mật khẩu không trùng khớp'));
            } else {
                callback()
            }
        }

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
                    { required: true, message: 'Xác nhận lại mật khẩu', trigger: 'blur' },
                    { validator: validateConfirmPass, message: 'Mật khẩu không trùng khớp', trigger: 'blur' }
                ]
            },
            showHeader: true,
        }
    },
    computed: {
        ...mapGetters('auth', {
            getterLoginStatus: 'getLoginStatus',
            gettersAuthData: 'getAuthData'
        })
    },
    mounted() {
        this.lastScrollPosition = window.scrollY
        window.addEventListener('scroll', this.onScroll)
        this.$api = inject('$api')
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
        onScroll() {
            if (window.scrollY < 0) {
                return
            }
            if (Math.abs(window.scrollY - this.lastScrollPosition) < this.scrollOffset) {
                return
            }
            this.showHeader = window.scrollY < this.lastScrollPosition
            this.lastScrollPosition = window.scrollY
        },
        async login(username, password) {
            await this.actionLogin({ username: username, password: password });
            
            if (this.getterLoginStatus == true) {                
                this.$router.push('/dashboard')
            } else {
            }
        },
        async submitForm(formEl) {
            if (!formEl) return
            await formEl.validate((valid,fields) => {
                if (valid) {
                    ElMessage({message: 'Bạn đã đăng ký thành công', type:'success', duration: 1500})
                    this.register()
                } else {
                    ElMessage({
                        message: 'Dữ liệu đầu vào không hợp lệ',
                        type: 'error',
                        duration: 1500
                    })
                }
            })
        },
        async register() {
            await this.$api.users.register(this.form)
                .then(response => {
                    this.login(response.data.username, response.data.password)
                })
                .catch(error => {
                    console.error(error)
                })
        },
        cancel() {
            this.$router.push('/')
        },

    },
}
</script>

<template>
    <the-header :classList="{ 'is-hidden': !showHeader }"  :height="40" dashboard></the-header>
    <el-container class="signup-layout">
        <div class="background-img"></div>        
        <el-main>
            <div class="signup-form">
                <div class="signup-form__header">Đăng ký</div>
                <div class="signup-form__body">
                    <el-form ref="signUpForm" :model="this.form" label-width="150px" :rules="this.rules" status-icon>
                        <el-form-item label="Tên tài khoản" label-position="right" prop="username">
                            <el-input v-model="form.username" autocomplete="off" />
                        </el-form-item>
                        <el-form-item label="Họ tên" label-position="right" prop="displayName">
                            <el-input v-model="form.displayName" autocomplete="off" />
                        </el-form-item>
                        <el-form-item label="Địa chỉ email" label-position="right" prop="email">
                            <el-input v-model="form.email" autocomplete="off" />
                        </el-form-item>
                        <el-form-item label="Mật khẩu" label-position="right" prop="password">
                            <el-input type="password" show-password v-model="form.password" autocomplete="off" />
                        </el-form-item>
                        <el-form-item label="Xác nhận mật khẩu" label-position="right" prop="confirmPass">
                            <el-input type="password" show-password v-model="form.confirmPass" autocomplete="off" />
                        </el-form-item>
                    </el-form>
                </div>

                <div class="signup-form__footer">
                    <el-button @click="cancel">Hủy</el-button>
                    <el-button type="primary" @click="submitForm(this.$refs.signUpForm)">
                        Đăng ký
                    </el-button>
                </div>
            </div>
        </el-main>
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