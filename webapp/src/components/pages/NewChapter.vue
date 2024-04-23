<script lang="js">
import { ElNotification } from 'element-plus'
import { mapActions, mapGetters } from 'vuex'
import axios from 'axios'

export default {
    name: 'NewChapter',
    data() {
        return {
            chapter: {},
            novel: this.getNovel,
            chapterContent: '',
            minChapterNum: 0,
            chapterFileContent: '',
            fileList: [],

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
            this.chapterContent = ''
            this.fileList = []
            this.novel = this.getNovel
        }
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
        }
    },
    methods: {
        ...mapActions('novel', {
            saveNovel: 'updateNovel'
        }),
        async getNewChapter() {
            var url = `http://localhost:10454/api/BookChapters/new-chapter?bookId=${this.getNovel.bookId}`
            console.log(this.gettersAuthData.token)
            await axios.get(url, {
                headers: {
                    'access_token': `${this.gettersAuthData.token}`
                }
            }).then(response => {
                this.chapter = response.data
                this.chapter.createBy = this.gettersAuthData.userId
                this.chapter.updateBy = this.gettersAuthData.userId
            }).catch(e => {
                console.error(e)
            })
        },
        async loadChapter() {
            var url = `http://localhost:10454/api/BookChapters/get-chapter?chapterId=${this.chapterId}`
            await axios.get(url)
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
            this.chapterContent = text
        },

        async saveChapter() {
            if (this.chapter.status == 1) {
                this.novel.chapters -= 1
            }
            if (this.$route.name === 'EditChapter') {
                var oldStatus = this.chapter.status
                var url = `http://localhost:10454/api/BookChapters/change-status?chapterId=${this.chapter.chapterId}&status=0`
                await axios.put(url, this.chapter, {
                    headers: {
                        'access_token': `${this.gettersAuthData.token}`,
                        'ownerId': `${this.novel.uploaderId}`
                    }
                }).then(response => {
                    if (response.status == 200) {
                        var uploadUrl = `http://localhost:10454/api/BookChapters/upload-chapter?chapterId=${this.chapter.chapterId}`
                        var txtEncode = new TextEncoder('utf-8').encode(this.chapterContent)
                        var file = new Blob([txtEncode], { type: 'text/plain' })
                        var formData = new FormData()
                        formData.append('file', file, this.chapter.fileName)
                        return axios.post(uploadUrl, formData, {
                            headers: {
                                'Content-Type': 'multipart/form-data',
                                'access_token': `${this.gettersAuthData.token}`,
                                'ownerId': `${this.novel.uploaderId}`
                            }
                        })
                    } else {
                        ElNotification({ title: 'Lỗi', message: 'Đã có lỗi xảy ra!!', type: 'error' })
                    }
                }).then(response => {
                    if (response.status == 201) {
                        if (oldStatus == 1) {
                            this.updateNovel()
                        }
                        ElNotification({ title: 'Thành công', message: 'Cập nhật thành công!', type: 'success' })
                    } else {
                        ElNotification({ title: 'Lỗi', message: 'Đã có lỗi xảy ra!!', type: 'error' })
                    }
                    this.$router.push(`/dashboard/workspace/${this.getNovel.bookId}`)
                })
            } else {
                this.chapter.status = 0
                var url = `http://localhost:10454/api/BookChapters/add`
                await axios.post(url, this.chapter, {
                    headers: {
                        'Content-Type': 'application/json-patch+json',
                        'access_token': `${this.gettersAuthData.token}`,
                        'ownerId': `${this.novel.uploaderId}`
                    }
                }).then(response => {
                    if (response.status == 201) {
                        this.chapter = response.data
                        var uploadUrl = `http://localhost:10454/api/BookChapters/upload-chapter?chapterId=${this.chapter.chapterId}`
                        var txtEncode = new TextEncoder('utf-8').encode(this.chapterContent)
                        var file = new Blob([txtEncode], { type: 'text/plain' })
                        var formData = new FormData()
                        formData.append('file', file, this.chapter.fileName)
                        return axios.post(uploadUrl, formData, {
                            headers: {
                                'Content-Type': 'multipart/form-data',
                                'access_token': `${this.gettersAuthData.token}`,
                                'ownerId': `${this.novel.uploaderId}`
                            }
                        })
                    } else {
                        ElNotification({title: 'Lỗi', message: 'Đã có lỗi xảy ra!!',type: 'error'})
                    }
                }).then(response => {
                    if (response.status == 201) {
                        ElNotification({title: 'Thành công',message: 'Đã lưu bản nháp mới!',type: 'success'})
                    } else {
                        ElNotification({title: 'Lỗi', message: 'Đã có lỗi xảy ra!!',type: 'error',})
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
            }
            if (this.$route.name === 'EditChapter') {
                var oldStatus = this.chapter.status
                var url = `http://localhost:10454/api/BookChapters/change-status?chapterId=${this.chapter.chapterId}&status=1`
                await axios.put(url, this.chapter, {
                    headers: {
                        'Content-Type': 'application/json-patch+json',
                        'access_token': `${this.gettersAuthData.token}`
                    }
                }).then(response => {
                    if (response.status == 200) {
                        var uploadUrl = `http://localhost:10454/api/BookChapters/upload-chapter?chapterId=${this.chapter.chapterId}`
                        var txtEncode = new TextEncoder('utf-8').encode(this.chapterContent)
                        var file = new Blob([txtEncode], { type: 'text/plain' })
                        var formData = new FormData()
                        formData.append('file', file, this.chapter.fileName)
                        return axios.post(uploadUrl, formData, {
                            headers: {
                                'Content-Type': 'multipart/form-data',
                                'access_token': `${this.gettersAuthData.token}`
                            }
                        })
                    } else {
                        ElNotification({title: 'Lỗi', message: 'Đã có lỗi xảy ra!!',type: 'error',})
                    }
                }).then(response => {
                    if (response.status == 201) {
                        ElNotification({title: 'Thành công', message: 'Xuất bản chương mới thành công!', type: 'success'})
                        if (oldStatus != 1) {
                            this.updateNovel()
                        }
                    } else {
                        ElNotification({title: 'Lỗi', message: 'Đã có lỗi xảy ra!!',type: 'error'})
                    }
                    this.$router.push(`/dashboard/workspace/${this.getNovel.bookId}`)
                }).catch(error => {
                        console.log(error)
                    })
            } else {
                this.chapter.Status = 1
                var url = `http://localhost:10454/api/BookChapters/add`
                await axios.post(url, this.chapter, {
                    headers: {
                        'Content-Type': 'application/json-patch+json',
                        'access_token': `${this.gettersAuthData.token}`,
                        'ownerId': `${this.novel.uploaderId}`
                    }
                }).then(response => {
                    if (response.status == 201) {
                        this.chapter = response.data
                        var uploadUrl = `http://localhost:10454/api/BookChapters/upload-chapter?chapterId=${this.chapter.chapterId}`
                        var txtEncode = new TextEncoder('utf-8').encode(this.chapterContent)
                        var file = new Blob([txtEncode], { type: 'text/plain' })
                        var formData = new FormData()
                        formData.append('file', file, this.chapter.fileName)
                        return axios.post(uploadUrl, formData, {
                            headers: {
                                'Content-Type': 'multipart/form-data',
                                'access_token': `${this.gettersAuthData.token}`,
                                'ownerId': `${this.novel.uploaderId}`
                            }
                        })
                    } else {
                        ElNotification({ title: 'Lỗi', message: 'Đã có lỗi xảy ra!!', type: 'error' })
                    }
                }).then(response => {
                    if (response.status == 201) {
                        this.updateNovel()
                        ElNotification({title: 'Thành công', message: 'Xuất bản chương mới thành công!', type: 'success'})
                    } else {
                        ElNotification({title: 'Lỗi', message: 'Đã có lỗi xảy ra!!',type: 'error'})
                    }
                    this.$router.push(`/dashboard/workspace/${this.getNovel.bookId}`)                    
                })
                    .catch(error => {
                        console.error(error)
                    })
            }

        },
        // hàm này đang bị sai logic
        async updateNovel() {
            var url = `http://localhost:10454/api/BookInfo/update?bookId=${this.getNovel.bookId}`
            await axios.put(url, this.novel, {
                headers: {
                    'Content-Type': 'application/json-patch+json',
                    'access_token': `${this.gettersAuthData.token}`,
                    'ownerId': `${this.novel.uploaderId}`
                }
            }).then(response => {
                // something                
            }).catch(error => {
                console.log(error)
            })
        },
        beforeUpload(rawFile) {
            // this.coverUrl = URL.createObjectURL(rawFile.raw);
            // trước khi upload file txt thì sẽ gọi api để lưu thông tin chương mới trước
            if (rawFile.type !== 'text/plain') {
                this.$message.error('Phải là định dạng .txt!');
                return false;
            }
            return true;
        },
        handleSuccess(response, uploadFile) {
            console.log(uploadFile)
            // this.coverUrl = URL.createObjectURL(uploadFile.raw!);
            this.fileList.push(uploadFile)
        },
    }

}
</script>

<template>
    <dashboard-layout>
        <template #header-content>Chương mới</template>
        <template #header-extra>
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
                <quill-editor toolbar="essential" contentType="text"
                    v-model:content="this.chapterContent"></quill-editor>
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
}

.el-form {
    margin: 20px 20px 0;
}

/* :deep(.el-input__wrapper) {
    margin: 20px 20px 0;
} */


:deep(.ql-container) {
    /* position: sticky; */
    /* max-height: calc(100% - 20px); */
    max-height: 100%;
}
</style>