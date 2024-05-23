<script lang="js">
import { ElMessage } from 'element-plus'
import axios from 'axios'
import { mapActions, mapGetters } from 'vuex';
import { inject } from 'vue';
export default {
    name: 'NewNovel',
    data() {
        return {
            editorData: '<p>Content of the editor.</p>',
            editorConfig: {
                // The configuration of the editor.
                indent_style: 'tab',
                tab_width: 4,
                charset: 'utf-8',
                end_of_line: 'lf',
                trim_trailing_whitespace: true,
                insert_final_newline: true,
            },
            genres: [],
            tags: [],
            novelDetails: {},
            rules: {
                bookTitle: [
                    { required: true, message: 'Điền tiêu đề của tác phẩm', trigger: 'blur' },
                    // { max: 150, message: 'Độ dài tối đa 200 ký tự', trigger: 'blur' }
                ],
                authorName: [
                    { required: true, message: 'Điền tên hoặc bút danh của tác giả', trigger: 'blur' },
                ],
                genreId: [
                    { required: true, message: 'Chọn thể loại của tác phẩm', trigger: 'change' },
                ]
            },
            coverFile: null,
            coverUrl: '',
            previousRoute: '',
            uploadUrl: `http://localhost:10454/api/BookInfo/upload-cover`,
        }
    },
    beforeRouteLeave(to, from, next) {
        // Store the previous route path before leaving the current route
        this.previousRoute = from.path;
        next();
    },
    mounted() {
        this.$api = inject('$api')
        this.loadGenres()
        this.novelDetails.status = 1
        if (this.$route.name === 'EditNovel') {
            this.loadNovel()
        }
    },
    props: ['novelId'],
    computed: {
        ...mapGetters('auth', {
            gettersAuthData: 'getAuthData',
        }),
        uploadHeaders() {
            if (this.$route.name === 'EditNovel') {
                var header = {
                    'access_token': `${this.gettersAuthData.token}`,
                    'ownerId': `${this.novelDetails.uploaderId}`
                }
                return header
            } else {
                var header = {
                    'access_token': `${this.gettersAuthData.token}`,
                }
                return header
            }
        }
    },
    watch: {
        $route(to, from) {
            this.novelDetails = {
                bookTitle: '',
                genreId: '',
                description: '',
                bookCover: '',
                authorName: '',
                uploaderId: '',
                chapters: 0,
                languagueCode: 1,
                languageName: 'vi',
                // tags: ref(),
                status: 1,
                createBy: '',
                updateBy: '',
            }
            this.coverUrl = ''
            this.previousRoute = from.path
        },
    },
    methods: {
        ...mapActions('novel', {
            saveNovel: 'updateNovel'
        }),
        async update() {
            var bookId = this.novelDetails.bookId
            var token = this.gettersAuthData.token
            var owner = this.novelDetails.uploaderId
            await this.$api.novels.updateNovel(bookId, this.novelDetails, token, owner) 
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({
                            message: 'Cập nhật thành công!',
                            type: 'success',
                            duration: 1500
                        })
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async submit() {
            try {
                this.novelDetails.uploaderId = this.gettersAuthData.userId
                this.novelDetails.createBy = this.gettersAuthData.userId
                this.novelDetails.updateBy = this.gettersAuthData.userId

                var url = `http://localhost:10454/api/BookInfo/add`
                var response = await axios.post(url, this.novelDetails, {
                    headers: {
                        'Content-Type': 'application/json-patch+json',
                        'access_token': `${this.gettersAuthData.token}`
                    }
                })
                this.saveNovel(response.data)
                this.$router.push(`/dashboard/workspace/${response.data.bookId}`)
            } catch (error) {
                console.error(error)
            }
        },
        async submitForm(formEl) {

            if (!formEl) return
            await formEl.validate((valid, fields) => {
                if (valid) {

                    if (this.$route.name === 'NewNovel') {
                        ElMessage({
                            message: 'Đăng tải thành công!',
                            type: 'success', duration: 1500
                        })
                        this.submit()

                    }
                    if (this.$route.name === 'EditNovel') {                        
                        this.update()
                        this.$router.push(`/dashboard/workspace/${this.novelDetails.bookId}`)
                    }
                } else {
                    ElMessage({
                        message: 'Dữ liệu đầu vào không hợp lệ',
                        type: 'error', duration: 1500
                    })
                }
            })
        },


        async loadGenres() {
            var url = `http://localhost:10454/api/Genres/lists`;
            await axios.get(url)
                .then(response => {
                    this.genres = response.data;
                })
                .catch(e => {
                    this.errors.push(e)
                })
        },
        async loadNovel() {
            var url = `http://localhost:10454/api/BookInfo/details?bookId=${this.novelId}`
            await axios.get(url)
                .then(response => {
                    this.novelDetails = JSON.parse(JSON.stringify(response.data))
                })
                .catch(e => {
                    console.error(e);
                })
            this.coverUrl = this.novelDetails.bookCover

        },
        beforeUpload(rawFile) {
            // this.coverUrl = URL.createObjectURL(rawFile.raw);
            // var url  = URL.createObjectURL(rawFile.raw)
            if (rawFile.type !== 'image/jpeg') {
                this.$message.error('Định dạng ảnh bìa phải là jpg!');
                return false;
            } else if (rawFile.size / 1024 / 1024 > 10) {
                this.$message.error('Ảnh bìa không được có kích thước lớn hơn 10MB!');
                return false;
            }
            return true;
        },
        handleSuccess(response, uploadFile) {
            this.novelDetails.bookCover = response.data.image.url
            this.coverUrl = response.data.thumb.url;
        },
    }
}
</script>


<template>
    <dashboard-layout>
        <template #header-content>{{ this.$route.name === 'EditNovel' ? 'Chỉnh sửa' : 'Thêm truyện' }}</template>
        <div class="detail-form">
            <div class="detail-form__header">
                <span>Series</span>
            </div>
            <div class="detail-form__body">
                <el-form ref="ruleFormRef" :model="this.novelDetails" label-width="120px" :rules="this.rules" label-position="right">
                    <el-form-item>
                        <el-upload class="cover-uploader" :action="this.uploadUrl" :show-file-list="false" :headers="this.uploadHeaders"
                            :on-success="handleSuccess" :before-upload="beforeUpload">
                            <img v-if="this.coverUrl" :src="this.coverUrl" class="cover" />
                            <el-icon v-else class="cover-uploader-icon">
                                <Plus />
                            </el-icon>
                        </el-upload>
                    </el-form-item>
                    <el-form-item label="Tiêu đề" prop="bookTitle">
                        <el-input v-model="this.novelDetails.bookTitle" />
                    </el-form-item>
                    <el-form-item label="Tác giả" prop="authorName">
                        <el-input v-model="this.novelDetails.authorName" />
                    </el-form-item>
                    <el-form-item label="Tóm tắt" prop="description">
                        <!-- <el-input type="textarea" :autosize="{ minRows: 4, maxRows: 10 }"
                            v-model="this.novelDetails.description" /> -->
                        <div class="editor">
                            <quill-editor toolbar="essential" v-model:content="this.novelDetails.description"
                                contentType="text"></quill-editor>
                        </div>

                    </el-form-item>
                    <el-form-item label="Thể loại" prop="genreId">
                        <el-select v-model="this.novelDetails.genreId" class="m-2" placeholder="Chọn thể loại" style="width: 240px">
                            <el-option v-for="genre in this.genres" :key="genre.genreId" :label="genre.genreName"
                                :value="genre.genreId"></el-option>
                        </el-select>
                    </el-form-item>
                    <!-- <el-form-item label="Tags">
                        <el-checkbox-group v-model="this.novelDetails.tags">
                            <el-checkbox v-for="tag in this.tags" :key="tag.key" :value="tag.value"
                                :label="tag.value"></el-checkbox>
                        </el-checkbox-group>
                    </el-form-item> -->
                    <el-form-item label="Trạng thái">
                        <el-select v-model="this.novelDetails.status" class="m-2" placeholder="Trạng thái" style="width: 240px" :disabled="this.novelDetails.status == 3">
                            <el-option label="Đang tiến hành" :value="1"></el-option>
                            <el-option label="Hoàn thành" :value="2"></el-option>
                            <el-option label="Tạm khóa" :value="3"></el-option>
                            <el-option label="Tạm ngưng" :value="0"></el-option>
                        </el-select>
                    </el-form-item>
                </el-form>
            </div>
            <div class="detail-form__footer">
                <el-row :gutter="12" justify="end" style="margin: 0">
                    <el-button type="primary" @click="this.submitForm(this.$refs.ruleFormRef)">
                        {{ this.$route.name === 'EditNovel' ? 'Cập nhật' : 'Thêm truyện' }}
                    </el-button>
                    <el-button type="default" @click="$router.go(-1)">Hủy</el-button>
                </el-row>
            </div>
        </div>
    </dashboard-layout>
</template>

<style scoped>
.detail-form {
    display: flex;
    flex-direction: column;
    gap: 10px;
    margin: 15px;
    border: 1px solid #ccc;
    border-radius: 8px;
}

.detail-form__header {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: left;
    border-radius: 8px 8px 0 0;
    border-bottom: 1px solid #ccc;
}

.detail-form__footer {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: left;
    border-radius: 0 0 8px 8px;
    border-top: 1px solid #ccc;
}

.detail-form .el-checkbox-group {
    text-align: left;
}

.el-form {
    border-radius: 8px;
}

.cover-uploader {
    border: 1px dashed #ccc;
    border-radius: 6px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
    transition: var(--el-transition-duration-fast);
}

.cover-uploader:hover {
    border-color: var(--el-color-primary);
}


.el-icon.cover-uploader-icon {
    font-size: 28px;
    color: #8c939d;
    width: 178px;
    height: 178px;
    text-align: center;
}


.detail-form__body {
    padding: 0 15px;
}
.picture-class {
    height: 148px;
    width: 148px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 40px;
    color: #ccc;
    background-color: #fff;
    border: 1px dashed #ccc;
    border-radius: 4px;
    cursor: pointer;
}

.editor {
    width: 100%;
}

:deep(.ql-container) {
    min-height: 250px;
    max-height: 400px;
    overflow-y: auto;
}

.cover-uploader .cover {
    width: 178px;
    height: 178px;
    display: block;
}

.cover-uploader .el-upload {
    border: 1px dashed var(--el-border-color);
    border-radius: 6px;
    cursor: pointer;
    position: relative;
    overflow: hidden;
    transition: var(--el-transition-duration-fast);
}

.cover-uploader .el-upload:hover {
    border-color: var(--el-color-primary);
}

:deep(.el-icon.cover-uploader-icon) {
    font-size: 28px;
    color: #8c939d;
    width: 178px;
    height: 178px;
    text-align: center;
}
</style>