<script>
import { inject } from 'vue'
import dayjs from "dayjs"

export default {
    name: 'NovelInfo',
    props: ['data', 'formRef', 'novelId'],
    data() {
        return {
            dialogFormVisible: false,
            form: {},
            formData: {},
            coverUrl: ''
        }
    },
    emits: ['update:data'],
    watch: {
        dialogFormVisible() {
            this.formData = {}
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
                this.coverUrl = this.data.bookCover
            },
            deep: true
        },
        novelId() {
            this.formData = this.loadNovel(this.novelId)
        }
    },
    mounted() {        
        this.$api = inject('$api')
        // this.formData = {...this.data}
        this.formData = this.loadNovel(this.novelId)
        
    },
    methods: {
        handleErrorAva() {
            this.coverUrl = 'https://birkhauser.com/product-not-found.png'
        },
        async loadNovel(id) {
            await this.$api.novels.getDetails(id)
                .then(response => {
                    this.formData = response.data
                    this.coverUrl = response.data.bookCover
                    this.formData.formatedCreateDate = this.formatDate(response.data.createDate)
                    switch (this.formData.status) {
                        case 0:
                            this.formData.statusStr = "Tạm ngưng"
                            break;
                        case 1:
                            this.formData.statusStr = "Đang tiến hành"
                            break;
                        case 2:
                            this.formData.statusStr = "Hoàn thành"
                            break;
                        case 3:
                            this.formData.statusStr = "Tạm khóa"
                            break
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        formatDate(date) {
            var date = dayjs(date).format("DD/MM/YYYY HH:mm:ss")
            return date
        },
        handleDeleteCover() {
            this.coverUrl = 'https://birkhauser.com/product-not-found.png'
            this.formData.bookCover = 'https://birkhauser.com/product-not-found.png'
        },
    }
}
</script>

<template>
    <div class="info-dialog">
        <el-form ref="editForm" class="info-dialog__form" :model="formData" label-position="left" label-width="auto">
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Tiêu đề">
                        <el-input class="dialog-input" readonly v-model="formData.bookTitle" autocomplete="off" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Đánh giá">
                        <el-rate v-model="formData.averageRating" disabled show-score void-color="#f7ba2a"
                            score-template="{value}" /> 
                        <span style="margin-left: 4px; font-family: 'Roboto Medium';">/ {{ formData.reviews }} Đánh giá</span>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">                    
                    <el-form-item label="Tác giả">
                        <el-input class="dialog-input" readonly v-model="formData.authorName" autocomplete="off" />
                    </el-form-item>
                </el-col>
                <el-col :span="12">                                        
                    <el-form-item label="Thể loại">
                        <el-input class="dialog-input" readonly v-model="formData.genreName" autocomplete="off" />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">                    
                    <el-form-item label="Ngày đăng">                        
                        <el-date-picker class="dialog-input" v-model="formData.createDate" type="datetime"
                            placeholder="Pick a Date" format="YYYY/MM/DD HH:mm:ss" readonly />
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Ngày cập nhật">
                        <el-date-picker v-model="formData.createDate" type="datetime" placeholder="Pick a Date"
                            format="YYYY/MM/DD HH:mm:ss" readonly />
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Lượt theo dõi">
                        <el-input class="dialog-input" readonly v-model="formData.followers"></el-input>                        
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Trạng thái">
                        <el-input class="dialog-input" readonly v-model="formData.statusStr"></el-input>
                    </el-form-item>
                </el-col>                
            </el-row>
            <el-form-item label="Mô tả">
                <template #default>
                    <div class="editor">
                        <quill-editor contentType="text" v-model:content="this.formData.description">
                        </quill-editor>
                    </div>

                </template>
            </el-form-item>
        </el-form>
        <div class="cover-box">
            <div class="book-cover">
                <el-image :src="coverUrl" @error="handleErrorAva">
                </el-image>
                <div class="cover-delete" @click.prevent.stop="handleDeleteCover">
                    <span class="changer-icon">
                        Xóa ảnh bìa truyện
                    </span>
                </div>
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

.cover-box {
    width: fit-content;
    display: flex;
    justify-content: center;
}

.book-cover {
    height: 265px;
    width: 200px;
    border: 1px solid #dadbdd;
    overflow: hidden;
    position: relative;
}


.book-cover:hover .cover-delete {
    cursor: pointer;
    position: absolute;
    bottom: 0;
    left: 0;
    width: 100%;
    text-align: center;
    z-index: 2;
    color: #fff;
    line-height: 3rem;
    background-color: rgb(245, 108, 108, 0.8);
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

:deep(.el-date-editor) {
    flex: 1;
}

.editor {
    flex: 1;
}

:deep(.ql-container) {
    min-height: 150px;
}
</style>