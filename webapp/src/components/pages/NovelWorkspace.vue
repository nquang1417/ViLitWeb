<script lang="js">
import axios from 'axios'
import dayjs from "dayjs"
import { mapActions, mapGetters } from 'vuex'
import { getRemainingTime, getBookStatusStr } from '../../scripts/utils/utils'
import { inject } from 'vue'

export default {
    name: 'NovelWorkspace',
    data() {
        return {
            novel: {},
            chapters: [],
            drafts: [],
            trashs: [],
            chapterCurrentPage: 1,
            draftsCurrentPage: 1,
            trashcurrentPage: 1,
            totalChapters: 0,
            totalDrafts: 0,
            totalTrashs: 0,
            pageSize: 20,
            // totalPage: 10,
            totalItems: 0,
            lastUpdate: '',
            novelStatus: '',
            activeName: 'drafts',
            dialogFormVisible: false,
            loading: false,
            isEdited: false,
            rowHover: false,

        }
    },
    mounted() {
        this.loadNovel().then(() => {
            this.loadDrafts(1)
        })
    },
    props: ['novelId'],
    computed: {
        ...mapGetters('novel', {
            getNovel: 'getNovelInfo'
        }),
        ...mapGetters('auth', {
            getterLoginStatus: 'getLoginStatus',
            gettersAuthData: 'getAuthData'
        }),
        $api() {
            return inject('$api')
        }
    },
    watch: {
    },
    methods: {
        ...mapActions('novel', {
            saveNovel: 'updateNovel',
            selectChapter: 'updateChapter'
        }),
        async loadNovel() {
            await this.$api.novels.getDetails(this.novelId)
                .then(response => {
                    this.novel = JSON.parse(JSON.stringify(response.data))
                    this.totalItems = this.novel.chapters
                })
                .catch(e => {
                    console.error(e);
                })
            this.lastUpdate = getRemainingTime(this.novel.updateDate)
            this.novelStatus = getBookStatusStr(this.novel.status)
            this.novel.CoverUrl = this.novel.bookCover            
            this.saveNovel(this.novel)
        },
        async loadChapters(page) {
            this.loading = true
            await this.$api.chapters.getChapters(this.novelId, page)
                .then(response => {
                    if (response.status == 200) {
                        this.chapters = JSON.parse(JSON.stringify(response.data.data.map(item => {
                            item.updateDate = dayjs(item.updateDate).format("DD/MM/YYYY HH:mm");
                            item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm");
                            return item;
                        })));
                        this.totalChapters = response.data.totals
                    }
                })
                .catch(e => {
                    console.error(e);
                })
            this.loading = false
        },
        async loadDrafts(page) {
            this.loading = true            
            await this.$api.chapters.getDrafts(this.novelId, page, this.gettersAuthData.token, this.novel.uploaderId)
                .then(response => {
                    if (response.status == 200) {
                        this.drafts = JSON.parse(JSON.stringify(response.data.data.map(item => {
                            item.updateDate = dayjs(item.updateDate).format("DD/MM/YYYY HH:mm");
                            item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm");
                            return item;
                        })));
                        this.totalDrafts = response.data.totals
                    }

                }).catch(e => {
                    console.error(e);
                })
            this.loading = false
        },
        async loadDeleteds(page) {
            this.loading = true   
            await this.$api.chapters.getDeleteds(this.novelId, page, this.gettersAuthData.token, this.novel.uploaderId)
                .then(response => {
                    if (response.status == 200) {
                        this.trashs = JSON.parse(JSON.stringify(response.data.data.map(item => {
                            item.updateDate = dayjs(item.updateDate).format("DD/MM/YYYY HH:mm");
                            item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm");
                            return item;
                        })));
                        this.totalTrashs = response.data.totals
                    }

                }).catch(e => {
                    console.error(e);
                })
            this.loading = false
        },
        async update(entity) {
            await this.$api.novels.updateNovel(entity.bookId, entity, this.gettersAuthData.token, entity.uploaderId)
                .then(() => {
                    this.saveNovel(entity)
                })
                .catch((error) => {
                    console.error(error)
                })            
        },
        async changeStatus(entity, status) {
            var token = this.gettersAuthData.token
            var owner = this.novel.uploaderId
            await this.$api.chapters.changeStatus(entity.chapterId, status, token, owner)
                .then((response) => { return response})
                .catch((error) => {
                    console.error(error)
                })
        },

        //==== Pagination Publish tab
        handleChapterChange(val) {
            this.loadChapters(val)
        },
        handleChapterRowClick(row, col) {
            if (col.no != 2) {
                var url = `/dashboard/edit-chapter/${row.chapterId}`
                this.selectChapter({ chapterId: row.chapterId, chapterTitle: row.chapterTitle, chapterNum: row.chapterNum })
                this.$router.push(url);
            }
        },
        handleChapterEdit(index, row) {
            this.selectChapter({ chapterId: row.chapterId, chapterTitle: row.chapterTitle, chapterNum: row.chapterNum })
            this.$router.push(`/dashboard/edit-chapter/${row.chapterId}`)
        },
        handleChapterDelete(index, row) {
            this.novel.chapters--
            this.changeStatus(row, 2).then(() => {
                if (this.trashs.length > 0) {
                    this.loadDeleteds(1)
                }
                this.update(this.novel)
            })
            this.chapters = this.chapters.filter(chapter => chapter.chapterId != row.chapterId)
        },

        //====
        //==== Pagination Drafts tab
        handleDraftsChange(val) {
            this.loadDrafts(val)
        },
        handleDraftsRowClick(row, col) {            
            if (col.no != 2) {
                var url = `/dashboard/edit-chapter/${row.chapterId}`
                this.selectChapter({ chapterId: row.chapterId, chapterTitle: row.chapterTitle, chapterNum: row.chapterNum })
                this.$router.push(url);
            }
        },
        handleDraftsPublish(index, row) {
            this.selectChapter({ chapterId: row.chapterId, chapterTitle: row.chapterTitle, chapterNum: row.chapterNum })
            this.$router.push(`/dashboard/edit-chapter/${row.chapterId}`)
        },
        handleDraftsDelete(index, row) {
            this.changeStatus(row, 2).then(() => {
                if (this.trashs.length > 0) {
                    this.loadDeleteds(1)
                }
            })
            this.drafts = this.drafts.filter(draft => draft.chapterId != row.chapterId)            
        },
        //====
        //==== Pagination Drafts tab
        handleTrashsChange(val) {
            this.loadDrafts(val)
        },
        handleTrashsRowClick(row, col) {
            if (col.no != 2) {
                var url = `/dashboard/edit-chapter/${row.chapterId}`
                this.selectChapter({ chapterId: row.chapterId, chapterTitle: row.chapterTitle, chapterNum: row.chapterNum })
                this.$router.push(url);
            }
        },
        handleTrashsRestore(index, row) {
            this.changeStatus(row, 0).then(() => {
                if (this.drafts.length > 0) {
                    this.loadDrafts(1)
                }
            })
            this.trashs = this.trashs.filter(trash => trash.chapterId != row.chapterId)
            
        },
        handleTrashsClear(index, row) {
            
        },
        //====
        handleClick(tab, event) {
            console.log(tab.paneName)
            if (tab.paneName === 'publish') {
                if (this.chapters.length == 0) {
                    this.loadChapters(1)
                }
            } else if (tab.paneName === 'drafts') {
                if (this.drafts.length == 0) {
                    this.loadDrafts(1)
                }
            } else if (tab.paneName === 'trash') {
                if (this.trashs.length == 0) {
                    this.loadDeleteds(1)
                }
            }       
        },
        beforeCloseDialog() {
            this.update(this.novel)
            // console.log(this.novel.Description)
            this.dialogFormVisible = false
        },
        openDialog() {
            this.dialogFormVisible = true
        },
        navigateEdit() {
            this.$router.push(`/dashboard/edit-novel/${this.novel.bookId}`)
        },
        
        newDraft() {
            this.$router.push(`/dashboard/${this.novel.bookTitle}/new-chapter`)
        },

        mouseEnter(row) {
            row.hovered = true
        },
        mouseLeave(row) {
            row.hovered = false
        }
    }
}
</script>

<template>
    <dashboard-layout>
        <template #header-content>Workspace</template>
        <template #dialog>
            <el-dialog v-model="dialogFormVisible" :show-close="false" :show-header="false"
                :before-close="beforeCloseDialog" close-on-press-escape>
                <el-input type="textarea" :autosize="{ minRows: 10 }" maxlength="1000" v-model="novel.description"
                    show-word-limit>
                </el-input>
                <!-- <template #footer>
                    <el-button type="primary">Lưu</el-button>
                </template> -->
            </el-dialog>
        </template>
        <template #header-extra>
            <el-button type="primary" @click="newDraft">Chương mới</el-button>
        </template>
        <el-container class="novel-details">
            <el-main>
                <el-container class="feature-section">
                    <el-main>
                        <div class="top-part">
                            <el-row :gutter="30">
                                <el-col :span="5" class="series-cover">
                                    <img :src="this.novel.CoverUrl">
                                </el-col>
                                <el-col :span="19" class="flex-col">
                                    <div class="series-info">
                                        <div class="series-title-group">
                                            <span class="series-title">{{ novel.bookTitle }}</span>
                                            <span style="float: right"><el-icon class="edit-icon" @click="navigateEdit">
                                                    <Edit />
                                                </el-icon></span>
                                        </div>

                                        <span class="info-item">
                                            <span class="info-name">Tác giả </span>
                                            <span class="info-value">{{ novel.authorName }}</span>
                                        </span>
                                        <span class="genre">
                                            <span class="info-name"> / </span>
                                            <span class="info-name"> Thể loại</span>
                                            <span class="info-value">{{ novel.genreName }}</span>
                                        </span>
                                    </div>
                                    <div class="description">
                                        <div class="block font-bold">Tóm Tắt
                                            <el-icon class="edit-icon icon14" @click="openDialog">
                                                <EditPen />
                                            </el-icon>
                                        </div>
                                        <span style="font-size: 15px;">{{ novel.description }}</span>
                                    </div>
                                    <div class="statistics">
                                        <el-row justify="space-between" :align="'middle'">
                                            <el-col class="statistics-item" :span="6" :align="'center'"
                                                @click="console.log('share')">
                                                <span class="block feature-name">Trạng thái</span>
                                                <span class="block feature-value font-bold">{{ novelStatus }}</span>
                                                <span class="block feature-value" style="font-size: 14px;">{{
                                                    novel.chapters }} chương</span>
                                            </el-col>
                                            <el-col class="statistics-item" :span="6" :align="'center'"
                                                @click="console.log('follow')">
                                                <span class="block feature-name">Lần cuối</span>
                                                <span class="block feature-value font-bold"> {{ this.lastUpdate
                                                    }}</span>
                                            </el-col>
                                            <el-col class="statistics-item" :span="6" :align="'center'"
                                                @click="console.log('navigate to index')">
                                                <span class="block feature-name">Đánh giá</span>
                                                <span class="block feature-value font-bold">{{ novel.averageRating }} /
                                                    {{
                                                    novel.reviews }}</span>
                                            </el-col>
                                            <el-col class="statistics-item" :span="6" :align="'center'"
                                                @click="console.log('share')">
                                                <span class="block feature-name">Lượt xem</span>
                                                <span class="block feature-value font-bold">{{ novel.views }}</span>
                                            </el-col>

                                        </el-row>
                                    </div>
                                </el-col>
                            </el-row>
                        </div>
                        <!-- <div class="bottom-part">
                            <el-row>
                                
                            </el-row>
                        </div> -->
                    </el-main>
                </el-container>
                <el-container class="basic-section">
                    <el-tabs v-model="activeName" class="demo-tabs" @tab-click="handleClick">
                        <el-tab-pane label="Bản nháp" name="drafts">
                            <el-container class="main-item">
                                <el-main class="chapter-index">
                                    <el-table :data="drafts" stripe style="width: 100%" :show-header="false"
                                        v-loading="loading" @row-click="handleDraftsRowClick"
                                        @cell-mouse-enter="mouseEnter" @cell-mouse-leave="mouseLeave"
                                        :default-sort="{ prop: 'chapterNum', order: 'ascending' }">
                                        <el-table-column prop="chapterNum"  label="STT" :align="'left'" sortable 
                                            width="80" />
                                        <el-table-column prop="chapterTitle" label="Tiêu đề" :align="'left'"
                                            min-width="200" />
                                        <el-table-column prop="updateDate" label="Ngày cập nhật" sortable :align="'right'">
                                            <template #default="scope">
                                                <div class="edit-options" v-if="scope.row.hovered">
                                                    <el-button size="small" plain
                                                        @click="handleDraftsPublish(scope.$index, scope.row)">Publish</el-button>
                                                    <el-button size="small" type="danger" plain
                                                        @click="handleDraftsDelete(scope.$index, scope.row)">Delete</el-button>
                                                </div>
                                            </template>
                                        </el-table-column>
                                        <template #empty>
                                            <div>Không có bản nháp nào</div>
                                            <el-button type="primary" @click="newDraft">Tạo bản nháp mới</el-button>
                                        </template>

                                    </el-table>
                                </el-main>
                                <el-footer class="pagination" height="40px">
                                    <el-pagination v-model:current-page="draftsCurrentPage" v-model:page-size="pageSize"
                                        :background="true" layout="prev, pager, next, jumper" :total="totalDrafts"
                                        @current-change="handleDraftsChange" />
                                </el-footer>
                            </el-container>
                        </el-tab-pane>

                        <el-tab-pane label="Xuất bản" name="publish">
                            <el-container class="main-item">
                                <el-main class="chapter-index">
                                    <el-table :data="chapters" stripe style="width: 100%" :show-header="false"
                                        v-loading="loading" @row-click="handleChapterRowClick"
                                        row-class-name="chapter-row" @cell-mouse-enter="mouseEnter"
                                        @cell-mouse-leave="mouseLeave" 
                                        :default-sort="{ prop: 'chapterNum', order: 'ascending' }">
                                        <el-table-column prop="chapterNum"  label="STT" :align="'left'" sortable 
                                            width="80" />
                                        <el-table-column prop="chapterTitle" label="Tiêu đề" :align="'left'"
                                            min-width="200" />
                                        <el-table-column prop="updateDate" label="Ngày cập nhật" :align="'right'" sortable>
                                            <template #default="scope">
                                                <div class="edit-options" v-if="scope.row.hovered">
                                                    <el-button size="small" plain
                                                        @click="handleChapterEdit(scope.$index, scope.row)">Edit</el-button>
                                                    <el-button size="small" type="danger" plain
                                                        @click="handleChapterDelete(scope.$index, scope.row)">Delete</el-button>
                                                </div>
                                            </template>
                                        </el-table-column>
                                        <template #empty>
                                            <div>Không có chương nào</div>
                                        </template>
                                    </el-table>
                                </el-main>
                                <el-footer class="pagination" height="40px">
                                    <!-- <el-button>Add</el-button> -->
                                    <el-pagination v-model:current-page="chapterCurrentPage"
                                        v-model:page-size="pageSize" :background="true"
                                        layout="prev, pager, next, jumper" :total="totalChapters"
                                        @current-change="handleChapterChange" />
                                </el-footer>
                            </el-container>
                        </el-tab-pane>

                        <el-tab-pane label="Thùng rác" name="trash">
                            <el-container class="main-item">
                                <el-main class="chapter-index">
                                    <el-table :data="trashs" stripe style="width: 100%" :show-header="false"
                                        v-loading="loading" @row-click="handleChapterRowClick"
                                        @cell-mouse-enter="mouseEnter" @cell-mouse-leave="mouseLeave"
                                        :default-sort="{ prop: 'chapterNum', order: 'ascending' }">
                                        <el-table-column prop="chapterNum"  label="STT" :align="'left'" sortable 
                                            width="80" />
                                        <el-table-column prop="chapterTitle" label="Tiêu đề" :align="'left'"
                                            min-width="200" />
                                        <el-table-column prop="updateDate" label="Ngày cập nhật" :align="'right'" sortable>
                                            <template #default="scope">
                                                <div class="edit-options" v-if="scope.row.hovered">
                                                    <el-button size="small" plain
                                                        @click="handleChapterEdit(scope.$index, scope.row)">Edit</el-button>
                                                    <el-button size="small" type="primary" plain
                                                        @click="handleTrashsRestore(scope.$index, scope.row)">Restore</el-button>
                                                </div>
                                            </template>
                                        </el-table-column>
                                        <template #empty>
                                            Thùng rác rỗng
                                        </template>
                                    </el-table>
                                </el-main>
                                <el-footer class="pagination" height="40px">
                                    <el-pagination v-model:current-page="trashcurrentPage" v-model:page-size="pageSize"
                                        :background="true" layout="prev, pager, next, jumper" :total="totalTrashs"
                                        @current-change="handleTrashsChange" />
                                </el-footer>
                            </el-container>
                        </el-tab-pane>
                    </el-tabs>

                </el-container>
            </el-main>
        </el-container>
    </dashboard-layout>
</template>

<style scoped>
.novel-details {
    border: 1px solid #ccc;
    border-radius: 6px;
    margin: 20px;
}

.block {
    display: block;
}


.feature-section {
    /* padding: 15px; */
    border: 1px solid #ccc;
    border-radius: 6px;
    height: max-content;
    width: 100%;
}

.feature-section .el-main {
    padding: 0;
}

.top-part {
    padding: 10px;
}

.bottom-part {
    padding: 0;
}


.basic-section {
    border-radius: 6px;
    margin-top: 20px;
    flex-direction: column;
}

.series-cover {
    /* max-width: 25%; */
    padding: 0 !important;
    background-image: url(https://biotrop.org/images/default-book.png);
    background-size: 100% 100%;
    background-repeat: no-repeat;
    background-position-y: top;
    background-position-x: center;
    aspect-ratio: 50/71;
}

.series-cover img {
    width: 100%;
    height: 100%;
}

.flex-col {
    display: flex;
    flex-direction: column;
}

/* 
.series-info {
    flex: 1;
} */

.series-title-group {
    margin-bottom: 10px;
}

.series-title {
    font-size: 1.625rem;
    font-weight: 700;
    line-height: 2.125rem;
}

.info-value {
    /* text-decoration: wavy; */
    margin-left: 5px;
    margin-right: 5px;

}

.info-name {
    color: #aaa;
    margin-right: 5px;
}

.feature-item:hover {
    cursor: pointer;
    opacity: 0.7;
}

.block {
    display: block;
}

.feature-value {
    font-size: 22px;
    /* font-weight: bold; */
}

.statistics-item .feature-name {
    font-size: 15px;
    color: #0000006c;
}

.statistics-item .feature-value {
    font-size: 18px;
}

.font-bold {
    font-family: 'Roboto Bold';
}


.statistics {
    /* border-top: 1px solid #ccc;
    border-bottom: 1px solid #ccc; */
    padding: 10px 0;
}

.description {
    padding: 10px 0;
    width: 100%;
    text-align: justify;
    word-wrap: break-word;
    flex: 1;

}


.description span {
    text-overflow: ellipsis;
    white-space: pre-wrap;
}

.main-item {
    margin-bottom: 10px;
    border: 1px solid #ccc;
    border-radius: 6px;
}

.main-item .el-footer {
    display: flex;
    justify-content: space-between;
}



.chapter-index {
    padding: 0;
    border-radius: 5px 5px 0 0;
}


.pagination {
    border-top: 1px solid #ccc;
    padding: 4px;
    background-color: #f0f2f5;
    border-radius: 0 0 6px 6px;
}

:deep(.el-table__row:hover) {
    cursor: pointer;
}

:deep(.el-dialog) {
    border-radius: 6px;
}

:deep(.el-dialog__header) {
    display: none;
}

:deep(.el-dialog__body) {
    padding: 10px;
}

:deep(.el-dialog__footer) {
    padding: 10px;
}


.el-row {
    margin: 0 !important;
}

.edit-icon {
    margin-left: 10px;
}

.icon16 {
    font-size: 16px;
}

.icon14 {
    font-size: 14px;
}

.edit-icon:hover {
    color: #0000006c;
    cursor: pointer;
}

</style>