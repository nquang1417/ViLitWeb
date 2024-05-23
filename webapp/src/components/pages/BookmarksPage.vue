<script>
import { inject } from 'vue'
import { mapActions, mapGetters } from 'vuex'
import dayjs from "dayjs"
import { ElMessage } from 'element-plus'
import { ro } from 'element-plus/es/locale/index.mjs'
export default {
    name: 'BookmarksPage',
    data() {
        return {
            novels: [],
            totalItems: 0,
            currentPage: 1,
            pageSize: 5,
            bookmarks: [],            
            focusingNovel: {}
        }
    },
    mounted() {
        this.$api = inject('$api')
        this.getNovelsHasBookmarks(1)
        this.getBookmarks()
    },
    computed: {
        ...mapGetters('auth', {
            getUser: 'getAuthData',
            getAuthStatus: 'getLoginStatus'
        }),
    },
    methods: {
        ...mapActions('novel', {
            saveNovel: 'updateNovel',
            selectChapter: 'updateChapter'
        }),
        async getBookmarks() {
            var userId = this.getUser.userId
            var token = this.getUser.token
            await this.$api.bookmarks.getAll(userId, token) 
                .then(response => {
                    this.bookmarks = response.data
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async getNovelsHasBookmarks(pageNo) {
            var userId = this.getUser.userId
            var token = this.getUser.token
            await this.$api.novels.getBookmarks(userId, pageNo,token)
                .then(response => {
                    this.novels = JSON.parse(JSON.stringify(response.data.data.map(item => {
                        item.dropdown = false
                        return item
                    })))
                    this.totalItems = response.data.totals
                })
                .catch(error => {
                    console.error(error)
                })
        },
        viewChapter(row, col) {
            if (col.no != 1) {
                this.saveNovel(this.focusingNovel)
                this.selectChapter({ chapterId: row.chapterId, chapterTitle: row.chapterTitle, chapterNum: row.chapterNum })
                var url = `/${this.focusingNovel.bookTitle}/Chuong-${row.chapterNum}`
                this.$router.push(url)
            }
            
        },
        filterBookmarks(bookId) {
            return this.bookmarks.filter(bookmark => bookmark.bookId == bookId)
        },
        formatDate(row) {
            var date = dayjs(row.createDate).format("DD/MM/YYYY")
            return date
        },
        handleDropdown(novel) {
            novel.dropdown = !novel.dropdown
            this.focusingNovel = novel            
        },
        mouseEnter(row) {
            row.hovered = true
        },
        mouseLeave(row) {
            row.hovered = false
        },  
        async handleDelete(index, row) {
            var token = this.getUser.token
            await this.$api.bookmarks.delete(row.bookmarkId, token) 
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({message: 'Xóa đánh dấu thành công.', type: 'success', duration: 1500})
                        if (this.filterBookmarks(row.bookId).length == 1) {                            
                            this.novels = this.novels.filter(novel => novel.bookId != row.bookId)                            
                        }
                        this.bookmarks = this.bookmarks.filter(bm => bm.bookmarkId != row.bookmarkId)
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        handleCurrentChange(val) {
            this.getNovelsHasBookmarks(val)
        }
    },
}
</script>

<template>
    <bookshelf-layout>
        <el-container class="bookmarks-showcase">
            <el-header>
                <!-- <span class="number">{{ totalItems }}</span> -->
                <span class="showcase-title">Danh sách đánh dấu</span>
            </el-header>
            <el-main>
                <span v-if="this.novels.length <= 0">Không có đánh dấu nào</span>
                <el-row :gutter="15" v-else v-for="novel in novels" :key="novel.bookId">
                    <div class="book-wrapper">
                        <div class="book-cover">
                            <el-image :src="novel.bookCover" :fit="'cover'"></el-image>
                        </div>
                        <div class="book-title" @click="this.$router.push(`/novel/${novel.bookId}`)">{{ novel.bookTitle
                            }}</div>
                        <div class="flex-grow"></div>
                        <div class="expand-btn" @click="handleDropdown(novel)">
                            <i class="fa-solid fa-angle-down"></i>
                        </div>
                    </div>
                    <div class="bookmarks-list" v-if="novel.dropdown">
                        <el-table :data="filterBookmarks(novel.bookId)" :show-header="false" @row-click="viewChapter"
                            @cell-mouse-enter="mouseEnter" @cell-mouse-leave="mouseLeave">
                            <el-table-column prop="chapterTitle" label="Title" />
                            <!-- <el-table-column prop="createDate" label="Date" width="120" :formatter="formatDate" /> -->

                            <el-table-column prop="createDate" label="Date" width="180px"  resizable :formatter="formatDate" align="right">
                                <template #default="scope">
                                    <div class="delete-options"  v-if="scope.row.hovered">
                                        <el-button size="small" type="danger" plain
                                            @click="handleDelete(scope.$index, scope.row)">Delete</el-button>
                                        <div> {{ formatDate(scope.row) }}</div>
                                    </div>
                                </template>
                            </el-table-column>
                        </el-table>
                    </div>
                </el-row>
            </el-main>
            <el-footer height="40px">
                <div class="flex-grow"></div>
                <el-pagination v-model:current-page="currentPage" v-model:pageSize="pageSize" layout="prev, pager, next"
                    :total="totalItems" background @current-change="handleCurrentChange">
                </el-pagination>
            </el-footer>
        </el-container>
    </bookshelf-layout>
</template>

<style scoped>
:deep(.el-pagination *) {
    border-radius: 30px;
}
.bookmarks-showcase  {
    border: 1px solid #ccc;
    border-radius: 6px;
    overflow: hidden;
    background-color: #f0f2f5 !important;    
}


.bookmarks-showcase>.el-header {
    height: 36px;
    /* border-bottom: 3px solid #000; */
    border-bottom: 1px solid #ccc;
    font-family: 'Roboto Bold';
    align-content: center;
    font-size: 18px;
    padding: 0;
    padding-left: 10px;
    /* background-color: #f0f2f5; */
    border-radius: 6px 6px 0 0;

}

.bookmarks-showcase>.el-main {
    padding: 15px;
    background-color: #fff !important;
    /* border-left: 1px solid #ccc;
    border-right: 1px solid #ccc; */
}

.bookmarks-showcase>.el-footer {
    padding: 4px;
    width: 100%;
    display: flex;
    /* background-color: #f0f2f5; */
    border-radius: 0 0 6px 6px;
    /* border: 1px solid #ccc; */
}

.bookmarks-showcase>.el-main>.el-row {
    width: 100%;
    margin-left: 0 !important;
    margin-right: 0 !important;
    border-bottom: 1px solid #ccc;
    padding-bottom: 10px;
    margin-bottom: 10px;
}

.bookmarks-showcase>.el-header>.number {
    display: inline-block;
    background-color: #000;
    color: #fff;
    height: 36px;
    width: 36px;
    line-height: 36px;
    text-align: center;
    margin-right: 10px;
}

.book-wrapper {
    max-height: 80px;
    display: flex;
    gap: 10px;
    width: 100%;
    
}



.flex-grow {    
    flex-grow: 1;    
}



.book-wrapper>.book-cover>.el-image {
    height: 80px;           
}

.book-wrapper>.book-title {
    line-height: 80px;
    font-family: 'Roboto Medium';
}

.book-wrapper>.book-title:hover{
    cursor: pointer;
    color: #409eff;
}

.book-wrapper>.expand-btn {
    width: 32px;
    line-height: 80px;
}

.book-wrapper>.expand-btn:hover {
    cursor: pointer;
}

.bookmarks-list {
    width: 100%;
}

.delete-options {
    display: flex;
    justify-content: space-between;
}

:deep(.el-table__row:hover) {
    cursor: pointer;
}
</style>