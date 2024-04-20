<script lang="ts">
import axios from 'axios'
import { mapActions, mapGetters } from 'vuex';
export default {
    name: 'TheHeader',
    data() {
        return {
            activeIndex: 0,
            dialogFormVisible: false,
            genres: [] as {
                genreId: String,
                genreName: String,
                description: String,
                status: String,
            }[],
            errors: [],
        }
    },
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
        }
    },
    computed: {
        ...mapGetters('auth', {
            getterLoginStatus: 'getLoginStatus',
            gettersAuthData: 'getAuthData',
            getterActiveToken: 'isTokenActive'
        })
    },
    created() {
        this.loadGenres();
    },
    methods: {
        ...mapActions('auth', {
            actionLogout: 'logout'
        }),
        login(dialogVisible) {
            this.dialogFormVisible = dialogVisible;
        },
        logout() {
            this.actionLogout(); 
            if (this.$route.path.includes('dashboard')) {
                this.$router.replace('/')
            } else {
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
        }

    },
}
</script>

<template>
    <el-header :class="classList" :height="`${height}px`">
        <el-menu :default-active="this.$route.path"  class="el-menu-demo" mode="horizontal" :ellipsis="false" :router="true">
            <el-menu-item index="/" class="logo">
                <img src="@/assets/logo/logo.png" :height="height">
            </el-menu-item>
            <div class="flex-grow"></div>
            <div class="searchBar" index="/Search" v-if="!reading && !dashboard">
                <el-autocomplete placeholder="Tìm kiếm"></el-autocomplete>
            </div>
            <el-sub-menu index="/danh-muc" v-if="!dashboard">
                <template #title>Danh mục</template>
                <!-- <el-menu-item index="/danh-muc/tan-van">Tản văn</el-menu-item> -->
                <el-menu-item v-for="genre in genres" :key="genre.genreId">{{ genre.genreName }}</el-menu-item>
            </el-sub-menu>
            <el-menu-item index="/dashboard" v-if="!dashboard">Sáng tác</el-menu-item>
            <el-menu-item index="/xep-hang" v-if="!dashboard">Xếp hạng</el-menu-item>
            <el-menu-item class="signInBtn" v-if="!getterLoginStatus" style="align-self: center;" @click="dialogFormVisible = true">
                <template #title>Đăng nhập</template>
            </el-menu-item>
            <el-sub-menu index="6" v-if="getterLoginStatus">
                <template #title>{{ gettersAuthData.name }}</template>
                <el-menu-item index="6-1">Tài khoản</el-menu-item>
                <el-menu-item index="6-2">Kệ sách</el-menu-item>
                <el-menu-item index="6-3">Đánh dấu</el-menu-item>
                <el-menu-item index="5-4">Lịch sử đọc</el-menu-item>
                <el-menu-item index="5-5">Hệ thống</el-menu-item>
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
    z-index: 1;
    padding: 0;
    position: sticky;
    top: 0px;
    background-color: #242424;
}
.el-header .el-menu {
    height: 100%;
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
</style>