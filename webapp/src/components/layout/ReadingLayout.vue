<script lang="ts">
export default {
    name: 'ReadingLayout',
    data() {
        return {      
            showHeader: true,
            lastScrollPosition: 0,
            scrollOffset: 40,
        }
    },
    mounted() {
        this.lastScrollPosition = window.scrollY
        window.addEventListener('scroll', this.onScroll)
    },
    beforeDestroy() {
        window.removeEventListener('scroll', this.onScroll)
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
    },
}
</script>

<template>
    <el-container class="reading-layout">
        <div class="background-img"></div>
        <the-header :classList="{ 'is-hidden': !showHeader }" :height="40" reading></the-header>
        <!-- <el-header class="reading-header">
            <el-menu :default-active="this.$route.path" class="el-menu-demo reading-header__menu" mode="horizontal"
                :ellipsis="false" :router="true">
                <el-menu-item index="/" class="logo" style="padding: 0">
                    <img src="@/assets/logo/logo.png" height="36">
                </el-menu-item>
                <div class="flex-grow"></div>
                <el-menu-item class="signInBtn" v-if="!isSignedIn" style="align-self: center;">
                    <login-modal></login-modal>
                </el-menu-item>
                <el-sub-menu index="#" v-if="isSignedIn">
                    <template #title>Ngọc Quang</template>
                    <el-menu-item index="#">Tài khoản</el-menu-item>
                    <el-menu-item index="#">Kệ sách</el-menu-item>
                    <el-menu-item index="#">Đánh dấu</el-menu-item>
                    <el-menu-item index="#">Lịch sử đọc</el-menu-item>
                    <el-menu-item index="#">Hệ thống</el-menu-item>
                    <el-menu-item index="#">Đăng xuất</el-menu-item>
                </el-sub-menu>
            </el-menu>
        </el-header> -->
        <el-main>       
            <el-page-header :icon="null">
                <template #title>
                    <span class="text-large font-600 mr-3 title">
                        <slot name="header-content"></slot>
                    </span>
                </template>
                <slot name="chapter-nav"></slot>
            </el-page-header>
            <slot></slot>   
            <div class="footer-nav">
                <slot name="chapter-nav"></slot>
            </div>
            <div class="cmt-box">
                <slot name="comments"></slot>
            </div>            
        </el-main>
    </el-container>
</template>

<style scoped>
.background-img {
  /* background-image: url('@/assets/background.jpg'); */
  background-size: cover;
  background-repeat: no-repeat;
  background-attachment: scroll;
  background-position-y: center;
  background-position-x: center;
  opacity: 0.85;
  content: "";
  position: fixed;
  top: 0px;
  right: 0px;
  bottom: 0px;
  left: 0px;
}
.title {
    font-size: 20px;
}

.flex-grow {
    flex-grow: 1;
}

.el-header {
    width: 90%;
    margin: 0 auto;
    padding: 0;
}

.el-main {
    padding: 0;
    overflow: visible;
    background-color: #fff;
    z-index: 1;
    color: #242424;
}

.reading-layout .el-main {
    width: 90%;
    margin: 0 auto;
}

.reading-header {
    height: 36px;
    margin-bottom: 5px;
}

.reading-header__menu {
    height: 36px;
}

.el-page-header {
    padding: 20px;
    border: 1px solid #eee;
    border-radius: 6px;
    margin-bottom: 15px;
}

.footer-nav {
    margin-top: 15px;
    padding: 0 20px 20px;
    border: 1px solid #eee;
    border-radius: 6px;
}

.cmt-box {
    /* padding: 5px 15px 15px; */
    margin-top: 20px;
    border: 1px solid #eee;
    border-radius: 6px;
}
</style>