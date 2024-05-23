<script>
import { inject } from 'vue'
import { mapGetters } from 'vuex'
import dayjs from "dayjs"
import { ElMessage } from 'element-plus'

export default {
    name: 'ProfilePage',
    data() {
        return {
            coverUrl: 'https://i.ibb.co/jWnvwM3/607021.jpg',
            avaUrl: 'https://i.docln.net/lightnovel/users/ua17930-17338602-c691-409d-a5c6-6c3f1ed3d84e.jpg',
            editAllow: false,
            userData: {},
            uploadNovel: [],
            totalItems: 0,
            chaptersUploaded: 0,
            uploadUrl: `http://localhost:10454/api/Uploads/upload-image`,
        }
    },
    props: ['userId'],
    mounted() {
        if (this.getUser.userId == this.userId) {
            this.editAllow = true
        }
        this.$api = inject('$api')
        this.loadUser()
        this.loadUploadNovel(1)
    },
    computed: {
        ...mapGetters('auth', {
            getUser: 'getAuthData',
            getAuthStatus: 'getLoginStatus'
        }),        
    },
    methods: {
        async loadUser() {
            await this.$api.users.getUser(this.userId)
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
        async loadUploadNovel(page) {
            var filter = {
                uploaderId: `${this.userId}`
            }
            await this.$api.novels.filter(filter, page)
                .then(response => {
                    this.uploadNovel = JSON.parse(JSON.stringify(response.data.data.map(item => {
                        item.updateDate = dayjs(item.updateDate).format("DD/MM/YYYY HH:mm:ss");
                        item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm:ss");
                        return item;
                    })));
                    this.totalItems = response.data.totals
                })
                .catch(error => {
                    console.error(error)
                })
        },
        
        handleErrorAva() {
            this.avaUrl = 'https://i0.wp.com/sbcf.fr/wp-content/uploads/2018/03/sbcf-default-avatar.png'
        },
        async updateUser(successMessage) {
            var token = this.getUser.token
            await this.$api.users.updateUser(this.userData, token)
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({message: successMessage, type: 'success', duration: 1500})
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        beforeUpload(rawFile) {
            if (rawFile.type !== 'image/jpeg') {
                this.$message.error('Định dạng ảnh bìa phải là jpg!');
                return false;
            } else if (rawFile.size / 1024 / 1024 > 24) {
                this.$message.error('Ảnh bìa không được có kích thước lớn hơn 24MB!');
                return false;
            }
            return true;
        },
        handleSuccessCover(response, uploadFile) {
            this.userData.cover = response.data.image.url
            this.coverUrl = response.data.image.url
            this.updateUser('Cập nhật ảnh bìa thành công')
        },
        handleSuccessAva(response, uploadFile) {
            this.userData.avatar = response.data.image.url
            this.avaUrl = response.data.image.url
            this.updateUser('Cập nhật ảnh đại diện thành công')
        },
    },
    watch: {
        getAuthStatus(newVal, oldVal) {
            if (oldVal == false && newVal == true) {
                this.$router.go()
            }
        }
    }
}
</script>
<template>
    <account-layout>
        <div class="profile-feature">
            <div class="profile-cover">
                <div class="content-img" :style="`background-image: url(${coverUrl});`"></div>
                <el-upload class="cover-uploader" :action="this.uploadUrl" :show-file-list="false" 
                    :on-success="handleSuccessCover" :before-upload="beforeUpload" v-if="editAllow">
                    <template #default>
                        <div class="profile-changer-cover">
                            <i class="fas fa-camera" style="color: #fff; z-index: 2;"></i>
                            <span class="p-c__text" style="margin-left: 5px;">Yêu cầu 1200 x 300 px</span>
                        </div>
                    </template>
                </el-upload>
            </div>
            <div class="profile-nav">
                <div class="profile-ava-wrapper">
                    <div class="profile-ava">
                        <el-image :src="avaUrl" @error="handleErrorAva"/>
                        <el-upload class="ava-uploader" :action="this.uploadUrl" :show-file-list="false" 
                            :on-success="handleSuccessAva" :before-upload="beforeUpload" v-if="editAllow">
                            <template #default>
                                <div class="profile-changer-ava">
                                    <span class="changer-icon">
                                        <i class="fas fa-camera"></i>
                                    </span>
                                </div>
                            </template>
                        </el-upload>
                    </div>

                </div>
                <div class="profile-intro">
                    <span class="profile-name">{{this.userData.displayName}}  </span>
                    <span v-if="this.userData.gender==1" style="margin-left: 8px;"><i class="fa-solid fa-mars"></i></span>
                    <span v-if="this.userData.gender==2" style="margin-left: 8px;"><i class="fa-solid fa-venus"></i></span>
                    <span v-if="this.userData.gender==0" style="margin-left: 8px;"><i class="fa-solid fa-venus-mars"></i></span>
                </div>
            </div>
        </div>
        <el-container class="profile-main">
            <el-aside class="profile-info">
                <el-container direction="vertical" class="basic-section">
                    <el-row class="statistic-wrapper">
                        <el-row class="statistic-body">
                            <el-col :span="12" class="statistic-item">
                                <el-row class="statistic-value txt-center item-center"><span
                                        style="font-size:22px; font-family: 'Roboto Bold';">{{this.userData.uploadItems}}</span></el-row>
                                <el-row class="statistic-name txt-center item-center">Truyện đã đăng</el-row>
                            </el-col>
                            <el-col :span="12" class="statistic-item txt-center">
                                <el-row class="statistic-value txt-center item-center"><span
                                        style="font-size:22px; font-family: 'Roboto Bold';">{{this.userData.followingItems}}</span></el-row>
                                <el-row class="statistic-name txt-center item-center">Đang theo dõi</el-row>
                            </el-col>
                        </el-row>
                        <el-row class="statistic-body">
                            <el-col :span="24" class="statistic-item txt-center">
                                <el-row class="statistic-value txt-center item-center"><span
                                        style="font-size:22px; font-family: 'Roboto Bold';">{{this.userData.comments}}</span></el-row>
                                <el-row class="statistic-name txt-center item-center">Bình luận</el-row>
                            </el-col>
                        </el-row>
                    </el-row>
                    <div class="divider"></div>
                    <el-row class="statistic-wrapper">                        
                        <el-row class="statistic-body">
                            <el-col :span="24" class="statistic-item txt-center">
                                <el-row class="statistic-value">
                                    <span style="color:#bbb">
                                        <i class="fa-solid fa-calendar" style="margin-right: 3px;"></i> Tham gia:
                                    </span>
                                    <span style="margin-left: 5px">{{ this.userData.formatCreateDate }}</span>
                                </el-row>
                            </el-col>
                        </el-row>
                    </el-row>
                </el-container>
            </el-aside>
            <el-main class="profile-data">
                <!-- <el-container class="profile-showcase">
                    <el-header>
                        <span class="number">0</span>
                        <span class="showcase-title">Truyện đang theo dõi</span>
                    </el-header>
                    <el-main>
                        không có truyện nào
                    </el-main>
                </el-container> -->
                <el-container class="profile-showcase">
                    <el-header>
                        <span class="number">{{ totalItems }}</span>
                        <span class="showcase-title">Truyện đã đăng</span>
                    </el-header>
                    <el-main>
                        <span v-if="this.totalItems <= 0">Không có truyện nào</span>
                        <el-row :gutter="15" v-else>
                            <el-col v-for="(novel, index) in uploadNovel" :key="novel.bookId" :span="6">
                                <post-card :cover="novel.bookCover != '' ? novel.bookCover : undefined" :title="novel.bookTitle" :novelId="novel.bookId"></post-card>
                                
                            </el-col>
                        </el-row>
                    </el-main>
                </el-container>
            </el-main>
        </el-container>
    </account-layout>
</template>

<style scoped>
.profile-feature {
    min-height: 384px;
    position: relative;
    border: 1px solid #ccc;
    border-radius: 5px;
    overflow: hidden;
    margin-bottom: 20px;
    /* padding-bottom: 10px; */
    background-color: #fff !important;
}

.profile-main>.profile-info {
    background-color: #fff !important;
    padding: 10px 0;
    border-radius: 5px;
    border: 1px solid #ccc;
    overflow: hidden;
}

.profile-data {
    padding: 10px;
    border-radius: 5px;
    overflow: hidden;
    margin-left: 20px;
}
.profile-cover {
    min-height: 300px;
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

.profile-ava:hover .profile-changer-ava {
    position: absolute;
    bottom: 0;
    left: 0;
    width: 100%;
    text-align: center;
    z-index: 2;
    color: #fff;
    line-height: 3rem;
    background-color: rgb(0,0,0,0.6);
}



.profile-ava .changer-icon {
    display: none;
    width:100%;
}

.profile-ava:hover .changer-icon {
    display: block;
    width: 100%;

}
.profile-changer-cover {
    position: absolute;
    top: 0;
    left: 0;
    border-radius: 5px;
    margin-left: 5px;
    margin-top: 5px;
    text-align: left;
    padding: 5px;
    z-index: 2;
    color: #fff;
    font-size: 13px;    
}

.profile-cover:hover .profile-changer-cover {
    background-color: rgb(0,0,0,0.6);
}

.profile-cover:hover .profile-changer-cover>.p-c__text {
    display: inline;
}
.p-c__text {
    display: none;
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

.profile-intro {
    padding-left: 180px;
    height: 100%;
}
.profile-intro>.profile-name {
    font-size: 1.35rem;
    font-family: 'Roboto Bold';
}

.basic-section {
    width: 100%;
}

.basic-section>.statistic-wrapper {
    width: 100%;
    justify-content: center;
    padding: 15px 0;
}

.divider {
    border-bottom: 1px solid #ccc;
}

.basic-section>.statistic-wrapper>.statistic-body {
    width: 100%;
}


.statistic-body>.statistic-item>.el-row {
    width: 100%;
    padding: 0 10px;
    text-align: center;
    align-items: center;
}

.statistic-body>.statistic-item>.statistic-name {
    color: #bbb;
}



.item-center {
    justify-content: center;
}

.profile-showcase>.el-header {
    height: 36px;
    border-bottom: 3px solid #000;
    font-family: 'Roboto Bold';
    align-content: center;
    font-size: 18px;
    padding: 0;
}

.profile-showcase>.el-main {
    padding: 15px 0;
}

.profile-showcase>.el-main>.el-row {
    width: 100%;
}

.profile-showcase>.el-header>.number {
    display: inline-block;
    background-color: #000;
    color: #fff;
    height: 36px;
    width: 36px;
    line-height: 36px;
    text-align: center;
    margin-right: 10px;
}
</style>