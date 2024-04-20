<script lang="js">
import axios from 'axios'
import dayjs from "dayjs"
import { mapActions, mapGetters } from 'vuex'
import {getRemainingTime} from '../../scripts/utils/utils'

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
            lastUpdate: ''
        }
    },
    created() {
        this.loadNovel()
        this.loadChapters(1)
    },
    computed: {
        id() {
            return this.$route.params.novelId
        },
        ...mapGetters('novel', {
            getNovel: 'getNovelInfo'
        }),
        
    },
    methods: {
        ...mapActions('novel', {
            saveNovel: 'updateNovel',
            selectChapter: 'updateChapter'
        }),
        async loadNovel() {
            var url = `http://localhost:10454/api/BookInfo/details?bookId=${this.id}`
            await axios.get(url)
                .then(response => {
                    this.novel = JSON.parse(JSON.stringify(response.data))
                    this.totalItems = this.novel.chapters

                    // var decodedContent = atob(this.novel.Cover.FileContents);

                    // Convert decoded content to a Uint8Array
                    // var uint8Array = new Uint8Array(decodedContent.length);
                    // for (var i = 0; i < decodedContent.length; i++) {
                    //     uint8Array[i] = decodedContent.charCodeAt(i);
                    // }

                    // Create a Blob from the Uint8Array
                    // var blob = new Blob([uint8Array], { type: 'image/jpeg' });

                    // Create a data URL for the Blob

                    // this.novel.CoverUrl = URL.createObjectURL(blob);
                    this.novel.CoverUrl = this.novel.bookCover;
                    
                })
                .catch(e => {
                    console.error(e);
                })
            this.lastUpdate = getRemainingTime(this.novel.updateDate)
            
            switch (this.novel.status) {
                case '0':
                    this.novel.status = "Tạm ngưng"
                    break;
                case '1':
                    this.novel.status = "Đang tiến hành"
                    break;
                case '2':
                    this.novel.status = "Hoàn thành"
                    break;
            }
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
    },
}
</script>

<template>
    <layout-default>
        <el-breadcrumb separator="/">
            <el-breadcrumb-item :to="{ path: '/' }"><i class="fa-solid fa-house"></i></el-breadcrumb-item>
            <el-breadcrumb-item :to="{ path: '/' }">{{ novel.genreName }}</el-breadcrumb-item>
            <el-breadcrumb-item>{{ novel.bookTitle }}</el-breadcrumb-item>
        </el-breadcrumb>
        <el-container>
            <el-row class="novel-details" :gutter="20">
                <el-col :span="16">
                    <el-container class="feature-section">
                        <el-main>
                            <div class="top-part">
                                <el-row :gutter="30">
                                    <el-col :xs="10" :sm="8" :md="6" :lg="7" :xl="6" class="series-cover">
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
                                            <div class="info-item">
                                                <span class="info-name">Tác giả: </span>
                                                <span class="info-value"><a href="#">{{ novel.authorName }}</a></span>
                                            </div>
                                            <div class="info-item">
                                                <span class="info-name">Tình trạng: </span>
                                                <span class="info-value">{{ novel.status }}<a href="#">
                                                    </a></span>
                                            </div>

                                        </div>
                                        <div class="side-features">
                                            <el-row justify="space-between" :align="'middle'">
                                                <el-col class="feature-item" :span="6" :align="'center'"
                                                    @click="console.log('follow')">
                                                    <span class="block feature-value"><i
                                                            class="fa-regular fa-heart"></i></span>
                                                    <span class="block feature-name">{{ novel.followers }}</span>
                                                </el-col>
                                                <el-col class="feature-item" :span="6" :align="'center'"
                                                    @click="console.log('rating')">
                                                    <span class="block feature-value"><i
                                                            class="fa-regular fa-star"></i></span>
                                                    <span class="block feature-name">{{ novel.averageRating }}</span>
                                                </el-col>
                                                <el-col class="feature-item" :span="6" :align="'center'"
                                                    @click="console.log('navigate to index')">
                                                    <span class="block feature-value"><i
                                                            class="fa-solid fa-list"></i></span>
                                                    <span class="block feature-name">Mục lục</span>
                                                </el-col>
                                                <el-col class="feature-item" :span="6" :align="'center'"
                                                    @click="console.log('share')">
                                                    <span class="block feature-value"><i
                                                            class="fa-solid fa-share-nodes"></i></span>
                                                    <span class="block feature-name">Chia sẻ</span>
                                                </el-col>
                                            </el-row>
                                        </div>
                                    </el-col>
                                </el-row>
                            </div>
                            <div class="bottom-part">
                                <div class="statistics">
                                            <el-row justify="space-between" :align="'middle'">
                                                <el-col class="statistics-item" :span="6" :align="'center'"
                                                    @click="console.log('follow')">
                                                    <span class="block feature-name">Lần cuối</span>
                                                    <span class="block feature-value font-bold"> {{ this.lastUpdate }}</span>
                                                </el-col>
                                                
                                                <el-col class="statistics-item" :span="6" :align="'center'"
                                                    @click="console.log('navigate to index')">
                                                    <span class="block feature-name">Đánh giá</span>
                                                    <span class="block feature-value font-bold">{{ novel.averageRating}} / {{ novel.reviews }}</span>
                                                </el-col>
                                                <el-col class="statistics-item" :span="6" :align="'center'"
                                                    @click="console.log('share')">
                                                    <span class="block feature-name">Lượt xem</span>
                                                    <span class="block feature-value font-bold">{{ novel.views }}</span>
                                                </el-col>
                                            </el-row>
                                        </div>
                                <el-row>
                                    <div class="description">
                                        <div class="block font-bold">Tóm Tắt</div>
                                        <span style="font-size: 15px;">Đây là văn bản tóm tắt nội dung cuốn tiểu thuyết giới hạn 1000 ký tự.</span>
                                    </div>
                                </el-row>
                            </div>
                        </el-main>
                    </el-container>
                </el-col>
                <el-col :span="8">
                    <el-container class="side-bar">
                        <div class="side-bar-item">
                            <div class="block series-owner">
                                <img class="block" width="50" heigh="50"
                                    src="https://as2.ftcdn.net/v2/jpg/03/31/69/91/1000_F_331699188_lRpvqxO5QRtwOM05gR50ImaaJgBx68vi.jpg">
                                <span class="block owner-name">{{ novel.username }}</span>
                            </div>
                            <div class="owner-donate"></div>
                        </div>
                        <el-container class="side-bar-item notes">
                            <el-header>
                                <span>Chú thích thêm</span>
                            </el-header>
                            <el-main>
                                <p>Editor: aljdfhasjfh</p>
                                <p>Nguon: lskjfhalksf</p>
                            </el-main>
                        </el-container>
                        <el-container class="side-bar-item discussion">
                            <el-header>
                                <span>Thảo luận</span>
                            </el-header>
                            <el-main>
                                <p>dfkjahfasjfh</p>
                                <p>lskjfhalksf</p>
                            </el-main>
                        </el-container>
                    </el-container>
                </el-col>
                <el-col :lg="16">
                    <el-container class="basic-section">
                        <el-container class="main-item">
                            <el-header>
                                <span>Reviews mới</span>
                                <span style="float: right;">{{ this.novel.reviews }}</span>
                            </el-header>
                            <el-main class="review-group">
                                <div class="review-item">
                                    <div class="review-item__header">
                                        <div class="review-owner">username</div>
                                        <div class="review-rating">rating score</div>
                                    </div>
                                    <div class="review-item__body">review content</div>
                                    <div class="review-item__footer">
                                        upload time
                                    </div>
                                </div>
                                <div class="review-item">
                                    <div class="review-item__header">
                                        <div class="review-owner">username</div>
                                        <div class="review-rating">rating score</div>
                                    </div>
                                    <div class="review-item__body">review content</div>
                                    <div class="review-item__footer">
                                        upload time
                                    </div>
                                </div>
                            </el-main>
                        </el-container>
                        <el-container class="main-item">
                            <el-header>
                                <span>Mục lục</span>
                            </el-header>
                            <el-main class="chapter-index">
                                <el-table :data="chapters" stripe style="width: 100%" :show-header="false"
                                    @row-click="handleRowClick">
                                    <el-table-column prop="chapterTitle" label="Title" :align="'left'" min-width="200" />
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
    margin-top: 20px;
    width: 100%;
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

.side-bar {
    /* border: 1px solid #ccc; */
    border-radius: 6px;
    flex-direction: column;
}

.side-bar-item {
    margin-bottom: 20px;
    box-sizing: border-box;
    border: 1px solid #ccc;
    border-radius: 6px;
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
    font-weight: 700;
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
    display: block;
    position: relative;
    width: 100%;
    background-color: #36a189;
    border-radius: 5px 5px 0 0;
}

.series-owner img {
    border-radius: 5px 0 0;
}

.series-owner .owner-name {
    position: absolute;
    left: 80px;
    width: calc(100% - 80px);
    top: 50%;
    transform: translateY(-50%);
    color: #fff;
}

.owner-donate {
    border-bottom: 4px solid #d9534f;
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


.review-item .review-item__footer {
    font-size: 14px;
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
</style>