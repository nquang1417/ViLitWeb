<script>
import { mapGetters } from 'vuex'
import { inject } from 'vue'
import { getRemainingTime } from '../../scripts/utils/utils'
import { ElMessage } from 'element-plus'

export default {
    name: 'ReviewPage',
    data() {
        return {
            reviews: [],
            totalItems: 0,
            novel: {},
            newReview: {},
            novelStats: {},
            reportFormVisible: false,
        }
    },
    computed: {
        ...mapGetters('novel', {
            getNovel: 'getNovelInfo'
        }),
        ...mapGetters('auth', {
            getLoginStatus: 'getLoginStatus',
            getUser: 'getAuthData'
        }),
    },
    mounted() {
        this.$api = inject('$api')
        this.novel = this.getNovel
        this.loadReviews(1)
        this.novelStats = {
            bookId: this.novel.bookId,
            views: this.novel.views,
            followers: this.novel.followers,
            comments: this.novel.comments,
            reviews: this.novel.reviews,
            averageRating: this.novel.averageRating
        }
    },
    methods: {
        async loadReviews(page) {
            var bookId = this.novel.bookId
            await this.$api.reviews.filter(bookId, page)
                .then(response => {
                    if (response.status == 200) {
                        this.reviews = response.data.data
                        this.totalItems = response.data.totals
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async uploadReview() {
            this.newReview.bookId = this.novel.bookId
            this.newReview.userId = this.getUser.userId
            var token = this.getUser.token
            if (!Object.hasOwn(this.newReview, 'ratingScore') || !Object.hasOwn(this.newReview, 'reviewContent')) {
                ElMessage({message: 'Điểm đánh giá và nội dung đánh giá không được để trống', type:'error', duration: 1500})
                return
            } else {
                await this.$api.reviews.add(this.newReview, token)
                    .then(response => {
                        if (response.status == 201) {
                            ElMessage({ message: 'Đăng đánh giá thành công.', type: 'success', duration: 1500 })
                            this.reviews.push(response.data)
                            var sum = this.novelStats.averageRating * this.totalItems + response.data.ratingScore                            
                            this.novelStats.reviews = ++this.totalItems
                            this.novelStats.averageRating = Math.round((sum / this.totalItems) * 10) / 10
                            this.novel.reviews = this.novelStats.reviews
                            this.novel.averageRating = this.novelStats.averageRating 
                            this.updateNovelStats(this.novelStats)
                        }
                    })
                    .catch(error => {
                        console.error(error)
                    })
            }
        },
        async updateNovelStats(stats) {
            await this.$api.novels.updateStat(stats)
                .catch(error => {
                    console.error(error)
                })
        },
        formatDate(date) {
            return getRemainingTime(date)
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
        handleReportClick() {
            if (!this.getLoginStatus) {
                ElMessage({message: 'Yêu cầu đăng nhập trước khi thực hiện chức năng này', type: 'warning', duration: 1500})
            } else {                
                this.reportFormVisible = true
                this.reportData = {}
                this.reportData.senderId = this.getUser.userId
                this.reportData.targetType = 0
                this.reportData.reportedType = 0
                this.reportData.reportedEntityId = this.novel.bookId
                this.reportData.createBy = this.getUser.userId
                this.reportData.updateBy = this.getUser.userId                
            }            
        },
    }
}
</script>

<template>
    <layout-default>
        <template #dialog>
            <el-dialog v-model="reportFormVisible" title="Tố cáo hành vi" width="40%">
                <report-form :data="reportData" @update:data="(formdata) => {this.reportData = formdata}" :reportType=0></report-form>
                <template #footer>
                    <div class="dialog-footer">
                        <el-button type="primary" @click="send">Gửi</el-button>
                        <el-button @click="this.reportFormVisible = false">Hủy</el-button>                        
                    </div>
                </template>
            </el-dialog>
        </template>
        <div class="page-cover"></div>
        <el-row :gutter="20">
            <el-col :span="18">
                <div class="main-part">
                    <div class="title">
                        <i class="fa-solid fa-circle" style="font-size: 13px; line-height: 25px;"></i>
                        <span> Đánh giá truyện {{ novel.bookTitle }}</span>
                    </div>
                    <div class="review-editor__wrapper" v-if="this.getLoginStatus">
                        <div class="review-rating">
                            <span>Đánh giá của bạn: </span>
                            <el-rate v-model="this.newReview.ratingScore" void-color="#f7ba2a"
                                score-template="{value}/5 điểm" show-score allow-half clearable></el-rate>
                        </div>
                        <div class="review-editor">
                            <quill-editor contentType="text"
                                v-model:content="this.newReview.reviewContent"></quill-editor>
                        </div>
                        <el-button class="review-upload__btn" round color="#111827" @click="uploadReview()">Đăng đánh
                            giá</el-button>
                    </div>

                    <div class="reviews-list">
                        <el-row class="review-item" v-for="(review, index) in reviews" :key="review.reviewId">
                            <el-row class="review-info">
                                <div class="review-info__avatar">
                                    <el-image :src="review.avatar"></el-image>
                                </div>
                                <div class="review-info__main">
                                    <div class="review-info__username">{{ review.displayName }}</div>
                                    <div class="review-info__score">
                                        <span>Đánh giá </span>
                                        <el-rate v-model="review.ratingScore" disabled></el-rate>
                                    </div>
                                    <div class="review-info__date">
                                        {{ formatDate(review.createDate) }}
                                    </div>
                                </div>
                                <div class="flex-grow"></div>
                                <el-dropdown class="noti-dropdown" trigger="click">
                                    <div class="options-btn">
                                        <i class="fa-solid fa-ellipsis"></i>
                                    </div>
                                    <template #dropdown>
                                        <el-dropdown-menu>
                                            <el-dropdown-item @click="handleReportClick">Báo cáo vấn
                                                đề</el-dropdown-item>
                                        </el-dropdown-menu>
                                    </template>
                                </el-dropdown>
                            </el-row>
                            <div class="review-content">
                                <div>{{ review.reviewContent }}</div>
                                <div class="likes-count">
                                    <i class="fa-solid fa-thumbs-up"></i>
                                    {{ review.likes }} người đồng ý
                                </div>
                            </div>
                        </el-row>
                    </div>
                </div>
            </el-col>
            <el-col :span="6">
                <div class="side-part">
                    <post-card :cover="novel.bookCover != '' ? novel.bookCover : undefined" :title="novel.bookTitle"
                        :novelId="novel.bookId">
                    </post-card>
                    <span>
                        <i class="fa-solid fa-star" style="color: #facc15;"></i>
                        <span style="font-family: 'Roboto Bold'; padding-left: 5px;"> {{ this.novel.averageRating }} |
                        </span>
                        <span style="text-decoration-line: underline;">{{ this.novel.reviews }} Đánh giá</span>
                    </span>
                </div>
            </el-col>
        </el-row>
    </layout-default>
</template>

<style scoped>
.page-cover {
    height: 300px;
    overflow: hidden;
    background-image: url('https://trusteid.mioa.gov.mk/wp-content/plugins/uix-page-builder/uixpb_templates/images/UixPageBuilderTmpl/default-cover-2.jpg');
    background-size: cover;
    background-repeat: no-repeat;
    background-attachment: scroll;
    background-position-y: center;
    background-position-x: center;
    margin-bottom: 20px;
}

.main-part {
    /* border: 1px solid #ccc; */
    /* border-radius: 5px; */
    /* background-color: #fff; */
    padding: 10px 0;
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.side-part {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    border-radius: 5px;
    padding: 10px 0;
    background-color: #fff;
}

.title {
    display: flex;
    align-items: center;
    font-family: 'Roboto Bold';
    font-size: 26px;
    gap: 10px;
}

.flex-grow {
    flex-grow: 1;
}

.reviews-list { 
    display: flex;
    flex-direction: column;
    gap: 20px;
    background-color: #fff;
    padding: 15px 5px;
}

.review-item {
    background-color: #fff;
    /* border: 1px solid #ccc; */
}

.review-item>.review-info {
    width: 100%;
    gap: 10px;
    /* border: 1px solid #ccc; */
}

.review-info__avatar {
    width: 60px;
    height: 60px;
    border-radius: 50px;
    overflow: hidden;

}

.review-item>.review-info .review-info__username {
    font-family: 'Roboto Medium';
}

.review-item>.review-info .review-info__username:hover {
    color: #409eff;
    cursor: pointer;
}

.review-item>.review-info .review-info__score {
    display: flex;
    align-items: center;
}

.review-item>.review-info .review-info__date{
    color: #9ca3af;
    font-size: 14px;
}
.review-item>.review-content {
    background-color: #f1f5f9;
    width: 100%;
    padding: 8px;
    border-radius: 5px;
    overflow: hidden;
}

.likes-count {
    margin-top: 10px;
    font-family: 'Roboto Bold';
    font-size: 14px;
}

.review-editor__wrapper {
    min-height: 280px;
}

.review-editor {
    background-color: #fff;
    margin-bottom: 10px;
    /* min-height: 200px; */    
}


.review-upload__btn {
    float: right;
}

.review-rating {
    display: flex;
    align-items: center;
    gap: 10px;

}

.review-rating .el-rate {
    background-color: #f1f5f9;
    padding: 0 8px;
    border-radius: 5px;
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
:deep(.ql-container) {
    min-height: 200px;
}
</style>