<script lang="js">
import axios from 'axios'
import dayjs from "dayjs"
import { mapActions, mapGetters } from 'vuex'
import { getRemainingTime, removeVietnameseTones } from '../../scripts/utils/utils'
import { inject } from 'vue'
import { ElMessage } from 'element-plus'

export default {
    name: 'NovelDetails',
    data() {
        return {
            novel: {},
            chapters: [],
            reviews: [],
            currentPage: 1,
            pageSize: 20,
            // totalPage: 10,
            totalItems: 0,
            lastUpdate: '',
            uploader: {},
            uploaderAvaUrl: '',
            newReviews: [],
            reportFormVisible: false,
            reportData: {},
            following: false,
        }
    },
    mounted() {
        this.$api = inject('$api')
        this.loadNovel()        
    },
    computed: {
        id() {
            return this.$route.params.novelId
        },
        ...mapGetters('novel', {
            getNovel: 'getNovelInfo'
        }),
        ...mapGetters('auth', {
            getLoginStatus: 'getLoginStatus',
            getUser: 'getAuthData'
        }),
        
    },
    watch: {
    },
    methods: {
        ...mapActions('novel', {
            saveNovel: 'updateNovel',
            selectChapter: 'updateChapter',
            setGenre: 'commitGenre'
        }),
        formatDate(date) {
            var date = dayjs(date).format("DD/MM/YYYY HH:mm:ss")
            return date
        },
        async loadUploaderInfo() {
            await this.$api.users.getUser(this.novel.uploaderId)
                .then(response => {
                    this.uploader = response.data
                    this.uploaderAvaUrl = response.data.avatar
                    this.uploader.createDate = dayjs(this.uploader.spdateDate).format("DD/MM/YYYY");
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async loadNovel() {
            var url = `http://localhost:10454/api/BookInfo/details?bookId=${this.id}`
            await axios.get(url)
                .then(response => {
                    this.novel = response.data
                    switch (this.novel.status) {
                        case 0:
                            this.novel.statusStr = "Tạm ngưng"
                            break;
                        case 1:
                            this.novel.statusStr = "Đang tiến hành"
                            break;
                        case 2:
                            this.novel.statusStr = "Hoàn thành"
                            break;
                        case 3:
                            this.novel.statusStr = "Tạm khóa"
                            break
                    }
                    this.totalItems = this.novel.chapters
                    this.novel.CoverUrl = this.novel.bookCover != '' ? this.novel.bookCover : 'https://biotrop.org/images/default-book.png';
                    this.loadUploaderInfo()
                    if (this.getLoginStatus) {
                        this.loadFollowingStatus()
                    }
                    if (this.novel.status != 3) {
                        this.loadChapters(1)
                    }
                })
                .catch(e => {
                    console.error(e);
                })
            this.lastUpdate = getRemainingTime(this.novel.updateDate)            
            this.saveNovel(this.novel)
        },
        async loadChapters(page) {
            var url = `http://localhost:10454/api/BookChapters/get-chapters?bookId=${this.id}&page=${page}`
            await axios.get(url)
                .then(response => {
                    this.chapters = JSON.parse(JSON.stringify(response.data.data.map(item => {
                        item.updateDate = dayjs(item.spdateDate).format("DD/MM/YYYY HH:mm");
                        item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm");
                        return item;
                    })));
                })
                .catch(e => {
                    console.error(e);
                })
        },
        async loadFollowingStatus(){
            var userId = this.getUser.userId
            var bookId = this.novel.bookId
            var token = this.getUser.token
            await this.$api.reactions.getFollowingStatus(userId, bookId, token)
                .then(response => {
                    if (response.status == 200) {
                        this.following = response.data
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async handleFollowClick() {
            if (!this.getLoginStatus) {
                ElMessage({ message: 'Yêu cầu đăng nhập để thực hiện chức năng này', type: 'warning', duration: 1500 })
            } else {
                if (this.novel.status != 3) {
                    var userId = this.getUser.userId
                    var bookId = this.novel.bookId
                    var token = this.getUser.token
                    var currentStatus = this.following
                    await this.$api.reactions.followBook(userId, bookId, token)
                        .then(response => {
                            if (response.status == 200) {
                                if (currentStatus == true) {
                                    ElMessage({ message: 'Hủy theo dõi tryện thành công.', type: 'success', duration: 1500 })

                                    this.novel.followers--
                                    var stats = {
                                        bookId: this.novel.bookId,
                                        views: this.novel.views,
                                        followers: this.novel.followers,
                                        comments: this.novel.comments,
                                        reviews: this.novel.reviews,
                                        averageRating: this.novel.averageRating
                                    }
                                    this.updateNovelStats(stats)
                                } else {
                                    ElMessage({ message: 'Theo dõi truyện thành công', type: 'success', duration: 1500 })

                                    this.novel.followers++
                                    var stats = {
                                        bookId: this.novel.bookId,
                                        views: this.novel.views,
                                        followers: this.novel.followers,
                                        comments: this.novel.comments,
                                        reviews: this.novel.reviews,
                                        averageRating: this.novel.averageRating
                                    }
                                    this.updateNovelStats(stats)
                                }
                                this.following = !this.following
                            }
                        })
                        .catch(error => {
                            console.error(error)
                        })
                } else {
                    ElMessage({ message: 'Truyện đang bị khóa', type: 'error', duration: 1500 })

                }
            }
        },
        async updateNovelStats(stats) {
            await this.$api.novels.updateStat(stats)
                .catch(error => {
                    console.error(error)
                })
        },
        handleRatingCLick() {
            var title = removeVietnameseTones(this.getNovel.bookTitle).replaceAll(" ", "-")
            this.$router.push(`/novel/${title}/review`)
        },
        handleCurrentChange(val) {
            this.loadChapters(val)
        },
        handleRowClick(row) {
            console.log(row.chapterId)
            var chapterTitle = `Chuong-${row.chapterNum}`
            var novelName = this.novel.bookTitle
            // var url = `/${this.novel.BookId}/${novelName}/${chatperId}/${chapterTitle}`
            var url = `/${novelName}/${chapterTitle}`
            this.selectChapter({ chapterId: row.chapterId, chapterTitle: row.chapterTitle, chapterNum: row.chapterNum })
            this.$router.push(url);
        },
        handleAvaError() {
            this.uploaderAvaUrl = 'https://i0.wp.com/sbcf.fr/wp-content/uploads/2018/03/sbcf-default-avatar.png'
        },
        handleReportClick() {
            if (!this.getLoginStatus) {
                ElMessage({message: 'Yêu cầu đăng nhập trước khi thực hiện chức năng này', type: 'warning', duration: 1500})
            } else {                
                this.reportFormVisible = true
                this.reportData = {}
                this.reportData.senderId = this.getUser.userId
                this.reportData.targetType = 1
                this.reportData.reportedType = 1
                this.reportData.reportedEntityId = this.novel.bookId
                this.reportData.createBy = this.getUser.userId
                this.reportData.updateBy = this.getUser.userId                
            }            
        },
        async send() {
            var token = this.getUser.token
            await this.$api.reports.add(this.reportData, token)
                .then(response => {
                    if (response.status == 201) {
                        ElMessage({message: 'Gửi báo cáo thành công.', type: 'success', duration: 1500})
                        this.reportData = {}
                    }
                    this.reportFormVisible = false
                })
        },
        breadcrumbClick() {
            this.setGenre({genreId: this.novel.genreId, genreName: this.novel.genreName})
        }
    },
}
</script>

<template>
    <layout-default>
        <template #dialog>
            <el-dialog v-model="reportFormVisible" title="Báo cáo tiểu thuyết" width="40%">
                <report-form :data="reportData" @update:data="(formdata) => {this.reportData = formdata}" :reportType=1></report-form>
                <template #footer>
                    <div class="dialog-footer">
                        <el-button type="primary" @click="send">Gửi</el-button>
                        <el-button @click="this.reportFormVisible = false">Hủy</el-button>                        
                    </div>
                </template>
            </el-dialog>
        </template>
        <el-breadcrumb class="breadcrumb" separator="/">
            <el-breadcrumb-item :to="{ path: '/' }"><i class="fa-solid fa-house"></i></el-breadcrumb-item>
            <el-breadcrumb-item :to="{ path: '/genre', query: {name: novel.genreName} }" @click="breadcrumbClick">{{ novel.genreName }}</el-breadcrumb-item>
            <el-breadcrumb-item>{{ novel.bookTitle }}</el-breadcrumb-item>
        </el-breadcrumb>
        <el-container>
            <el-row class="novel-details" :gutter="20">
                <el-col :span="16">
                    <el-container class="feature-section">
                        <el-main>
                            <div class="top-part">
                                <el-row :gutter="16">
                                    <el-col :span="7" class="series-cover">
                                        <img :src="this.novel.CoverUrl">
                                    </el-col>
                                    <el-col :span="16" class="flex-col">
                                        <div class="series-info">
                                            <div class="series-title-group">
                                                <span class="series-title">{{ novel.bookTitle }}</span>
                                            </div>
                                            <div class="genre">
                                                <span class="series-genre">{{ novel.genreName }}</span>
                                            </div>
                                            <div>
                                                <span class="info-name"
                                                    style="font-family: 'Roboto Medium'; font-size: 18px;">Tác giả:
                                                </span>
                                                <span class="info-value" style="font-size: 18px;"><a href="#">{{
                                                        novel.authorName }}</a></span>
                                            </div>
                                            <div>
                                                <span class="info-name" style="font-family: 'Roboto Italic';">Tình
                                                    trạng: </span>
                                                <span class="info-value"
                                                    style="font-family: 'Roboto Italic'; color: #aaa">{{ novel.statusStr
                                                    }}<a href="#">
                                                    </a></span>
                                            </div>
                                            <div class="lockTag" v-if = "this.novel.status == 3">
                                                <el-tag type="danger" size="large" effect="dark" >Truyện đang bị khóa</el-tag>
                                                <div style="color: #f56c6c">Truyện bị khóa tới ngày <span style="font-family: 'Roboto Italic';">{{ formatDate(this.novel.lockedExpired) }}</span></div>
                                                <div style="color: #f56c6c">Lý do: {{ this.novel.lockedReason }}</div>
                                            </div>

                                        </div>
                                        <div class="side-features">
                                            <el-row justify="space-between" :align="'middle'">
                                                <el-col class="feature-item" :span="6" :align="'center'"
                                                    @click="handleFollowClick">
                                                    <span class="block feature-value">
                                                        <i class="fa-regular fa-heart" v-if="!this.following"></i>
                                                        <i class="fa-solid fa-heart" v-if="this.following"></i>
                                                    </span>
                                                    <span class="block feature-name">{{ novel.followers }}</span>
                                                </el-col>
                                                <el-col class="feature-item" :span="6" :align="'center'"
                                                    @click="handleRatingCLick">
                                                    <span class="block feature-value">
                                                        <i class="fa-regular fa-star"></i>
                                                    </span>
                                                    <span class="block feature-name">{{ novel.averageRating }}</span>
                                                </el-col>
                                                <el-col class="feature-item" :span="6" :align="'center'"
                                                    @click="this.$refs.index.$el.scrollIntoView({behavior: 'smooth'})">
                                                    <span class="block feature-value"><i
                                                            class="fa-solid fa-list"></i></span>
                                                    <span class="block feature-name">Mục lục</span>
                                                </el-col>
                                                <!-- <el-col class="feature-item" :span="6" :align="'center'"
                                                    @click="console.log('share')">
                                                    <span class="block feature-value"><i
                                                            class="fa-solid fa-share-nodes"></i></span>
                                                    <span class="block feature-name">Chia sẻ</span>
                                                </el-col> -->
                                            </el-row>
                                        </div>
                                    </el-col>
                                    <el-col :span="1" style="padding: 0; ">
                                        <el-dropdown class="noti-dropdown" trigger="click">
                                            <div class="options-btn">
                                                <i class="fa-solid fa-ellipsis"></i>                                                
                                            </div>
                                            <template #dropdown>
                                                <el-dropdown-menu>
                                                    <el-dropdown-item @click="handleReportClick">Báo cáo vấn đề</el-dropdown-item>
                                                </el-dropdown-menu>
                                            </template>
                                        </el-dropdown>
                                    </el-col>
                                </el-row>
                            </div>
                            <div class="bottom-part">
                                <div class="statistics">
                                    <el-row justify="space-between" :align="'middle'">
                                        <el-col class="statistics-item" :span="6" :align="'center'">
                                            <span class="block feature-name">Lần cuối</span>
                                            <span class="block feature-value font-bold"> {{ this.lastUpdate }}</span>
                                        </el-col>

                                        <el-col class="statistics-item" :span="6" :align="'center'">
                                            <span class="block feature-name">Đánh giá</span>
                                            <span class="block feature-value font-bold">{{ novel.averageRating}} / {{
                                                novel.reviews }}</span>
                                        </el-col>
                                        <el-col class="statistics-item" :span="6" :align="'center'">
                                            <span class="block feature-name">Lượt xem</span>
                                            <span class="block feature-value font-bold">{{ novel.views }}</span>
                                        </el-col>
                                    </el-row>
                                </div>
                                <el-row>
                                    <div class="description">
                                        <div class="block font-bold">Tóm Tắt</div>
                                        <span style="font-size: 15px;">{{ novel.description }}</span>
                                    </div>
                                </el-row>
                            </div>
                        </el-main>
                    </el-container>
                </el-col>
                <el-col :span="8">
                    <el-container class="side-bar">
                        <div class="side-bar-item">
                            <div class="series-owner">
                                <div class="avatar">
                                    <el-image :src="this.uploaderAvaUrl" @error="handleAvaError" />
                                </div>
                                <div class="owner-name">{{ this.uploader.displayName }}</div>
                            </div>
                            <div class="owner-info">
                                <el-row :gutter="20">
                                    <el-col class="userinfo-item" :span="8">
                                        <span>Truyện</span>
                                        <span>{{ this.uploader.uploadItems }}</span>
                                    </el-col>
                                    <el-col class="userinfo-item" :span="8">
                                        <span>Bình luận</span>
                                        <span>{{ this.uploader.comments }}</span>
                                    </el-col>
                                    <el-col class="userinfo-item" :span="8">
                                        <span>Tham gia</span>
                                        <span>{{ this.uploader.createDate }}</span>
                                    </el-col>
                                </el-row>
                            </div>
                        </div>
                    </el-container>
                </el-col>
                <el-col :lg="16">
                    <el-container class="basic-section">
                        <!-- <el-container class="main-item">
                            <el-header>
                                <span>Reviews mới</span>
                                
                            </el-header>
                            <el-main class="review-group">
                                <div class="review-item">
                                    <div class="review-item__header">
                                        <div class="review-owner">username</div>
                                        <div class="review-rating">
                                            <el-rate v-model="this.novel.averageRating" :colors="colors" disabled />
                                        </div>
                                    </div>
                                    <div class="review-item__body">
                                        <div class="review-content">review content</div>
                                        upload time
                                    </div>
                                </div>
                                <div class="review-item">
                                    <div class="review-item__header">
                                        <div class="review-owner">username</div>
                                        <div class="review-rating">
                                            <el-rate v-model="this.novel.averageRating" :colors="colors" disabled />
                                        </div>
                                    </div>
                                    <div class="review-item__body">
                                        <div class="review-content">review content</div>
                                        upload time
                                    </div>
                                </div>
                            </el-main>
                        </el-container> -->
                        <el-container class="main-item" ref="index">
                            <el-header>
                                <span>Mục lục</span>
                            </el-header>
                            <el-main class="chapter-index">
                                <el-table :data="chapters" stripe style="width: 100%" :show-header="false"
                                    @row-click="handleRowClick">
                                    <el-table-column prop="chapterTitle" label="Title" :align="'left'"
                                        min-width="200" />
                                    <el-table-column prop="updateDate" label="Update" :align="'right'" />
                                </el-table>
                            </el-main>
                            <el-footer class="pagination" height="40px">
                                <el-pagination v-model:current-page="currentPage" v-model:page-size="pageSize"
                                    :background="true" layout="prev, pager, next, jumper" :total="totalItems"
                                    @current-change="handleCurrentChange" />
                            </el-footer>
                        </el-container>
                    </el-container>
                </el-col>
            </el-row>
        </el-container>
    </layout-default>
</template>
<style scoped>
.block {
    display: block;
}

.novel-details {
    padding-top: 20px;
    width: 100%;
}

.feature-section  * {
    background-color: #fff;
}

.feature-section {
    /* padding: 15px; */
    border: 1px solid #ccc;
    border-radius: 6px;
    height: max-content;

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

.side-bar * {
    background-color: #fff;
    
}

.side-bar {
    /* border: 1px solid #ccc; */
    border-radius: 6px;
    flex-direction: column;
    /* background-color: #fff; */
}

.side-bar-item {
    margin-bottom: 20px;
    box-sizing: border-box;
    border: 1px solid #ccc;
    border-radius: 6px;
    background-color: #f2f4f5;
    overflow: hidden;

}

:deep(.el-pagination *) {
    border-radius: 30px;
}

.basic-section .el-main{
    background-color: #fff !important;
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

.series-info {
    flex: 1;
}

.series-title-group {
    margin-bottom: 10px;
}

.series-title {
    font-size: 1.625rem;
    font-family: 'Roboto Bold';
    line-height: 2.125rem;
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

.series-owner {
    display: flex;
    flex-direction: column;
    position: relative;
    width: 100%;
    align-items: center;
    border-radius: 5px 5px 0 0;
    padding: 15px 0;

}


.series-owner .owner-name {
    /* position: absolute; */
    color: #000;
    font-size: 18px;
}

.avatar {    
    width: 150px;
    height: 150px;
    margin-bottom: 10px;
    border-radius: 100px;
    overflow: hidden;
}

.owner-info {
    /* border-bottom: 4px solid #d9534f; */
    border-top: 1px solid #ccc;
    text-align: center;
}


.side-bar-item .el-header {
    border-radius: 5px 5px 0 0;
    background-color: #ddd;
    height: fit-content;
    padding: 10px 15px;
    font-family: 'Roboto Medium';
}

.font-bold {
    font-family: 'Roboto Bold';
}

.side-bar-item .el-main {
    padding: 15px;
    font-size: 14px;
}

.side-bar-item .el-main p {
    margin: 0;
}

.statistics {
    border-top: 1px solid #ccc;
    border-bottom: 1px solid #ccc;
    padding: 10px 0;
}

.description {
    margin: 10px 0;
    padding: 10px;
    width: 100%;
    /* overflow-wrap: break-word; */
    text-align: justify;
    word-wrap: break-word;
}
.main-item {
    margin-bottom: 10px;
    border: 1px solid #ccc;
    border-radius: 6px;
}

.main-item .el-header {
    border-radius: 5px 5px 0 0;
    background-color: #ddd;
    height: fit-content;
    padding: 10px 15px;
    font-family: 'Roboto Medium';
}

.review-group {
    padding: 15px
}

.review-item {
    display: flex;
    flex-direction: column;
    margin-bottom: 10px;
}

.review-item .review-item__header {
    display: flex;
    justify-content: space-between;
}

.review-item .review-item__header .review-owner {
    font-family: 'Roboto Medium';
}

.review-item__body {
    display: flex;
    align-items: center;
    justify-content: space-between;
}

.review-item .review-item__footer {
    font-size: 14px;
    text-align: right;
}

.chapter-index {
    padding: 0;
}

.chapter-list {
    list-style: none;
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

.el-row {
    margin: 0 !important;
}

.userinfo-item {
    display: flex;
    flex-direction: column;
    align-items: center;
}
.lockTag .el-tag{
    background-color: #f56c6c !important;
    font-size: 16px;
    margin: 10px 0;
}
.options-btn {
    /* border: 1px solid #ccc; */
    background-color: #fff;
    color: #000;
    text-align: center;
    float: right;
    width: 24px;
    height: 24px;
    border-radius: 100px;
    padding: 0 !important;
    font-size: 16px;
    line-height: 25px;
    /* border: 1px solid #000; */
}

.options-btn:hover {
    cursor: pointer;
    color: #000;
    border: 1px solid #ccc;

    /* background-color: #ccc; */
}

.options-btn:focus {
    color: #000;
    border: 1px solid #ccc;
}

.genre {
    border: 1px solid #409EFF;
    padding: 0 10px;
    width: fit-content;
    border-radius: 50px;
    margin-bottom: 10px;
    color: #409EFF;
    font-family: 'Roboto Italic'; 
    font-size: 20px;
}
.breadcrumb {
    border: 1px solid #ccc;
    width: fit-content;
    padding: 7px;
    border-radius: 5px;
    margin-left: 10px;
    background-color: #fff;
}
</style>