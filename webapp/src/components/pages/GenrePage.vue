<script>
import { inject } from 'vue'
import { mapActions, mapGetters } from 'vuex'
export default {
    name: 'GenrePage',
    data() {
        return {
            novels: [],
            totalItems: 0,      
            currentPage: 1,
            pageSize: 20,
            genre: {},
            filter: {},
            isCurrent: [
                true, false, false, false, false, false, 
                false, false, false, false, false, false, 
                false, false, false, false, false, false, 
                false, false, false, false, false, false, 
                false, false, false
            ],
            alphaCurrent: 0
        }
    },
    mounted() {
        this.$api = inject('$api')
        this.filter.genreId = this.getGenre.genreId
        this.filter.status = [0,1,2]
        this.loadGenre()
        this.loadNovelByGenre(1)
    },
    watch: {
        $route(to, from) {
            if (Object.hasOwn(to.query, 'alphabet')) {
                if (to.query.alphabet != 'khac') {
                    this.filter.title = to.query.alphabet 
                }      
            } else {
                this.filter.title = null
            }
            this.loadGenre()
            this.filter.genreId = this.getGenre.genreId        
            this.loadNovelByGenre(1)
        },
    },
    computed: {
        ...mapGetters('novel', {
            getGenre: 'getGenre'
        })
    },
    methods: {
        async loadNovelByGenre(page) {
            // console.log(this.filter)
            await this.$api.novels.filter(this.filter, page)
                .then(response => {
                    if (response.status == 200) {
                        this.novels = response.data.data
                        this.totalItems = response.data.totals
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async loadGenre() {
            await this.$api.genres.getDetails(this.getGenre.genreId)
                .then(response => {
                    if (response.status == 200) {
                        this.genre = response.data
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        handleCurrentChange(val) {
            this.loadNovelByGenre(val)
        },
        handleAlphaClick(val) {
            if (val != this.alphaCurrent) {
                this.isCurrent[this.alphaCurrent] = false
                this.isCurrent[val] = true
                this.alphaCurrent = val
            }
        },
        handleStatusApply() {
            var currentQuery = {...this.$route.query}
            currentQuery.status = this.filter.status
            this.$router.push({path: '', query: currentQuery})
        }
    }
}
</script>
<template>
    <layout-default>
        <el-breadcrumb class="breadcrumb" separator="/">
            <el-breadcrumb-item :to="{ path: '/' }"><i class="fa-solid fa-house"></i></el-breadcrumb-item>
            <el-breadcrumb-item>{{ this.getGenre.genreName }}</el-breadcrumb-item>
        </el-breadcrumb>
        <el-row :gutter="20">
            <el-col :span="18">
                <div class="main-part">
                    <div class="description">
                        <h2 style="margin: 0 0 10px; color: #56ccf2;"><i class="fa-solid fa-flag"></i> {{
                            genre.genreName }}</h2>
                        {{ genre.description }}
                    </div>
                    <el-row :gutter="10">
                        <el-col v-for="(novel, index) in novels" :key="novel.bookId" :span="4">
                            <PostCard :cover="novel.bookCover != '' ? novel.bookCover : undefined"
                                :title="novel.bookTitle" :novelId="novel.bookId"></PostCard>
                        </el-col>
                    </el-row>
                    <div class="pagination">
                        <el-pagination v-model:current-page="currentPage" v-model:pageSize="pageSize" :background="true"
                            layout="prev, pager, next, jumper" :total="totalItems"
                            @current-change="handleCurrentChange">
                        </el-pagination>
                    </div>
                </div>
            </el-col>
            <el-col :span="6">
                <div class="filter-part">
                    <div class="filter-item">
                        <div class="filter-title">
                            Chữ cái
                        </div>
                        <div class="filter-options">
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`}}" class="alphabet-item"
                                :class="{current: isCurrent[0]}" @click="handleAlphaClick(0)">Tất cả</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'khac'}}"
                                class="alphabet-item" :class="{current: isCurrent[1]}"
                                @click="handleAlphaClick(1)">#</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'a'}}"
                                class="alphabet-item" :class="{current: isCurrent[2]}"
                                @click="handleAlphaClick(2)">A</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'b'}}"
                                class="alphabet-item" :class="{current: isCurrent[3]}"
                                @click="handleAlphaClick(3)">B</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'c'}}"
                                class="alphabet-item" :class="{current: isCurrent[4]}"
                                @click="handleAlphaClick(4)">C</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'd'}}"
                                class="alphabet-item" :class="{current: isCurrent[5]}"
                                @click="handleAlphaClick(5)">D</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'e'}}"
                                class="alphabet-item" :class="{current: isCurrent[6]}"
                                @click="handleAlphaClick(6)">E</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'f'}}"
                                class="alphabet-item" :class="{current: isCurrent[7]}"
                                @click="handleAlphaClick(7)">F</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'g'}}"
                                class="alphabet-item" :class="{current: isCurrent[8]}"
                                @click="handleAlphaClick(8)">G</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'h'}}"
                                class="alphabet-item" :class="{current: isCurrent[9]}"
                                @click="handleAlphaClick(9)">H</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'i'}}"
                                class="alphabet-item" :class="{current: isCurrent[10]}"
                                @click="handleAlphaClick(10)">I</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'j'}}"
                                class="alphabet-item" :class="{current: isCurrent[11]}"
                                @click="handleAlphaClick(11)">J</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'k'}}"
                                class="alphabet-item" :class="{current: isCurrent[12]}"
                                @click="handleAlphaClick(12)">K</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'l'}}"
                                class="alphabet-item" :class="{current: isCurrent[13]}"
                                @click="handleAlphaClick(13)">L</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'm'}}"
                                class="alphabet-item" :class="{current: isCurrent[14]}"
                                @click="handleAlphaClick(14)">M</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'n'}}"
                                class="alphabet-item" :class="{current: isCurrent[15]}"
                                @click="handleAlphaClick(15)">N</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'o'}}"
                                class="alphabet-item" :class="{current: isCurrent[16]}"
                                @click="handleAlphaClick(16)">O</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'p'}}"
                                class="alphabet-item" :class="{current: isCurrent[17]}"
                                @click="handleAlphaClick(17)">P</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'q'}}"
                                class="alphabet-item" :class="{current: isCurrent[18]}"
                                @click="handleAlphaClick(18)">Q</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'r'}}"
                                class="alphabet-item" :class="{current: isCurrent[19]}"
                                @click="handleAlphaClick(19)">R</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 's'}}"
                                class="alphabet-item" :class="{current: isCurrent[20]}"
                                @click="handleAlphaClick(20)">S</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 't'}}"
                                class="alphabet-item" :class="{current: isCurrent[21]}"
                                @click="handleAlphaClick(21)">T</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'u'}}"
                                class="alphabet-item" :class="{current: isCurrent[22]}"
                                @click="handleAlphaClick(22)">U</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'v'}}"
                                class="alphabet-item" :class="{current: isCurrent[23]}"
                                @click="handleAlphaClick(23)">V</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'w'}}"
                                class="alphabet-item" :class="{current: isCurrent[24]}"
                                @click="handleAlphaClick(24)">W</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'x'}}"
                                class="alphabet-item" :class="{current: isCurrent[25]}"
                                @click="handleAlphaClick(25)">X</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'y'}}"
                                class="alphabet-item" :class="{current: isCurrent[26]}"
                                @click="handleAlphaClick(26)">Y</router-link>
                            <router-link :to="{path:'', query: {name: `${genre.genreName}`, alphabet: 'z'}}"
                                class="alphabet-item" :class="{current: isCurrent[27]}"
                                @click="handleAlphaClick(27)">Z</router-link>
                        </div>
                    </div>
                    <div class="filter-item">
                        <div class="filter-title">
                            Trạng thái
                        </div>
                        <div class="filter-options">
                            <!-- <el-radio-group class="status-filter" v-model="this.filter.status">
                                <el-radio :value=0>Tạm ngưng</el-radio>
                                <el-radio :value=1>Đang tiến hành</el-radio>
                                <el-radio :value=2>Hoàn thành</el-radio>
                            </el-radio-group> -->
                            <el-checkbox-group  class="status-filter" v-model="this.filter.status">
                                <el-checkbox label="Đang tiến hành" :value=1 />
                                <el-checkbox label="Hoàn thành" :value=2 />
                                <el-checkbox label="Tạm ngưng" :value=0 />
                            </el-checkbox-group>
                            <el-button type="success" round @click="handleStatusApply">Áp dụng</el-button>
                        </div>
                    </div>
                </div>
            </el-col>
        </el-row>

    </layout-default>
</template>

<style scoped>
.breadcrumb {
    border: 1px solid #ccc;
    width: fit-content;
    padding: 7px;
    border-radius: 5px;
    background-color: #fff;
}

.main-part {
    /* border: 1px solid #ccc; */
    /* border-radius: 5px; */
    
    padding: 10px 0;
    display: flex;
    flex-direction: column;
    gap: 15px;
}

.filter-part {
    /* border: 1px solid #ccc; */
    border-radius: 5px;
    padding: 10px 0;
}

.description {
    border: 1px solid #ccc;
    border-radius: 5px;
    padding: 10px;
    background-color: #fff;
}

.filter-item {
    border: 1px solid #ccc;
    border-radius: 5px;
    background-color: #fff;
    padding: 10px;
    margin-bottom: 20px;
}

.filter-item>.filter-title {
    border: 1px solid #ccc;
    background-color: #ebebeb;
    border-radius: 5px;
    width: fit-content;
    padding: 5px;
    font-family: 'Roboto Bold';
    margin-bottom: 20px;
}

.filter-item>.filter-options {
    /* padding: 10px 0; */
    margin-bottom: 20px;
}

.alphabet-item {
    /* border: 1px solid #ccc; */
    border-radius: 50px;
    text-align: center;
    align-content: center;
    padding: 0 10px;
    min-height: 30px;
    display: inline-block;
    margin-right: 2px;
    background-color: #fff;
    font-family: 'Roboto Medium';
}

.alphabet-item:hover {
    /* border: 1px solid #ccc;     */
    background-color: #409eff;
    color: #fff;
}

.alphabet-item.current {
    background-color: #409eff;
    color: #fff;
}


.status-filter {
    display: flex;
    flex-direction: column;
    align-items: baseline;
}
.pagination {
    display: flex;
    justify-content: flex-end;
}

:deep(.el-pagination *) {
    border-radius: 30px;
}
</style>