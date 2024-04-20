<script lang="js">
import { pad } from '../../scripts/utils/utils.js'
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
            var header = {
                'access_token': `${this.gettersAuthData.token}`
            }
            return header
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
            var url = `https://localhost:44367/api/BookChapters/${this.chapterId}`
            await axios.get(url)
                .then(response => {
                    this.chapter = response.data.Chapter
                    this.chapterFileContent = response.data.File
                    this.minChapterNum = this.chapter.ChapterNum
                })
                .catch(e => {
                    console.error(e)
                })
            var decodedContent = atob(this.chapterFileContent.FileContents)
            var utf8decoder = new TextDecoder('utf-8')
            var text = utf8decoder.decode(new Uint8Array([...decodedContent].map(char => char.charCodeAt(0))))
            this.chapterContent = text
        },
        async saveChapter() {
            if (this.$route.name === 'EditChapter') {
                var url = `https://localhost:44367/api/BookChapters/ChangeStatus?status=0`
                await axios.put(url, this.chapter, {
                    headers: {
                        'Content-Type': 'application/json-patch+json'
                    }
                }).then(response => {
                })
                    .catch(error => {
                        console.log(error)
                    })
            } else {
                this.chapter.Status = '0'
                var url = `https://localhost:44367/api/BookChapters/add`
                await axios.post(url, this.chapter, {
                    headers: {
                        'Content-Type': 'application/json-patch+json'
                    }
                }).then(response => {
                    this.$route.push(`/dashboard/workspace/${this.getNovel.BookId}`)
                })
                    .catch(error => {
                        console.error(error)
                    })
            }
        },
        async publishChapter() {
            this.novel.Chapters += 1
            if (this.$route.name === 'EditChapter') {
                var url = `https://localhost:44367/api/BookChapters/ChangeStatus?status=1`
                await axios.put(url, this.chapter, {
                    headers: {
                        'Content-Type': 'application/json-patch+json'
                    }
                }).then(response => {
                    this.$route.push(`/dashboard/workspace/${this.getNovel.BookId}`)
                })
                    .catch(error => {
                        console.log(error)
                    })
            } else {
                this.chapter.Status = '1'
                var url = `https://localhost:44367/api/BookChapters/add`
                await axios.post(url, this.chapter, {
                    headers: {
                        'Content-Type': 'application/json-patch+json'
                    }
                }).then(response => {
                    this.updateNovel()
                })
                    .catch(error => {
                        console.error(error)
                    })
            }

        },
        // hàm này đang bị sai logic
        async updateNovel() {
            var url = `https://localhost:44367/api/BookInfo/update`
            await axios.put(url, this.novelDetails, {
                headers: {
                    'Content-Type': 'application/json-patch+json'
                }
            }).then(response => {
                this.$route.push(`/dashboard/workspace/${this.getNovel.BookId}`)
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
                <quill-editor toolbar="essential" contentType="text" v-model:content="chapterContent"></quill-editor>
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