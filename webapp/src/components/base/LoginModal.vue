<template>
    <!-- Form -->

    <el-dialog v-model="dialogFormVisible" style="width: 30%">
        <el-form :model="form">
            <el-form-item label="Username">
                <el-input v-model="form.username" autocomplete="off" />
            </el-form-item>
            <el-form-item label="Password" label-position="right">
                <el-input type="password" show-password v-model="form.password" autocomplete="off" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer" label-position="right">
                <el-button @click="cancel">Hủy</el-button>
                <el-button type="primary" @click="login">
                    Đăng nhập
                </el-button>
            </span>
        </template>
    </el-dialog>
</template>
  
<script lang="ts">
import { mapActions, mapGetters } from 'vuex';
import { ElNotification } from 'element-plus'
export default {
    name: 'LoginModal',
    data() {
        return {
            dialogFormVisible: false,
            form: {
                username: '',
                password: '',
            }
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
            await this.actionLogin({ username: this.form.username, password: this.form.password });
            if (this.getterLoginStatus === true) {
                // alert('login sucess');
                this.dialogFormVisible = false
                ElNotification({
                        title: 'Thành công!',
                        message: 'Đăng nhập thành công',
                        type: 'success',
                        duration: 900,
                        showClose: false,
                    })
                this.$emit('close')
            } else {
                // alert('failed to login')
                ElNotification({
                        title: 'Đã có lỗi xảy ra!',
                        message: 'Tên đăng nhập hoặc mật khẩu không đúng',
                        type: 'error',
                        duration: 900,
                        showClose: false,

                    })
                this.form.username = '';
                this.form.password = '';
                // this.$emit('close')
            }
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

.dialog-footer button:first-child {
    margin-right: 10px;
}
</style>
  