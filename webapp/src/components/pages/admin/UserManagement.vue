<script>
import { ref } from 'vue'
import axios from 'axios'
import dayjs from "dayjs"
import { mapActions, mapGetters } from 'vuex'

export default {
    name: 'UserManagement',
    data() {
        return {
            loading: false,
            search: ref(''),
            users: [],
            currentPage: 1,
            pageSize: 20,
            totalItems: 0,
            filter: {},
            formVisible: false,
            selectedUser: {}
        }
    },
    computed: {
        filterTableData() {
            return this.users.filter(this.checkFilter)
        },
        ...mapGetters('auth', {
            getUser: 'getAuthData'
        })
    },
    mounted() {
        this.loadUsers(1)
    },
    methods: {
        checkFilter(data) {
            //console.log(data)
            var result = !this.search || data.username.startsWith(this.search) || data.email.startsWith(this.search)
            if (this.filter.status != null && typeof (this.filter.status) == 'number') {
                console.log(typeof (this.filter.status))
                result = result && (data.status === this.filter.status)
                // console.log(`${data.username} ${data.status} ${result} ${this.filter.status}`)
            }
            return result
        },
        async loadUsers(page) {
            var url = `http://localhost:10454/api/Users/all`
            await axios
                .get(url, {
                    headers: {
                        'access_token': `${this.getUser.token}`
                    }
                })
                .then(response => {
                    this.users = JSON.parse(JSON.stringify(response.data.map(item => {
                        item.updateDate = dayjs(item.updateDate).format("DD/MM/YYYY HH:mm:ss");
                        item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm:ss");
                        switch (item.gender) {
                            case 1:
                                item.genderStr = 'Nam'
                                break
                            case 2:
                                item.genderStr = "Nữ"
                                break
                            case 0:
                                item.genderStr = "Khác"
                                break
                        }
                        switch (item.status) {
                            case 0:
                                item.statusStr = "Không hoạt động"
                                break
                            case 1:
                                item.statusStr = "Đang hoạt động"
                                break
                            case 2:
                                item.statusStr = "Tạm khóa"
                                break
                            case 3:
                                item.statusStr = "Báo cáo vi phạm"
                                break
                        }
                        return item;
                    })));
                    this.totalItems = response.data.length
                })
        },
        handleRowClick(row, col) {
            if (col.no != 5) {
                this.formVisible = true
                this.selectedUser = row
            }
        },
        save() {
            this.formVisible = false
        },
        mouseEnter(row) {
            row.hovered = true
        },
        mouseLeave(row) {
            row.hovered = false
        },
    },
    watch: {
        // selectedUser: {            
        //     handler(value) {
        //         console.log(`${value.gender} `)
        //         switch (value.gender) {
        //             case 1:
        //                 value.genderStr = 'Nam'
        //                 break
        //             case 2:
        //                 value.genderStr = "Nữ"
        //                 break
        //             case 0:
        //                 value.genderStr = "Khác"
        //                 break
        //         }
        //         switch (value.status) {
        //             case 0:
        //                 value.statusStr = "Không hoạt động"
        //                 break
        //             case 1:
        //                 value.statusStr = "Đang hoạt động"
        //                 break
        //             case 2:
        //                 value.statusStr = "Tạm khóa"
        //                 break
        //             case 3:
        //                 value.statusStr = "Báo cáo vi phạm"
        //                 break
        //         }
        //     },
        //     deep: true
        // }
    }
}
</script>

<template>
    <admin-layout>
        <template #header-content>Users Management</template>
        <template #dialog>
            <!-- <user-info :dialogVisible="this.formVisible"></user-info>         -->
            <el-dialog v-model="formVisible" width="80%" title="User Details" >
                <user-info :data="this.selectedUser"
                    @update:data="(newVal) => { console.log(newVal); this.selectedUser = newVal }"></user-info>
                <template #footer>
                    <div class="dialog-footer">
                        <el-button type="primary" @click="save">Lưu</el-button>
                        <el-button @click="this.formVisible = false">Hủy</el-button>
                        
                    </div>
                </template>
            </el-dialog>
        </template>
        <div class="panel-default">
            <div class="panel-header">
                <span>Danh sách người dùng</span>
            </div>
            <div class="panel-extra">
                <!-- thêm các bộ lọc -->
                <div class="filter-item">
                    <span class="filter-item__label">Tìm kiếm: </span>
                    <el-tooltip class="box-item filter-item__input" effect="dark"
                        content="Tìm kiếm theo username hoặc email" placement="top-start">
                        <el-input v-model="search" size="medium" placeholder="Tìm kiếm ..." />
                    </el-tooltip>
                </div>
                <div class="filter-item">
                    <span class="filter-item__label">Tìm kiếm: </span>
                    <el-tooltip class="box-item filter-item__input" effect="dark" content="Lọc theo trạng thái"
                        placement="top-start">
                        <el-select v-model="filter.status" placeholder="Chọn trạng thái" class="m-2" clearable>
                            <el-option label="Đang hoạt động" :value=1></el-option>
                            <el-option label="Tạm khóa" :value=2></el-option>
                            <el-option label="Không hoạt động" :value=0></el-option>
                        </el-select>
                    </el-tooltip>
                </div>
            </div>
            <div class="panel-body">
                <el-table v-loading="loading" :data="filterTableData" style="width: 100%" @row-click="handleRowClick"
                    @cell-mouse-enter="mouseEnter" @cell-mouse-leave="mouseLeave">

                    <el-table-column label="Username" prop="username" width="120" show-overflow-tooltip />
                    <el-table-column label="Tên người dùng" prop="displayName" width="150" show-overflow-tooltip />

                    <el-table-column label="Email" prop="email" show-overflow-tooltip />
                    <el-table-column label="Giới tính" prop="genderStr" width="80" show-overflow-tooltip />
                    <el-table-column label="Ngày tạo" prop="createDate" show-overflow-tooltip sortable />
                    <el-table-column label="Trạng thái" prop="statusStr" width="200" resizable>

                        <!-- <template #default="scope">
                            <el-select v-model="scope.row.status" class="m-2">
                                <el-option label="Đang hoạt động" :value=1></el-option>
                                <el-option label="Tạm khóa" :value=2></el-option>
                                <el-option label="Không hoạt động" :value=0></el-option>
                            </el-select>
                        </template> -->
                        <template #default="scope">
                            <div class="delete-options" v-if="scope.row.hovered">
                                <div>{{ scope.row.statusStr }}</div>
                                <el-button size="small" type="danger"
                                    @click="handleDelete(scope.$index, scope.row)">Delete</el-button>
                            </div>
                        </template>
                    </el-table-column>
                    <!-- <el-table-column :align="'right'">
                        <template #header>
                            <el-tooltip class="box-item" effect="dark" content="Tìm kiếm theo username hoặc email"
                                placement="top-start">
                                <el-input v-model="search" size="small" placeholder="Tìm kiếm ..." />
                            </el-tooltip>
                        </template>
                        <template #default="scope">
                            
                        </template>
                    </el-table-column> -->
                </el-table>
            </div>
        </div>

    </admin-layout>
</template>

<style scoped>
.panel-default {
    display: flex;
    flex-direction: column;
    margin: 15px;
    border: 1px solid #ccc;
    border-radius: 8px;
}

.panel-extra {
    display: flex;
    padding: 10px 15px;
    gap: 10px;
    text-align: left;
    border-radius: 8px 8px 0 0;
    border-bottom: 1px solid #ccc;
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

.filter-item {
    display: flex;
    gap: 10px;
    align-items: center;
}

.filter-item__label {
    min-width: 80px;
}

.delete-options {
    display: flex;
    justify-content: space-between;
}

:deep(.el-table__row:hover) {
    cursor: pointer;
}
</style>