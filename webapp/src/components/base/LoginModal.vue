<template>
    <!-- Form -->

    <el-dialog v-model="dialogFormVisible" style="width: 30%">
        <el-form ref="loginForm" :model="form" :rules="this.rules" >
            <el-form-item label="Username" prop="username">
                <el-input v-model="form.username" autocomplete="off" @keydown.enter.native="handleEnter"/>
            </el-form-item>
            <el-form-item label="Password" prop="password" label-position="right">
                <el-input type="password" show-password v-model="form.password" autocomplete="off" @keydown.enter.native="handleEnter"/>
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <router-link class="register-btn" :to="'/signup'">Đăng ký</router-link>
                <div class="flex-grow"></div>
                <el-button @click="cancel">Hủy</el-button>
                <el-button type="primary" @click="login">
                    Đăng nhập
                </el-button>
            </span>
        </template>
    </el-dialog>
</template>
  
<script>
import { mapActions, mapGetters } from 'vuex';
import { ElMessage, ElMessageBox } from 'element-plus'
export default {
    name: 'LoginModal',
    data() {
        return {
            dialogFormVisible: false,
            form: {
                username: '',
                password: '',
            },
            rules: {
                username: [
                    { required: true, message: 'Nhập tên tài khoản', trigger: 'blur' },
                    
                ],
                password: [
                    { required: true, message: 'Nhập mật khẩu', trigger: 'blur' },
                ],
            },
        }
    },
    emits: ['openDialog', 'closeDialog'],
    props: {
        dialogVisible: Boolean,
    },
    computed: {
        ...mapGetters('auth', {
            getterLoginStatus: 'getLoginStatus',
            gettersAuthData: 'getAuthData'
        })
    },
    watch: {
        dialogFormVisible() {
            this.form.username = '';
            this.form.password = '';
        },
        dialogVisible() {
            this.dialogFormVisible = this.dialogVisible;
        }
    },
    methods: {
        ...mapActions('auth', {
            actionLogin: 'login'
        }),
        async login() {
            await this.actionLogin({ username: this.form.username, password: this.form.password })
                .then(response => {
                    if (response.data && this.getterLoginStatus === true) {
                        console.log(response)
                        this.dialogFormVisible = false
                        ElMessage({
                            message: 'Đăng nhập thành công',
                            type: 'success',
                            duration: 1500,
                        })
                        this.$emit('close')
                    } else {
                        var error = response.response.data
                        this.form.username = '';
                        this.form.password = '';
                        if (error.status == 1) {
                            ElMessage({
                                message: error.message,
                                type: 'error',
                                duration: 1500,
                            })
                        }
                        if (error.status == 2) {
                            ElMessageBox.alert(
                                `${error.message}`,
                                'Tài khoản đã bị khóa',
                                {
                                    confirmButtonText: 'Đóng',
                                    type: 'warning',
                                }
                            ).then(() => {
                                this.$emit('close')
                            })
                        }
                        
                    }
                })
        },
        async handleEnter(event) {
            var elForm = this.$refs.loginForm
            if (!elForm) return
            await elForm.validate((valid, fields) => {
                if (valid) {
                    this.login()
                }
            })
        },
        cancel() {
            this.dialogFormVisible = false
            this.$emit('close')
        },
        openDialog(dialogFormVisible) {
            this.dialogFormVisible = dialogFormVisible;
            // this.$emit('openDialog'); // Kích hoạt sự kiện openDialog từ component con
        },

        closeDialog() {
            this.dialogFormVisible = false;
            // this.$emit('closeDialog'); // Kích hoạt sự kiện closeDialog từ component con
        },

    },
}
</script>
<style scoped>
.el-button--text {
    margin-right: 15px;
}

.el-input {
    max-width: 100%;
}

.flex-grow {
    flex-grow: 1;
}

.dialog-footer {
    display: flex;
    
    align-items: center;
    width: 100%;
}
.register-btn {
    font-size: 14px;
    line-height: 16px;
    font-family: 'Roboto Italic';
}
.register-btn:hover {
    scale: 1.05;
    color: #409eff;
}
</style>
  