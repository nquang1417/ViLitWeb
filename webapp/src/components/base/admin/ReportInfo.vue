<script>
import { inject } from 'vue'
import dayjs from "dayjs"

export default {
    name: 'ReportInfo',
    props: ['data', 'formRef', 'admin', 'user', 'reportType'],
    data() {
        return {
            dialogFormVisible: false,
            form: {},
            formData: {},
            avaUrl: '',
            user: {},
            novel: {},
            sender: {},
            target: {},
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
                this.$emit('update:data', this.formData)
            },
            deep: true
        },
        data: {
            handler() {
                this.formData = {...this.data}
                
                this.loadSender()
                this.loadTarget()
            },
            deep: true
        }
    },
    mounted() {        
        this.$api = inject('$api')
        this.formData = {...this.data}        
        this.loadSender()
        this.loadTarget()
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
        async loadSender() {
            var userId = this.formData.senderId
            await this.$api.users.getUser(userId)
                .then(response => {
                    this.sender = response.data                    
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async loadNovel() {
            var bookId = this.formData.reportedEntityId
            await this.$api.novels.getDetails(bookId) 
                .then(response => {
                    this.novel = response.data
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async loadUser() {
            var userId = this.formData.reportedEntityId
            await this.$api.users.getUser(userId)
                .then(response => {
                    this.formData.user = response.data
                })
                .catch(error => {
                    console.error(error)
                })
        },
        loadTarget() {
            if (this.reportType == 0) {
                this.formData.objectName = "Người dùng"
                this.loadUser()
            } 
            if (this.reportType == 1) {
                this.formData.objectName = "Tiểu thuyết"
                this.loadNovel()
            }
        },
        formatDate(date) {
            var date = dayjs(date).format("DD/MM/YYYY HH:mm:ss")
            return date
        },
    }
}
</script>

<template>
    <div class="info-dialog">
        <el-form ref="editForm" class="info-dialog__form" :model="formData" label-position="right" label-width="auto">
            <el-row :gutter="10">
                <el-col :span="18">
                    <el-form-item label="Tên báo cáo">
                        <el-input class="dialog-input" readonly v-model="formData.reportName" autocomplete="off" />
                    </el-form-item>
                </el-col>
                <el-col :span="6">
                    <el-form-item label="Người gửi">
                        <el-input class="dialog-input" readonly v-model="this.sender.username" autocomplete="off" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="24">
                    <el-form-item label="Nội dung">
                        <el-input class="dialog-input" readonly v-model="formData.message" autocomplete="off" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Đối tượng báo cáo">
                        <el-input class="dialog-input" readonly v-model="formData.objectName" autocomplete="off" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Tiêu đề" v-if="formData.reportedType == 1">
                        <el-input class="dialog-input" readonly v-model="novel.bookTitle" autocomplete="off" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Người đăng" v-if="formData.reportedType == 1">
                        <el-input class="dialog-input" readonly v-model="novel.username" autocomplete="off" />
                    </el-form-item>
                </el-col>                
                <el-col :span="12">
                    <el-form-item label="Ngày đăng">
                        <!-- <el-input class="dialog-input" readonly v-model="formData.formatedCreateDate" autocomplete="off" /> -->
                        <el-date-picker class="dialog-input" v-model="novel.createDate" type="datetime"
                            placeholder="Pick a Date" format="YYYY/MM/DD HH:mm:ss" readonly />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Ngày gửi báo cáo">
                        <!-- <el-input class="dialog-input" readonly v-model="formData.formatedCreateDate" autocomplete="off" /> -->
                        <el-date-picker class="dialog-input" v-model="novel.createDate" type="datetime"
                            placeholder="Pick a Date" format="YYYY/MM/DD HH:mm:ss" readonly />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Trạng thái">
                        <el-select v-model="formData.status" placeholder="Chọn trạng thái" class="m-2" clearable :disabled="!this.admin">
                            <el-option label="Đã xử lý" :value=1></el-option>                            
                            <el-option label="Chưa xử lý" :value=0></el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-form-item label="Phản hồi">
                <template #default>
                    <div class="editor">
                        <quill-editor contentType="text" v-model:content="this.formData.about">
                        </quill-editor>
                    </div>

                </template>
            </el-form-item>
        </el-form>
        
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

:deep(.el-date-editor) {
    flex: 1;
}

:deep(.ql-container) {
    min-height: 150px;
}
</style>