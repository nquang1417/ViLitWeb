<template>
    <el-container class="dashboard-layout">
        <slot name="dialog"></slot>
        <the-header :classList="{ 'is-hidden': !showHeader }" :height="40" dashboard></the-header>
        <div class="background-img"></div>
        <el-container>
            <el-aside>
                <el-row>
                    <el-button type="default" class="menu-open" icon="Expand" @click="handleMenu"
                        v-if="isCollapse"></el-button>
                    <el-button type="default" icon="House" class="menu-home" @click="this.$router.push('/dashboard')"
                        v-if="!isCollapse">
                    </el-button>
                    <el-button type="default" class="menu-close" icon="Fold" @click="handleMenu"
                        v-if="!isCollapse"></el-button>
                </el-row>
                <el-menu :default-active="this.$route.path" :router="true" mode="vertical" class="el-menu-vertical-demo"
                    :collapse="isCollapse">

                    <el-menu-item index="/dashboard">
                        <el-icon>
                            <DataAnalysis />
                        </el-icon>
                        <template #title>Dashboard</template>
                    </el-menu-item>
                    <el-sub-menu index="#">
                        <template #title>
                            <el-icon><Collection /></el-icon>
                            <span>Sáng tác</span>
                        </template>
                        <el-menu-item index="/dashboard/novel-management">Truyện đã đăng</el-menu-item>
                        <el-menu-item index="/dashboard/new-novel">Thêm truyện</el-menu-item>
                    </el-sub-menu>
                    <el-menu-item index="">
                        <el-icon>
                            <document />
                        </el-icon>
                        <template #title>Navigator Three</template>
                    </el-menu-item>
                    <el-menu-item index="">
                        <el-icon>
                            <setting />
                        </el-icon>
                        <template #title>Settings</template>
                    </el-menu-item>
                </el-menu>
            </el-aside>
            <el-main>
                <el-page-header :icon="null">
                    <template #title>
                        <span class="text-large font-600 mr-3 title">Dashboard</span>
                    </template>
                    <template #content>
                        <span class="text-large font-600 mr-3">
                            <slot name="header-content"></slot>
                        </span>
                    </template>
                    <template #extra>
                        <slot name="header-extra"></slot>
                    </template>
                    <template #default>
                        <slot name="header-main"></slot>
                    </template>
                </el-page-header>
                <slot></slot>
            </el-main>
        </el-container>
        <!-- <the-footer></the-footer> -->
    </el-container>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
export default {
    name: 'DashboardLayout',
    data() {
        return {
            isCollapse: true,
            isSignedIn: true,
            showHeader: true,
            lastScrollPosition: 0,
            scrollOffset: 20,
        };
    },
    mounted() {
        this.lastScrollPosition = window.scrollY
        window.addEventListener('scroll', this.onScroll)
    },
    beforeDestroy() {
        window.removeEventListener('scroll', this.onScroll)
    },
    methods: {
        handleMenu() {
            this.isCollapse = !this.isCollapse;
        },
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

.el-main {
    padding: 10px;
    overflow: visible;
    background-color: #fff;
    z-index: 1;
    color: #242424;
}


.el-aside {
    border-right: #dcdfe6 1px solid;
    width: max-content;
    z-index: 1;
    background-color: #fff;

}

.el-menu {
    border: none;
}

.dashboard-header {
    height: 36px;
    /* margin-bottom: 5px; */
    padding: 0;
}

.dashboard-header__menu {
    height: 36px;
    border-bottom: 1px solid #ccc;
}

.menu-open {
    border: none;
}

.menu-open:hover {
    background-color: #fff;
}

.menu-home {
    border: none;
    margin-left: 10px;
    background-color: #ecf5ff;

}
/* 
.menu-home:hover {
    background-color: #dce4ff;
} */

.menu-close {
    border: none;
    background-color: #ecf5ff;

}
/* 
.menu-close:hover {
    background-color: #dce4ff;

} */

.el-row:has(.menu-open) {
    justify-content: center;
    height: 50px;
    align-content: center;
    align-items: center;
}

.el-row:has(.menu-close) {
    justify-content: space-between;
    height: 50px;
    align-content: center;
    align-items: center;
    /* border-bottom: 1px solid #ccc; */
    background-color: #ecf5ff;

}</style>