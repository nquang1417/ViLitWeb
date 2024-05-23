<script>
export default {
    name: 'UserInfo',
    props: ['data', 'formRef', 'admin', 'user'],
    data() {
        return {
            dialogFormVisible: false,
            form: {},
            formData: {},
            avaUrl: '',
            uploadUrl: `http://localhost:10454/api/Uploads/upload-image`,
        }
    },
    emits: ['update:data'],
    watch: {
        dialogFormVisible() {
            this.data = {}
        },
        formData: {
            handler() {
                switch (this.formData.gender) {
                    case 1:
                        this.formData.genderStr = 'Nam'
                        break
                    case 2:
                        this.formData.genderStr = "Nữ"
                        break
                    case 0:
                        this.formData.genderStr = "Khác"
                        break
                    default: 
                        this.formData.genderStr = null
                        break
                }
                this.$emit('update:data', this.formData)
            },
            deep: true
        },
        data: {
            handler() {
                this.formData = {...this.data}
                this.avaUrl = this.data.avatar
            },
            deep: true
        }
    },
    mounted() {        
        this.formData = {...this.data}
        this.avaUrl = this.data.avatar
    },
    methods: {
        handleErrorAva() {
            this.avaUrl = 'https://i0.wp.com/sbcf.fr/wp-content/uploads/2018/03/sbcf-default-avatar.png'
        },
        handleSuccess(response, uploadFile) {
            this.formData.avatar = response.data.image.url
            this.avaUrl = response.data.image.url
            // this.updateUser('Cập nhật ảnh đại diện thành công')
        },
        beforeUpload(rawFile) {
            if (rawFile.type !== 'image/jpeg') {
                this.$message.error('Định dạng ảnh bìa phải là jpg!');
                return false;
            } else if (rawFile.size / 1024 / 1024 > 24) {
                this.$message.error('Ảnh bìa không được có kích thước lớn hơn 24MB!');
                return false;
            }
            return true;
        },
        
    }
}
</script>

<template>
    <div class="info-dialog">
        <el-form ref="editForm" class="info-dialog__form" :model="formData" label-position="right" label-width="auto">
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Tài khoản">
                        <el-input class="dialog-input" readonly v-model="formData.username" autocomplete="off" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Email">
                        <el-input class="dialog-input" v-model="formData.email" autocomplete="off" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Họ và tên">
                        <el-input class="dialog-input" v-model="formData.displayName" autocomplete="off" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Giới tính">
                        <!-- <el-select class="dialog-input" v-model="data.gender" autocomplete="off" /> -->
                        <el-select v-model="formData.gender" placeholder="Giói tính" >
                            <el-option label="Nam" :value=1></el-option>
                            <el-option label="Nữ" :value=2></el-option>
                            <el-option label="Khác" :value=0></el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Quyền truy cập">
                        <!-- <el-input class="dialog-input"  v-model="formData.username" autocomplete="off" /> -->
                        <el-select v-model="formData.role" placeholder="Vai trò" :disabled="!this.admin">
                            <el-option label="User" :value=1></el-option>
                            <el-option label="Admin" :value=0></el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Trạng thái">
                        <el-select v-model="formData.status" placeholder="Chọn trạng thái" class="m-2" clearable :disabled="!this.admin">
                            <el-option label="Đang hoạt động" :value=1></el-option>
                            <el-option label="Tạm khóa" :value=2></el-option>
                            <el-option label="Không hoạt động" :value=0></el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-form-item label="Giới thiệu">
                <template #default>
                    <div class="editor">
                        <quill-editor contentType="text" v-model:content="this.formData.about">
                        </quill-editor>
                    </div>

                </template>
            </el-form-item>
        </el-form>
        <div class="avatar-box">
            <div class="user-avatar">
                <el-image :src="avaUrl" @error="handleErrorAva">
                </el-image>
                <el-upload class="ava-uploader" :action="this.uploadUrl" :show-file-list="false"
                    :on-success="handleSuccess" :before-upload="beforeUpload">
                    <template #default>
                        <div class="profile-changer-ava">
                            <span class="changer-icon">
                                <i class="fas fa-camera"></i>
                            </span>
                        </div>
                    </template>
                </el-upload>
            </div>
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
    flex: 1;
}

.avatar-box {
    width: fit-content;
    display: flex;
    justify-content: center;
}

.user-avatar {
    height: 200px;
    width: 200px;
    border-radius: 100px;
    border: 1px solid #dadbdd;
    overflow: hidden;
    position: relative;
    /* background-color: antiquewhite; */
}

.user-avatar:hover .profile-changer-ava {
    position: absolute;
    bottom: 0;
    left: 0;
    width: 100%;
    text-align: center;
    z-index: 2;
    color: #fff;
    line-height: 3rem;
    background-color: rgb(0,0,0,0.6);
}

.dialog-input {
    width:  100%;
}

.el-button--text {
    margin-right: 15px;
}

.el-input {
    flex: 1;
}

.el-select {
    flex: 1;
}

.editor {
    flex: 1;
}

:deep(.ql-container) {
    min-height: 150px;
}
</style>