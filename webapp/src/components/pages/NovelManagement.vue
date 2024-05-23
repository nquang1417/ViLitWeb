<script>
import { inject, ref } from 'vue'
import axios from 'axios'
import dayjs from "dayjs"
import { mapActions, mapGetters } from 'vuex'


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
        this.loadNovel(1)
        this.$api = inject('$api')
    },
    computed: {
        filterTableData() {
            return this.novels.filter((data) =>                
                !this.search ||
                data.bookTitle.toLowerCase().includes(this.search.toLowerCase()))
        },
        ...mapGetters('auth', {
            getUser: 'getAuthData'
        })
    },
    methods: {
        handleRowClick(row, col) {
            if (col.no != 4) {
                this.$router.push(`/dashboard/workspace/${row.bookId}`)
            }
        },
        handleEdit(index, row) {
            this.$router.push(`/dashboard/edit-novel/${row.bookId}`)
        },
        async handleDelete(index, row) {
            var token = this.getUser.token
            var owner = this.getUser.userId
            await this.$api.novels.deleteNovel(row.bookId, token, owner)
            // var url = `http://localhost:10454/api/BookInfo/delete?bookId=${row.bookId}`
            // await axios.delete(url, )
                .then(response => {
                    if (response.status == 200) {
                        this.novels = this.novels.filter(novel => novel.bookId != row.bookId)
                    }
                })
                .catch(e => {
                    console.error(e)
                })
        },
        async loadNovel(page) {

            var url = `http://localhost:10454/api/BookInfo/filter?page=${page}`
            await axios
                .post(url, {
                    uploaderId: `${this.getUser.userId}`
                })
                .then(response => {
                    this.novels = JSON.parse(JSON.stringify(response.data.data.map(item => {
                        item.updateDate = dayjs(item.updateDate).format("DD/MM/YYYY HH:mm:ss");
                        item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm:ss");
                        switch (item.status) {
                            case 0:
                                item.statusStr = "Tạm ngưng"
                                break
                            case 1:
                                item.statusStr = "Đang tiến hành"
                                break
                            case 2:
                                item.statusStr = "Hoàn thành"
                                break
                            case 3:
                                item.statusStr = "Tạm khóa"
                                break
                        }
                        return item;
                    })));
                    this.totalItems = response.data.totals
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
                    
                    <el-table-column label="Tên truyện" prop="bookTitle" />
                    <el-table-column label="Tác giả" prop="authorName" />
                    <el-table-column label="Trạng thái" prop="statusStr">
                        <!-- <template #default="scope">
                            <el-select v-model="scope.row.status" class="m-2">                                
                                <el-option label="Đang tiến hành" :value=1></el-option>
                                <el-option label="Hoàn thành" :value=2></el-option>
                                <el-option label="Tạm ngưng" :value=0></el-option>
                            </el-select>
                        </template> -->
                    </el-table-column>
                    <el-table-column label="Ngày cập nhật" prop="updateDate" />
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
