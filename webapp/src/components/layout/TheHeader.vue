<script>
import axios from 'axios'
import { inject } from 'vue'
import { mapActions, mapGetters } from 'vuex'
import clickOutSide from "@mahdikhashan/vue3-click-outside";
export default {
    name: 'TheHeader',
    data() {
        return {
            activeIndex: 0,
            dialogFormVisible: false,
            genres: [],
            homeRoute: "/",
            errors: [],
            searchResults: [],
            searchQuery: null,
            loading: false,
            showNoti: false
        }
    },
    directives: {clickOutSide},
    props: {
        classList: Object,
        height: {
            type: Number,
            default: 60,
        },
        reading: {
            type: Boolean,
            default: false
        },
        dashboard: {
            type: Boolean,
            default: false
        },
        admin: {
            type: Boolean,
            default: false
        },        
    },
    computed: {        
        ...mapGetters('auth', {
            getterLoginStatus: 'getLoginStatus',
            gettersAuthData: 'getAuthData',
            getterActiveToken: 'isTokenActive'
        }),    
    },
    mounted() {
        this.$api = inject('$api')
        if (!this.dashboard && !this.admin) {
            this.loadGenres();
        }
        if (!this.admin) {
            this.homeRoute = "/"
        } else {
            this.homeRoute = "/admin"
        }
    },
    methods: {
        ...mapActions('auth', {
            actionLogout: 'logout'
        }),
        ...mapActions('novel', {
            setGenre: 'commitGenre'
        }),
        login(dialogVisible) {
            this.dialogFormVisible = dialogVisible;
            if (this.$route.name == "Signup") {
                this.$router.push('/')
            } else {
                this.$router.go()
            }
        },
        logout() {
            this.actionLogout(); 
            if (this.$route.path.includes('dashboard')) {
                this.$router.replace('/')
            } else if (this.$route.path.includes('admin')) {
                this.$router.replace('/admin')
                if (this.$route.path == "/admin") {
                    this.$router.go(0)
                }
            } 
            else {
                this.$router.go();
            }
        },
        async loadGenres() {
            var url = `http://localhost:10454/api/Genres/lists`;
            await axios.get(url)
            .then(response => {
                this.genres = response.data;
            })
            .catch(e => {
                this.errors.push(e)
            })
        },
        async search(query, cb) {
            if (query !== '') {
                this.loading = true
                await this.$api.novels.search(query)
                    .then(response => {
                        this.searchResults = response.data.map(book => ({
                            key: book.bookId,
                            value: book.bookId,
                            label: book.bookTitle
                        }))
                        this.loading = false                        
                    })
                    .catch(error => {
                        console.error(error)
                    })
            } else {
                this.searchResults = []
            }            
            cb(this.searchResults)
        },
        showNotiDropdown() {
            this.showNoti = !this.showNoti
            console.log(this.showNoti)
        },
        closeNotiDropdown() {
            if (this.showNoti == true) {
                this.showNoti = false
            }
            console.log(this.showNoti)
        },
        browseGenre(genre) {
            var query = {
                name: genre.genreName
            }
            this.setGenre({genreId: genre.genreId, genreName: genre.genreName})
            this.$router.push({path: '/genre', query: query})
        },
        handleSelect(item) {
            this.$router.push(`novel/${item.key}`)
        }
    },
}
</script>

<template>
    <el-header :class="classList" :height="`${height}px`">
        <el-menu :default-active="this.$route.path" class="header-menu" mode="horizontal" :ellipsis="false"
            :router="true">
            <el-menu-item :index="homeRoute" class="logo">
                <img src="@/assets/logo/logo.png" :height="height">
            </el-menu-item>
            <div class="flex-grow"></div>
            <div class="searchBar" index="/Search" v-if="!reading && !dashboard && !admin">
                <el-autocomplete v-model="searchQuery" :fetch-suggestions="search" placeholder="Tìm kiếm" @select="handleSelect">
                    <template #suffix>
                        <i class="fa-solid fa-magnifying-glass"></i>
                    </template>
                    <template #default="{ item }">
                        <div class="value">{{ item.label }}</div>                        
                    </template>
                </el-autocomplete>
                <!-- <el-select-v2 v-model="searchQuery" style="width: 240px" filterable clearable
                    :options="this.searchResults" remote :remote-method="search" :loading="loading"
                    @change="this.$router.push(`novel/${searchQuery}`)" placeholder="Tìm kiếm ...">
                </el-select-v2> -->
            </div>
            <el-sub-menu index="/danh-muc" v-if="!dashboard && !admin">
                <template #title>Danh mục</template>
                <!-- <el-menu-item index="/danh-muc/tan-van">Tản văn</el-menu-item> -->
                <el-menu-item v-for="genre in genres" :key="genre.genreId" @click="browseGenre(genre)">{{
                    genre.genreName }}</el-menu-item>
            </el-sub-menu>
            <!-- <el-menu-item index="/sang-tac" v-if="!dashboard && !admin">Sáng tác</el-menu-item> -->
            <!-- <el-menu-item index="/xep-hang" v-if="!dashboard && !admin">Xếp hạng</el-menu-item> -->
            <el-menu-item class="signInBtn" v-if="!getterLoginStatus" style="align-self: center;"
                @click="dialogFormVisible = true">
                <template #title>Đăng nhập</template>
            </el-menu-item>
            <!-- <el-icon class="noti-btn"><Bell /></el-icon> -->
            <!-- <div class="noti-wrapper" v-if="getterLoginStatus">

                <el-dropdown class="noti-dropdown" trigger="click">
                    <div class="noti-btn">
                        <i class="fa-regular fa-bell"></i>
                        <i class="fa-solid fa-bell"></i>
                    </div>

                    <template #dropdown>
                        <el-dropdown-menu>
                            <el-dropdown-item>Action 1</el-dropdown-item>
                            <el-dropdown-item>Action 2</el-dropdown-item>
                            <el-dropdown-item>Action 3</el-dropdown-item>
                            <el-dropdown-item disabled>Action 4</el-dropdown-item>
                            <el-dropdown-item divided>Action 5</el-dropdown-item>
                        </el-dropdown-menu>
                    </template>
                </el-dropdown>
            </div> -->

            <el-sub-menu index="6" v-if="getterLoginStatus && !admin">
                <template #title>{{ gettersAuthData.name }}</template>
                <el-menu-item :index="`/profile/${gettersAuthData.userId}`" v-if="!dashboard">Tài khoản</el-menu-item>
                <el-menu-item index="/bookshelf" v-if="!dashboard">Kệ sách</el-menu-item>
                <el-menu-item index="/bookmarks" v-if="!dashboard">Đánh dấu</el-menu-item>
                <!-- <el-menu-item index="5-4">Lịch sử đọc</el-menu-item> -->
                <el-menu-item index="/dashboard" v-if="!dashboard">Hệ thống</el-menu-item>
                <el-menu-item index="/profile/edit" v-if="dashboard">Thay đổi thông tin</el-menu-item>
                <el-menu-item index="/profile/change-password" v-if="dashboard">Đổi mật khẩu</el-menu-item> 

                <el-menu-item @click="logout">Đăng xuất</el-menu-item>
            </el-sub-menu>
            <el-sub-menu index="6" v-if="getterLoginStatus && admin">
                <template #title>{{ gettersAuthData.name }}</template>

                <el-menu-item @click="logout">Đăng xuất</el-menu-item>
            </el-sub-menu>
        </el-menu>
    </el-header>
    <login-modal :dialogVisible="this.dialogFormVisible" @close="login"></login-modal>
</template>

<style scoped>
.flex-grow {
    flex-grow: 1;
}
.el-header {
    width: 100%;
    z-index: 1000;
    padding: 0;
    position: sticky;
    top: 0px;
    background-color: #242424;
}
.el-header .el-menu {
    height: 100%;
}

.header-menu{
    align-items: center;
}

.header-menu>.el-sub-menu{
    height: inherit;
}
.logo:hover {
    background-color: #fff !important;
}
#signInBtn:hover {
    background-color: #fff;
    cursor: auto;
}
#signInBtn.is-active {
    border-bottom: 0;
    background-color: #fff;
}
.searchBar {
    display: inline-flex;
    justify-content: center;
    align-items: center;
}

.noti-wrapper {
    position: relative;
}
.noti-btn {
    /* border: none; */
    /* position: relative; */
    padding: 0 10px;
    font-size: 18px;
}
.noti-dropdown {
    border: none;
}

.noti-btn>.fa-solid {
    display: none;
}

.noti-btn:hover {
    cursor: pointer;
}

.noti-btn:hover>.fa-solid {
    display: inline;
}

.noti-btn:hover>.fa-regular {
    display: none;
}


</style>