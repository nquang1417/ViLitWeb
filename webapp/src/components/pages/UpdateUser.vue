<script>
import { mapActions, mapGetters } from 'vuex'
import { inject, ref } from 'vue'
import { ElMessage } from 'element-plus'
export default {
    name: 'UpdateUser',
    data() {
        const validateConfirmPass = (rule, value, callback) => {
            if (value !== this.passForm.newPassword) {
                callback(new Error('Mật khẩu không trùng khớp'));
            } else {
                callback()
            }
        }
        const validateNewPass = (rule, value, callback) => {
            if (value == this.passForm.oldPassword) {
                callback(new Error('Mật khẩu mới không trùng với mật khẩu hiện tại'));
            } else {
                callback()
            }
        }

        return {
            user: {},            
            passForm: {},
            pageName: '',
            passRules: {
                oldPassword: [
                    {required: true, message: 'Mật khẩu hiện tại không được bỏ trống', trigger: 'blur'},
                ],
                newPassword: [
                    {required: true, message: 'Mật khẩu mới không được bỏ trống', trigger: 'blur'},
                    { validator: validateNewPass, message: 'Mật khẩu mới không trùng với mật khẩu hiện tại', trigger: 'blur' }
                ],
                confirmNewPass: [
                    {required: true, message: 'Xác nhận mật khẩu mới không được bỏ trống', trigger: 'blur'},
                    { validator: validateConfirmPass, message: 'Mật khẩu không trùng khớp', trigger: 'blur' }
                ]
            }
        }
    },
    computed: {
        ...mapGetters('auth', {
            getUser: 'getAuthData'
        })
    },
    mounted() {
        this.$api = inject('$api')
        console.log(this.$route.name)  
        this.pageName = this.$route.name
        if (this.$route.name === "UpdateUser") {
            this.loadUser()
        }      
        if (this.$route.name === "ChangePass") {
            this.passForm = {}
            this.passForm.userId = this.getUser.userId
        }
    },
    watch: {
        $route(to, from) {
            this.pageName = this.$route.name
            if (this.$route.name === "UpdateUser") {
                this.loadUser()
            }
            if (this.$route.name === "ChangePass") {
                this.passForm = {}
                this.passForm.userId = this.getUser.userId
            }
        }
    },
    methods: {
        ...mapActions('auth', {
            updateName: 'updateName'
        }),
        async loadUser() {
            var userId = this.getUser.userId
            await this.$api.users.getUser(userId)
                .then(response => {
                    this.user = response.data
                    if (this.user.gender == -1) {
                        this.user.gender = null
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async updateUser(elForm) {
            var token = this.getUser.token
            this.user = elForm.formData
            console.log(this.user)
            await this.$api.users.updateUser(this.user, token)
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({message: 'Cập nhật thông tin thành công', type: 'success', duration: 1500})
                        this.updateName(this.user.displayName)
                        this.$router.push(`/profile/${this.getUser.userId}`)
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async submitPassword(elform) {
            if (!elform) return
                await elform.validate((valid, fields) => {
                    if (valid) {
                        // this.passForm.confirmNewPass = null
                        return valid
                    } else {
                        ElMessage({message: 'Đổi mật khẩu không hợp lệ', type:'error', duration: 1500})
                        return valid
                    }
                })
                .then(valid => {
                    if (valid) {
                        var token = this.getUser.token
                        return this.$api.users.changePass(this.passForm, token)       
                    }                    
                })
                .then(response => {
                    console.log(response)                    
                    if (response.status == 200) {
                        ElMessage({message: 'Đổi mật khẩu thành công', type:'success', duration: 1500})
                        this.$router.push(`/profile/${this.getUser.userId}`)
                    }
                })
                .catch(error => {
                    if (error.response.status == 401) {
                        ElMessage({message: 'Mật khẩu hiện tại không đúng', type:'error', duration: 1500})
                        this.passForm = {}
                        this.passForm.userId = this.getUser.userId
                    }
                })
        },
    },
}
</script>

<template>
    <dashboard-layout>
        <template #header-content>Cập nhật thông tin</template>
        <div class="form-wrapper">
            <div class="form-title">
                <span v-if="pageName == 'UpdateUser'">Thông tin cá nhân</span>
                <span v-if="pageName == 'ChangePass'">Đổi mật khẩu</span>
            </div>
            <div class="form-body">
                <user-info :data="this.user" ref="form" v-if="pageName == 'UpdateUser'"></user-info>
                <el-form ref="passForm" :model="passForm" label-position="right" label-width="auto" :rules="passRules" v-if="pageName == 'ChangePass'">
                    <el-form-item prop="oldPassword" label="Mật khẩu hiện tại">
                        <el-input type="password" show-password v-model="passForm.oldPassword"></el-input>
                    </el-form-item>
                    <el-form-item prop="newPassword" label="Mật khẩu mới">
                        <el-input type="password" show-password v-model="passForm.newPassword"></el-input>                        
                    </el-form-item>
                    <el-form-item prop="confirmNewPass" label="Xác nhận mật khẩu mới">
                        <el-input type="password" show-password v-model="passForm.confirmNewPass"></el-input>                                                
                    </el-form-item>
                </el-form>
            </div>
            <div class="form-footer">
                <el-button type="primary" @click="updateUser(this.$refs.form)" v-if="pageName == 'UpdateUser'">Cập nhật</el-button>
                <el-button type="primary" @click="submitPassword(this.$refs.passForm)" v-if="pageName == 'ChangePass'">Lưu</el-button>
                <el-button type="default" @click="this.$router.go(-1)">Hủy</el-button>
            </div>
        </div>
    </dashboard-layout>
</template>

<style scoped>

.form-wrapper {
    width: 70%;
    display: block;
    margin: 20px auto;
    border-radius: 8px;
    border: 1px solid #ccc;
    overflow: hidden;
}

.form-title {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: left;
    /* border-radius: 8px 8px 0 0;
    border-bottom: 1px solid #ccc; */
    margin-bottom: 10px;
}

.form-body {
    padding: 10px 15px;
}

.form-footer {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: right;
    border-radius: 0 0 8px 8px;
    border-top: 1px solid #ccc;
}
</style>