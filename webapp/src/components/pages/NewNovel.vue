<script lang="ts">
import { ElNotification } from 'element-plus'
import axios from 'axios'
import { mapActions, mapGetters } from 'vuex';
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
            novelDetails: {
                BookTitle: '',
                GenreId: '',
                Description: '',
                BookCover: '',
                AuthorName: '',
                UploaderId: '',
                Chapters: 0,
                LanguagueCode: 1,
                LanguageName: 'vi',
                // tags: ref(),
                Status: '1',
                CreateBy: '',
                UpdateBy: '',
            },
            rules: {
                BookTitle: [
                    { required: true, message: 'Điền tiêu đề của tác phẩm', trigger: 'blur' },
                    // { max: 150, message: 'Độ dài tối đa 200 ký tự', trigger: 'blur' }
                ],
                AuthorName: [
                    { required: true, message: 'Điền tên hoặc bút danh của tác giả', trigger: 'blur' },
                ],
                GenreId: [
                    { required: true, message: 'Chọn thể loại của tác phẩm', trigger: 'change' },
                ]
            },
            coverFile: null,
            coverUrl: '',
            previousRoute: '',
            uploadUrl: `https://localhost:44367/api/Uploader/novel-cover`,
        }
    },
    beforeRouteLeave(to, from, next) {
        // Store the previous route path before leaving the current route
        this.previousRoute = from.path;
        next();
    },
    mounted() {
        this.loadGenres()
        this.novelDetails.UploaderId = this.gettersAuthData.userId
        this.novelDetails.CreateBy = this.gettersAuthData.userId
        this.novelDetails.UpdateBy = this.gettersAuthData.userId
        // console.log(this.$route.name === 'EditNovel')
        if (this.$route.name === 'EditNovel') {
            this.loadNovel()
        }
        console.log(this.uploadUrl)
    },
    props: ['novelId'],
    computed: {
        ...mapGetters('auth', {
            gettersAuthData: 'getAuthData',
        }),
        bookTitle() {
            return this.novelDetails.BookTitle
        }
    },
    watch: {
        $route(to, from) {
            this.novelDetails = {
                BookTitle: '',
                GenreId: '',
                Description: '',
                BookCover: '',
                AuthorName: '',
                UploaderId: '',
                Chapters: 0,
                LanguagueCode: 1,
                LanguageName: 'vi',
                // tags: ref(),
                Status: '1',
                CreateBy: '',
                UpdateBy: '',
            }
            this.coverUrl = ''
            this.previousRoute = from.path
        },
        bookTitle(value) {
            this.uploadUrl = `https://localhost:44367/api/Uploader/novel-cover?bookTitle=${value}`
            // console.log(this.uploadUrl)
        }
    },
    methods: {
        ...mapActions('novel', {
            saveNovel: 'updateNovel'
        }),
        async update() {
            try {

                var url = `https://localhost:44367/api/BookInfo/update`
                var respone = await axios.put(url, this.novelDetails, {
                    headers: {
                        'Content-Type': 'application/json-patch+json'
                    }
                })
                // this.uploadCover()
                // console.log(this.$refs.upload)
                // this.$refs.upload.submit()
            } catch (error) {
                console.error(error)
            }
        },
        async submit() {
            try {

                var url = `https://localhost:44367/api/BookInfo/add`
                var respone = await axios.post(url, this.novelDetails, {
                    headers: {
                        'Content-Type': 'application/json-patch+json'
                    }
                })
                // this.uploadCover()
                // console.log(this.$refs.upload)
                // this.$refs.upload.submit()
            } catch (error) {
                console.error(error)
            }
        },
        async submitForm(formEl) {

            if (!formEl) return
            await formEl.validate((valid, fields) => {
                if (valid) {

                    if (this.$route.name === 'NewNovel') {
                        ElNotification({
                            title: 'Thành công',
                            message: 'Đăng tải thành công!',
                            type: 'success',
                        })
                        //this.submit()
                        this.uploadCover()
                    }
                    if (this.$route.name === 'EditNovel') {
                        ElNotification({
                            title: 'Thành công',
                            message: 'Cập nhật thành công!',
                            type: 'success',
                        })
                        this.update()
                        this.uploadCover()
                        this.$router.back()
                    }
                } else {
                    ElNotification({
                        title: 'Lỗi',
                        message: 'Dữ liệu đầu vào không hợp lệ',
                        type: 'error',
                    })
                }
            })
        },
        async uploadCover() {
            var url = `https://localhost:44367/api/Uploader/novel-cover?bookTitle=${this.novelDetails.BookTitle}`
        
            var formData = new FormData()
            formData.append("file", this.coverFile)
            try {
                const response = await axios.post(url, formData, {
                    headers: {
                        "Content-Type": "multipart/form-data"
                    }
                })
            } catch (error) {
                console.error(error)
            }
        },
        triggerFileInput() {
            this.$refs.fileInput.click();
        },
        async handleFileUpload(file) {
            // this.coverFile = this.$refs.fileInput.files[0];
            
            // Xử lý tệp đã chọn theo yêu cầu của bạn
            var url = 'https://freeimage.host/api/1/upload'
            var formData = new FormData()
            formData.append('key', '6d207e02198a847aa98d0a2a901485a5')
            formData.append('source', file)
            formData.append('format', 'json')

            try {
                var response = await axios.post(url, formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data'
                    }
                })
                // this.coverUrl = response.data.image.url
                this.novelDetails.BookCover = response.data.image.url
            } catch (error) {
                console.error(error)
            }
        },
        async loadGenres() {
            var url = `https://localhost:44367/api/genres`;
            await axios.get(url)
                .then(response => {
                    this.genres = response.data;
                })
                .catch(e => {
                    this.errors.push(e)
                })
        },
        async loadNovel() {
            var url = `https://localhost:44367/api/BookInfo/${this.novelId}`
            await axios.get(url)
                .then(response => {
                    this.novelDetails = JSON.parse(JSON.stringify(response.data.BookInfo))
                })
                .catch(e => {
                    console.error(e);
                })
            this.coverUrl = this.novelDetails.BookCover

        },
        beforeUpload(rawFile) {
            // this.coverUrl = URL.createObjectURL(rawFile.raw);
            this.handleFileUpload(rawFile)
            this.coverFile = rawFile
            if (rawFile.type !== 'image/jpeg') {
                this.$message.error('Định dạng ảnh bìa phải là jpg!');
                return false;
            } else if (rawFile.size / 1024 / 1024 > 2) {
                this.$message.error('Ảnh bìa không được có kích thước lớn hơn 2MB!');
                return false;
            }
            return true;
        },
        handleSuccess(response, uploadFile) {
            console.log(uploadFile)
            this.coverUrl = URL.createObjectURL(uploadFile.raw!);
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
                <el-form ref="ruleFormRef" :model="this.novelDetails" label-width="120px" :rules="this.rules" status-icon>
                    <el-form-item>
                        <el-upload class="cover-uploader" 
                            :action="this.uploadUrl"
                            :show-file-list="false" :on-success="handleSuccess" 
                            :before-upload="beforeUpload">
                            <img v-if="this.coverUrl" :src="this.coverUrl" class="cover" />
                            <el-icon v-else class="cover-uploader-icon">
                                <Plus />
                            </el-icon>
                        </el-upload>
                        <!-- <label for="fileInput" class="picture-class" @click="triggerFileInput">
                            <img v-if="coverFile || coverUrl" :src="coverUrl" alt="" width="148" height="148">
                            <el-icon v-else>
                                <Plus />
                            </el-icon>
                        </label>
                        <input type="file" ref="fileInput" style="display: none;" @change="handleFileUpload" /> -->
                    </el-form-item>
                    <el-form-item label="Tiêu đề" prop="BookTitle">
                        <el-input v-model="this.novelDetails.BookTitle" />
                    </el-form-item>
                    <el-form-item label="Tác giả" prop="AuthorName">
                        <el-input v-model="this.novelDetails.AuthorName" />
                    </el-form-item>
                    <el-form-item label="Tóm tắt" prop="Description">
                        <!-- <el-input type="textarea" :autosize="{ minRows: 4, maxRows: 10 }"
                            v-model="this.novelDetails.Description" /> -->
                        <div class="editor">
                            <quill-editor toolbar="essential" v-model:content="this.novelDetails.Description"
                                contentType="text"></quill-editor>
                        </div>

                    </el-form-item>
                    <el-form-item label="Thể loại" prop="GenreId">
                        <el-select v-model="this.novelDetails.GenreId" class="m-2" placeholder="Chọn thể loại">
                            <el-option v-for="genre in this.genres" :key="genre.GenreId" :label="genre.GenreName"
                                :value="genre.GenreId"></el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="Tags">
                        <el-checkbox-group v-model="this.novelDetails.tags">
                            <el-checkbox v-for="tag in this.tags" :key="tag.key" :value="tag.value"
                                :label="tag.value"></el-checkbox>
                        </el-checkbox-group>
                    </el-form-item>
                    <el-form-item label="Trạng thái">
                        <el-select v-model="this.novelDetails.Status" class="m-2" placeholder="Trạng thái">
                            <el-option label="Đang tiến hành" value="1"></el-option>
                            <el-option label="Hoàn thành" value="2"></el-option>
                            <el-option label="Tạm ngưng" value="0"></el-option>
                        </el-select>
                    </el-form-item>
                </el-form>
            </div>
            <div class="detail-form__footer">
                <el-row :gutter="12" justify="end">
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