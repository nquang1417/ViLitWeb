<script lang="js">
import { ElMessage } from 'element-plus'
import { mapActions, mapGetters } from 'vuex'
import dayjs from "dayjs"
import { ref,inject } from 'vue'
import { Quill } from "@vueup/vue-quill"
import BlotFormatter from 'quill-blot-formatter'
import ImageUploader from 'quill-image-uploader'
import { htmlEditButton } from "quill-html-edit-button"

export default {
    name: 'NewChapter',
    setup() {
        if (Quill) {
            Quill.register({
                "modules/htmlEditButton": htmlEditButton,
                "modules/blotFormater": BlotFormatter,                
            })
        }
    },
    data() {
        return {
            chapter: {},
            novel: this.getNovel,
            chapterContent: {},
            minChapterNum: 0,
            chapterFileContent: '',
            fileList: [],
            quillModules: {},
            options: {
                debug: 'info',
                modules: {
                    toolbar: [
                        [{ 'header': [1,2,3,4,5,6,false]}],
                        [{'size': ['small',false,'large','huge']}],
                        ['bold', 'italic', 'underline'],
                        ['blockquote', 'code-block'],                        
                        [{'list': 'ordered'}, {'list': 'bullet'}],
                        [{'align': []}],
                        [{ 'script': 'sub'}, { 'script': 'super' }], 
                        ['link', 'image']
                    ],
                    htmlEditButton
                },
                readOnly: false,
                theme: 'snow'
            }
        }
    },
    props: ['novelTitle', 'chapterId'],
    mounted() {
        this.novel = this.getNovel
        if (this.$route.name === 'EditChapter') {
            this.loadChapter()

        } else {
            this.getNewChapter()
        }
    },
    watch: {
        $route(to, from) {
            this.chapter = {}
            this.chapterContent = {}
            this.fileList = []
            this.novel = this.getNovel
        },
    },
    computed: {
        ...mapGetters('novel', {
            getNovel: 'getNovelInfo'

        }),
        ...mapGetters('auth', {
            gettersAuthData: 'getAuthData',
        }),
        uploadHeaders() {
            if (this.$route.name === 'EditChapter') {
                var header = {
                    'access_token': `${this.gettersAuthData.token}`,
                    'ownerId': `${this.novel.uploaderId}`
                }
                return header
            } else {
                var header = {
                    'access_token': `${this.gettersAuthData.token}`,
                }
                return header
            }
        },
        $api() {
            return inject('$api')
        },
        quill() {
            return this.$refs.myEditor
        }
    },
    methods: {
        ...mapActions('novel', {
            saveNovel: 'updateNovel'
        }),
        async getNewChapter() {            
            await this.$api.chapters.getNewChapter(this.getNovel.bookId, this.gettersAuthData.token)
                .then(response => {
                    this.chapter = response.data
                    this.chapter.createBy = this.gettersAuthData.userId
                    this.chapter.updateBy = this.gettersAuthData.userId
                }).catch(e => {
                    console.error(e)
                })
        },
        async loadChapter() {
            var token = this.gettersAuthData.token
            var owner = this.novel.uploaderId
            await this.$api.chapters.editChapterById(this.chapterId, token, owner)
                .then(response => {
                    this.chapter = response.data.chapter
                    this.chapterFileContent = response.data.file
                    this.minChapterNum = this.chapter.chapterNum
                })
                .catch(e => {
                    console.error(e)
                })
            var decodedContent = atob(this.chapterFileContent.fileContents)
            var utf8decoder = new TextDecoder('utf-8')
            var text = utf8decoder.decode(new Uint8Array([...decodedContent].map(char => char.charCodeAt(0))))
            this.chapterContent.text = text
            this.quill.setHTML(text)
            this.chapterContent.delta = this.quill.getContents()

        },

        async saveChapter() {
            if (this.chapter.status == 1) {
                this.novel.chapters -= 1
            }
            if (this.$route.name === 'EditChapter') {
                var oldStatus = this.chapter.status
                var token = this.gettersAuthData.token
                var owner = this.novel.uploaderId
                await this.$api.chapters.changeStatus(this.chapter.chapterId, 0, token, owner)
                    .then(response => {
                        if (response.status == 200) {
                            this.chapter.fileName = `${this.chapter.fileName.slice(0,-3)}json`
                            console.log(this.chapter.fileName)
                            var token = this.gettersAuthData.token
                            var owner = this.novel.uploaderId
                            return this.$api.chapters.uploadChapter(this.chapter, this.chapterContent.text, token, owner)
                        } else {
                            ElMessage({message: 'Đã có lỗi xảy ra!!', type: 'error', duration: 1500 })
                        }
                    }).then(response => {
                        if (response.status == 201) {
                            if (oldStatus == 1) {
                                this.updateNovel()
                            }
                            ElMessage({message: 'Cập nhật thành công!', type: 'success', duration: 1500 })
                        } else {
                            ElMessage({message: 'Đã có lỗi xảy ra!!', type: 'error', duration: 1500 })
                        }
                        this.$router.push(`/dashboard/workspace/${this.getNovel.bookId}`)
                    })
            } else {
                this.chapter.status = 0
                var token = this.gettersAuthData.token
                var owner = this.novel.uploaderId
                await this.$api.chapters.addChapter(this.chapter, token, owner)
                    .then(response => {
                        if (response.status == 201) {
                            this.chapter = response.data
                            this.chapter.fileName = `${this.chapter.fileName.slice(0,-3)}json`
                            console.log(this.chapter.fileName)
                            var token = this.gettersAuthData.token
                            var owner = this.novel.uploaderId
                            return this.$api.chapters.uploadChapter(this.chapter, this.chapterContent.text, token, owner)
                        } else {
                            ElMessage({message: 'Đã có lỗi xảy ra!!', type: 'error', duration: 1500 })
                        }
                    }).then(response => {
                        if (response.status == 201) {
                            ElMessage({message: 'Đã lưu bản nháp mới!', type: 'success', duration: 1500 })
                        } else {
                            ElMessage({message: 'Đã có lỗi xảy ra!!', type: 'error', duration: 1500 })
                        }
                        this.$router.push(`/dashboard/workspace/${this.getNovel.bookId}`)
                    })
                    .catch(error => {
                        console.error(error)
                    })
            }
        },

        async publishChapter() {

            if (this.chapter.status != 1) {
                this.novel.chapters += 1
                this.novel.updateDate = dayjs()
                console.log(this.novel.updateDate)
            }
            if (this.$route.name === 'EditChapter') {
                var oldStatus = this.chapter.status
                var token = this.gettersAuthData.token
                var owner = this.novel.uploaderId
                await this.$api.chapters.changeStatus(this.chapter.chapterId, 1, token, owner)
                    .then(response => {
                        if (response.status == 200) {
                            var token = this.gettersAuthData.token
                            var owner = this.novel.uploaderId
                            return this.$api.chapters.uploadChapter(this.chapter, this.chapterContent.text, token, owner)
                        } else {
                            ElMessage({message: 'Đã có lỗi xảy ra!!', type: 'error', duration: 1500 })
                        }
                    }).then(response => {
                        if (response.status == 201) {
                            ElMessage({message: 'Xuất bản chương mới thành công!', type: 'success', duration: 1500 })
                            if (oldStatus != 1) {
                                this.updateNovel()
                            }
                        } else {
                            ElMessage({message: 'Đã có lỗi xảy ra!!', type: 'error', duration: 1500 })
                        }
                        this.$router.push(`/dashboard/workspace/${this.getNovel.bookId}`)
                    }).catch(error => {
                        console.log(error)
                    })
            } else {
                this.chapter.Status = 1
                var token = this.gettersAuthData.token
                var owner = this.novel.uploaderId
                await this.$api.chapters.addChapter(this.chapter, token, owner)
                    .then(response => {
                        if (response.status == 201) {
                            this.chapter = response.data
                            var token = this.gettersAuthData.token
                            var owner = this.novel.uploaderId
                            return this.$api.chapters.uploadChapter(this.chapter, this.chapterContent.text, token, owner)
                        } else {
                            ElMessage({message: 'Đã có lỗi xảy ra!!', type: 'error', duration: 1500 })
                        }
                    }).then(response => {
                        if (response.status == 201) {
                            this.updateNovel()
                            ElMessage({message: 'Xuất bản chương mới thành công!', type: 'success', duration: 1500 })
                        } else {
                            ElMessage({message: 'Đã có lỗi xảy ra!!', type: 'error', duration: 1500 })
                        }
                        this.$router.push(`/dashboard/workspace/${this.getNovel.bookId}`)
                    })
                    .catch(error => {
                        console.error(error)
                    })
            }

        },
        async updateNovel() {
            var token = this.gettersAuthData.token
            var owner = this.novel.uploaderId
            await this.$api.novels.updateNovel(this.getNovel.bookId, this.novel, token, owner)
                .then(response => {
                    // something                
                }).catch(error => {
                    console.log(error)
                })
        },
        beforeUpload(rawFile) {
            // trước khi upload file txt thì sẽ gọi api để lưu thông tin chương mới trước
            if (rawFile.type !== 'text/plain') {
                this.$message.error('Phải là định dạng .txt!');
                return false;
            }
            return true;
        },
        handleSuccess(response, uploadFile) {
            console.log(uploadFile)
            this.fileList.push(uploadFile)
        },
    }

}
</script>

<template>
    <dashboard-layout>
        <template #header-content>Chương mới</template>
        <template #header-extra>
            <!-- <el-button type="default" plain icon="CollectionTag" @click="loadContent">load</el-button> -->
            <el-button type="default" plain icon="CollectionTag" @click="saveChapter">Lưu</el-button>
            <el-button type="primary" icon="Check" @click="publishChapter">Xuất bản</el-button>
        </template>
        <template #header-main>

            <el-form :model="this.chapter" label-width="100px" label-position="left" :rules="this.rules" status-icon>
                <el-form-item label="Tên truyên">
                    <div>{{ this.getNovel.bookTitle }}</div>
                </el-form-item>
                <el-form-item label="Tiêu đề" prop="chapterTitle">
                    <el-input v-model="this.chapter.chapterTitle" />
                </el-form-item>
                <!-- <el-form-item>
                    <el-upload v-model:file-list="fileList" class="upload-demo"
                        :action="`http://localhost:10454/api/BookChapters/upload-chapter?chapterId=${this.chapter.chapterId}`"
                        :headers="uploadHeaders"
                        :before-upload="beforeUpload" :on-success="handleSuccess">
                        <el-button type="primary">Click to upload</el-button>
                    </el-upload>
                </el-form-item> -->
            </el-form>
        </template>
        <el-container class="editor">
            <el-main>
                <quill-editor ref="myEditor" contentType="delta"
                    v-model:content="this.chapterContent.delta" 
                    @update:content="this.chapterContent.text = this.quill.getHTML()"
                    :options="options">
                </quill-editor>
            </el-main>
        </el-container>
    </dashboard-layout>
</template>

<style scoped>
.editor {
    border: 1px solid #ccc;
    border-radius: 6px;
    margin: 20px;
    height: calc(100vh - 50px);
    
}

.editor .el-main {
    padding: 0;
    position: relative;
    /* max-height: calc(100vh - 50px); */
    overflow: clip;
    background-color: #fff !important;
}

.el-form {
    margin: 20px 20px 0;
}

:deep(.ql-container) {
    max-height: 93%;
}
</style>