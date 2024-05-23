<script>
import { inject } from 'vue'
import { mapGetters } from 'vuex'
import dayjs from "dayjs"
export default {
    name: 'BookshelfLayout',
    data() {
        return {      
            showHeader: true,
            lastScrollPosition: 0,
            scrollOffset: 40,
            coverUrl: 'https://i.ibb.co/jWnvwM3/607021.jpg',
            avaUrl: 'https://i.docln.net/lightnovel/users/ua17930-17338602-c691-409d-a5c6-6c3f1ed3d84e.jpg',
            userData: {},
            bookshelf: [],
            totalItems: 0
        }
    },
    mounted() {
        this.lastScrollPosition = window.scrollY
        window.addEventListener('scroll', this.onScroll)
        this.$api = inject('$api')
        this.loadUser()
    },
    beforeDestroy() {
        window.removeEventListener('scroll', this.onScroll)
    },
    computed: {
        ...mapGetters('auth', {
            getUser: 'getAuthData',
            getAuthStatus: 'getLoginStatus'
        }),
    },
    methods: {
        // Toggle if navigation is shown or hidden
        onScroll() {
            if (window.scrollY < 0) {
                return
            }
            if (Math.abs(window.scrollY - this.lastScrollPosition) < this.scrollOffset) {
                return
            }
            this.showHeader = window.scrollY < this.lastScrollPosition
            this.lastScrollPosition = window.scrollY
        },
        async loadUser() {
            await this.$api.users.getUser(this.getUser.userId)
                .then(response => {
                    this.userData = response.data
                    this.coverUrl = response.data.cover
                    this.avaUrl = response.data.avatar
                    this.userData.formatCreateDate = dayjs(response.data.createDate).format("DD/MM/YYYY")
                    this.userData.formatUpdateDate = dayjs(response.data.updateDate).format("DD/MM/YYYY")
                })
                .catch(error => {
                    console.error(error)
                })
        },
    },
}
</script>

<template>
    <the-header :classList="{ 'is-hidden': !showHeader }" :height="40"></the-header>
    <el-container class="bookshelf-layout">
        <slot name="dialog"></slot>
        <el-main class="main-part">
            <div class="profile-feature">
                <div class="profile-cover">
                    <div class="content-img" :style="`background-image: url(${coverUrl});`"></div>
                </div>
            </div>
            <el-container class="bookshelf-main">
                <el-aside class="private-tabs">
                    <el-container>
                        <el-header>
                            <h4 style="margin:0">Tài khoản</h4>
                            <span class="user-name">{{ this.userData.displayName }}</span>
                        </el-header>
                        <el-main>
                            <el-menu :default-active="this.$route.path" :router="true" mode="vertical"
                                class="el-menu-vertical-demo">
                                <el-menu-item index="/bookshelf">
                                    <el-icon><Collection /></el-icon>
                                    <template #title>Kệ sách</template>
                                </el-menu-item>                                
                                <el-menu-item index="/bookmarks">
                                    <el-icon><i class="fa-regular fa-bookmark"></i></el-icon>
                                    <template #title>Đánh dấu</template>
                                </el-menu-item>
                                <!-- <el-menu-item index="/notifications">
                                    <el-icon><Bell /></el-icon>
                                    <template #title>Thông báo cá nhân</template>
                                </el-menu-item> -->
                            </el-menu>
                        </el-main>
                    </el-container>
                </el-aside>
                <el-main class="bookshelf-data">
                    <slot></slot>
                </el-main>
            </el-container>
        </el-main>
    </el-container>
</template>

<style scoped>

.main-part {
    min-height: 564px;
}

.bookshelf-data {
    padding-top: 0;
}

.profile-feature {
    position: relative;
    border: 1px solid #ccc;
    border-radius: 5px;
    overflow: hidden;
    margin-bottom: 20px;
    /* padding-bottom: 10px; */
    background-color: #fff !important;

}
.profile-info {
    padding: 10px 0;
    border-radius: 5px;
    border: 1px solid #ccc;
    overflow: hidden;
    background-color: #fff !important;

}

.profile-cover {
    min-height: 250px;
    position: relative;
}
.profile-nav {
    position: relative;
    min-height: 48px;
    padding: 10px;
}

.profile-ava-wrapper {
    position: absolute;
    bottom: 10px;
    left: 20px;
}

.content-img {
    /* background-image: url('https://i.ibb.co/jWnvwM3/607021.jpg'); */
    background-size: 100% auto;
    background-repeat: no-repeat;
    background-attachment: scroll;
    background-position-y: center;
    background-position-x: center;
    background-color: aquamarine;
    /* min-height: 300px; */
}

.profile-cover>.content-img {
    position: absolute;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
}


.profile-ava {
    position: relative;
    height: 150px;
    width: 150px;
    border-radius: 100px;
    overflow: hidden;   
    background-color: antiquewhite;
}

.profile-intro {
    padding-left: 180px;
    height: 100%;
}

.profile-intro>.profile-name {
    font-size: 1.35rem;
    font-family: 'Roboto Bold';
}

.private-tabs {
    padding: 0;
    border-radius: 5px;
    border: 1px solid #ccc;
    overflow: hidden;
    background-color: #fff !important;

}

.private-tabs .el-header {
    background-color: #f4f5f6;
    align-content: center;
    border-bottom: 1px solid #ccc;
}

.private-tabs .el-header>.user-name {
    opacity: 0.4;
    color: #000;
}

.private-tabs .el-main {
    padding: 0;
}
.basic-section {
    width: 100%;
}

.is-active {
    border-right: 2px solid;
}
</style>