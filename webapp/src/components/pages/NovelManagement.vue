<script lang="ts">
import { ref } from 'vue'
import axios from 'axios'
import dayjs from "dayjs"
import { mapActions, mapGetters } from 'vuex'
import type { Column } from 'element-plus'

export default {
    name: 'NovelManagement',
    data() {
        return {
            loading: false,
            search: ref(''),
            novels: [],
            currentPage: 1,
            pageSize: 20,
            // totalPage: 10,
            totalItems: 0,

        }
    },
    mounted() {
        this.loading = true
        this.loadNovel(0, 20, true)

    },
    computed: {
        filterTableData() {
            return this.novels.filter((data) =>
                !this.search ||
                data.title.toLowerCase().includes(this.search.toLowerCase()))
        },
        ...mapGetters('auth', {
            getUser: 'getAuthData'
        })
    },
    methods: {
        handleRowClick(row) {
            console.log(row)
            // var url = `/${this.novel.BookId}/${novelName}/${chaatperId}/${chapterTitle}`
            // var url = `/${novelName}/${chapterTitle}`
            // this.selectChapter({ChapterId: row.ChapterId, ChapterTitle: row.ChapterTitle, ChapterNum: row.ChapterNum})
            // this.$router.push(url);
            this.$router.push(`/dashboard/workspace/${row.BookId}`)
        },
        handleEdit(index, row) {
            this.$router.push(`/dashboard/edit-novel/${row.BookId}`)
        },
        handleDelete(index, row) {
            console.log(index, row)
        },
        async loadNovel(page, pageSize, asc) {

            var params = `uploaderId=${this.getUser.userId}&page=${page}&pageSize=${pageSize}&asc=${asc}`
            var url = `https://localhost:44367/api/BookInfo/filterUploader?${params}`
            await axios
                .get(url)
                .then(response => {
                    this.novels = JSON.parse(JSON.stringify(response.data.Items.map(item => {
                        item.UpdateDate = dayjs(item.UpdateDate).format("DD/MM/YYYY HH:mm:ss");
                        item.CreateDate = dayjs(item.CreateDate).format("DD/MM/YYYY HH:mm:ss");
                        return item;
                    })));
                    this.totalItems = response.data.TotalItem
                })
                .catch(e => {
                    console.error(e)
                })
            this.loading = false

        }
    }
}
</script>
<template>
    <dashboard-layout>
        <template #header-content>Truyện đã đăng</template>
        <div class="panel-default">
            <div class="panel-header">
                <span>Truyện của tôi</span>
            </div>
            <div class="panel-body">
                <el-table v-loading="loading" :data="filterTableData" style="width: 100%" @row-click="handleRowClick">
                    <!-- <el-table-column width="50">
                        <template #default="scope">
                            <div style="display: flex; align-items: center">
                                <el-image :src="scope.row.BookCover" :fit="'scale-down'" style="height: 50px; width: 50px"/>
                            </div>
                        </template>
                    </el-table-column> -->
                    <el-table-column label="Tên truyện" prop="BookTitle" />
                    <el-table-column label="Tác giả" prop="AuthorName" />
                    <el-table-column label="Trạng thái">
                        <template #default="scope">
                            <el-select v-model="scope.row.Status" class="m-2">
                                
                                <el-option label="Đang tiến hành" value="1"></el-option>
                                <el-option label="Hoàn thành" value="2"></el-option>
                                <el-option label="Tạm ngưng" value="0"></el-option>
                            </el-select>
                        </template>
                    </el-table-column>
                    <el-table-column label="Ngày cập nhật" prop="UpdateDate" />
                    <el-table-column :align="'right'">
                        <template #header>
                            <el-input v-model="search" size="small" placeholder="Tìm kiếm tên truyện" />
                        </template>
                        <template #default="scope">
                            <el-button size="small" @click="handleEdit(scope.$index, scope.row)">Edit</el-button>
                            <el-button size="small" type="danger"
                                @click="handleDelete(scope.$index, scope.row)">Delete</el-button>
                        </template>
                    </el-table-column>
                </el-table>
            </div>
        </div>
    </dashboard-layout>
</template>

<style scoped>
.panel-default {
    display: flex;
    flex-direction: column;
    gap: 10px;
    margin: 15px;
    border: 1px solid #ccc;
    border-radius: 8px;
}

.panel-header {
    background-color: #f5f5f5;
    padding: 10px 15px;
    text-align: left;
    border-radius: 8px 8px 0 0;
    border-bottom: 1px solid #ccc;
}

.search-bar {
    float: right;
    margin-bottom: 20px;
}

:deep(.el-table__row:hover) {
    cursor: pointer;
}
</style>
