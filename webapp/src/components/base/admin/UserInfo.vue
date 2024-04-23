<script>
export default {
    name: 'UserInfo',
    props: ['data'],
    data() {
        return {
            dialogFormVisible: false,
            form: {}
        }
    },
    emits: ['update:data'],
    watch: {
        dialogFormVisible() {
            this.data = {}
        },
    },
    methods: {
    }
}
</script>

<template>
    <div class="info-dialog">
        <el-form class="info-dialog__form" :model="data" label-position="right" label-width="auto">
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Tài khoản">
                        <el-input class="dialog-input" readonly v-model="data.username" autocomplete="off" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Email">
                        <el-input class="dialog-input" v-model="data.email" autocomplete="off" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Họ và tên">
                        <el-input class="dialog-input" v-model="data.displayName" autocomplete="off" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Giới tính">
                        <!-- <el-select class="dialog-input" v-model="data.gender" autocomplete="off" /> -->
                        <el-select v-model="data.gender" placeholder="Giói tính" >
                            <el-option label="Nam" :value=1></el-option>
                            <el-option label="Nữ" :value=2></el-option>
                            <el-option label="Khác" :value=0></el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>
        <div class="avatar-box">
            safadfasf
        </div>
    </div>

</template>

<style scoped>
.info-dialog {
    width: 100%;
    display: flex;
    gap: 10px;
    justify-content: space-between;
}
.info-dialog__form {
    width: 70%;
}

.avatar-box {
    width: 30%;
}
.dialog-input {
    width:  100%;
}

.el-button--text {
    margin-right: 15px;
}

.el-input {
    max-width: 100%;
}
</style>