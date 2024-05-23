<script>
import { inject } from 'vue'
import { mapGetters } from 'vuex'
import dayjs from "dayjs"

export default {
    name: 'BookshelfPage',
    data() {
        return {
            bookshelf: [],
            totalItems: 0,
            isUnfollow: false,
            currentStats: {}
        }
    },
    mounted() {
        this.$api = inject('$api')
        this.loadFollowings()
    },
    computed: {
        ...mapGetters('auth', {
            getUser: 'getAuthData',
            getAuthStatus: 'getLoginStatus'
        }),
    },
    methods: {
        async loadFollowings() {
            var id = this.getUser.userId
            var token = this.getUser.token
            await this.$api.novels.getBookshelf(id,token)
                .then(response => {
                    this.bookshelf = response.data
                    this.totalItems = response.data.length
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async unfollow(novel) {
            var userId = this.getUser.userId
            var bookId = novel.bookId
            var token = this.getUser.token
            var stats = {}
            novel.isUnfollow = true            
            await this.$api.novels.getBookStats(bookId)
                .then(response => {
                    stats = response.data
                    return this.$api.reactions.followBook(userId, bookId, token)
                })
                .then(response => {
                    if (response.status == 200) {
                        this.totalItems--
                        stats.followers--
                        novel.isUnfollow = true
                        console.log(stats)

                        this.updateNovelStats(stats)
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async updateNovelStats(stats) {
            await this.$api.novels.updateStat(stats)
                .catch(error => {
                    console.error(error)
                })
        },
        async getStats(bookId) {
            await this.$api.novels.getBookStats(bookId)
                .then(response => {
                    this.currentStats = response.data
                })
                .catch(error => {
                    console.error(error)
                })
        },
    },
}
</script>

<template>
    <bookshelf-layout>
        <el-container class="bookshelf-showcase">
            <el-header>
                <span class="number">{{ totalItems }}</span>
                <span class="showcase-title">Truyện đang theo dõi</span>
            </el-header>
            <el-main>
                <span v-if="this.totalItems <= 0">Không có truyện nào</span>
                <el-row :gutter="15" v-else >
                    <el-col class="book-item" v-for="(novel, index) in bookshelf" :key="novel.bookId" :span="6" >
                        <post-card  :cover="novel.bookCover != '' ? novel.bookCover : undefined" :title="novel.bookTitle"
                            :novelId="novel.bookId"></post-card>
                        <div class="unfollow-btn" @click="unfollow(novel)" v-if="!novel.isUnfollow"><i class="fa-solid fa-xmark"></i> Hủy theo dõi</div>
                    </el-col>
                </el-row>
            </el-main>
        </el-container>
    </bookshelf-layout>
</template>

<style scoped>
.bookshelf-showcase>.el-header {
    height: 36px;
    border-bottom: 3px solid #000;
    font-family: 'Roboto Bold';
    align-content: center;
    font-size: 18px;
    padding: 0;
}

.bookshelf-showcase>.el-main {
    padding: 15px 0;
}

.bookshelf-showcase>.el-main>.el-row {
    width: 100%;
}

.bookshelf-showcase>.el-header>.number {
    display: inline-block;
    background-color: #000;
    color: #fff;
    height: 36px;
    width: 36px;
    line-height: 36px;
    text-align: center;
    margin-right: 10px;
}

.book-item {
    margin-bottom: 5px;
}

.unfollow-btn {
    color: #f56c6c;
    padding: 0 5px;
    text-align: left;
}
.unfollow-btn:hover {
    color: #ff4343;
    cursor: pointer;
    /* font-weight: 900; */
    font-family: 'Roboto Medium';
    scale: 1.05;
    /* float: left; */
}
</style>