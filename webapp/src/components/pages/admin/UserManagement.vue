<script>
import { inject, ref } from 'vue'
import axios from 'axios'
import dayjs from "dayjs"
import { mapActions, mapGetters } from 'vuex'
import { ElMessage } from 'element-plus'
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
            selectedUser: {},
            bannedDialog: false,
            bannedInfo: {},
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
        this.$api = inject('$api')        
        this.loadUsers(1)
    },
    methods: {
        checkFilter(data) {
            var result = !this.search || data.username.startsWith(this.search) || data.email.startsWith(this.search)
            if (this.filter.status != null && typeof (this.filter.status) == 'number') {                
                result = result && (data.status === this.filter.status)                
            }
            return result
        },
        formatDate(row, col) {
            var date = dayjs(row.createDate).format("DD/MM/YYYY HH:mm:ss")
            return date
        },
        async loadUsers(page) {
            await this.$api.users.getUsersFilter(page, this.getUser.token)
                .then(response => {
                    this.users = JSON.parse(JSON.stringify(response.data.data.map(item => {
                        
                        // item.updateDate = dayjs(item.updateDate).format("DD/MM/YYYY HH:mm:ss");
                        // item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm:ss");
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
                    this.totalItems = response.data.totals
                })
                .catch(error => {
                    console.error(error)
                })
        },
        handleRowClick(row, col) {
            this.selectedUser = row
            if (col.no != 5) {
                this.formVisible = true
            }
        },
        async handleDelete(index, row) {
            var token = this.getUser.token
            await this.$api.users.deleteUser(row.userId, token)
                .then(response => {
                    if (response.status == 200) {
                        this.users = this.users.filter(user => user.userId != row.userId)
                        ElMessage({message:'Xóa người dùng thành công!', type: 'success', duration: 1500})
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async save(myForm) {
            if (this.selectedUser.status != 2) {
                if (myForm.formData.status != 2) {
                    await this.$api.users.updateUser(myForm.formData, this.getUser.token)
                        .then(() => {
                            ElMessage({message: 'Cập nhật thành công!', type: 'success', duration: 1500})
                            this.loadUsers(this.currentPage)
                        })
                        .catch(error => {
                            console.error(error)
                        })
                    this.formVisible = false
                } else {
                    this.bannedDialog = true
                }
            } else {
                if (myForm.formData.status != 2) {
                    await this.$api.users.unlockUser(myForm.formData.userId, this.getUser.token)
                        .then(() => {
                            ElMessage({message: `Đã mở khóa tài khoản người dùng ${this.selectedUser.username}`, type: 'success', duration: 1500})
                            this.loadUsers(this.currentPage)
                        })
                        .catch(error => {
                            console.error(error)
                        })
                    this.formVisible = false
                }
            }
        },
        mouseEnter(row) {
            row.hovered = true
        },
        mouseLeave(row) {
            row.hovered = false
        },
        async lockUser() {
            var userId = this.selectedUser.userId
            
            var token = this.getUser.token
            await this.$api.users.banUser(userId, this.bannedInfo, token)
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({ message: `Người dùng ${this.selectedUser.username} đã bị khóa`, type: 'success', duration: 1500 })
                        this.loadUsers(this.currentPage)
                    }
                })
                .catch(error => {
                    console.error(error)
                })
            this.bannedDialog = false
            this.formVisible = false
        },
        handleCurrentChange(val) {
            this.loadUsers(val)
        }
    },
}
</script>

<template>
    <admin-layout>
        <template #header-content>Quản lí người dùng</template>
        <template #dialog>
            <!-- <user-info :dialogVisible="this.formVisible"></user-info>         -->
            <el-dialog v-model="formVisible" width="70%" top="50px" title="Thông tin tài khoản" >
                <user-info :data="this.selectedUser" :admin="true" ref="form"></user-info>
                <template #footer>
                    <div class="dialog-footer">
                        <el-button type="primary" @click="save(this.$refs.form)">Lưu</el-button>
                        <el-button @click="this.formVisible = false">Hủy</el-button>                        
                    </div>
                </template>
            </el-dialog>
            <el-dialog v-model="bannedDialog" width="30%" title="Xác nhận khóa người dùng" >
                <el-form ref="banForm" :model="bannedInfo" label-position="right" label-width="auto">
                    <el-form-item label="Thời hạn">
                        <el-select v-model="bannedInfo.duration" placeholder="-- Chọn thời hạn khóa tài khoản --">
                            <el-option label="3 Ngày" :value="3"></el-option>
                            <el-option label="7 Ngày" :value="7"></el-option>
                            <el-option label="30 Ngày" :value="30"></el-option>
                            <el-option label="3 Tháng" :value="90"></el-option>
                            <el-option label="Vô thời hạn" :value="9999"></el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="Lý do">
                        <el-input type="textarea" v-model="bannedInfo.reason"  :autosize="{ minRows: 5 }"></el-input>
                    </el-form-item>
                </el-form>
                <template #footer>
                    <div class="dialog-footer">
                        <el-button type="primary" @click="lockUser">Xác nhận</el-button>
                        <el-button @click="this.bannedDialog = false">Hủy</el-button>                        
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
                    <span class="filter-item__label">Lọc trạng thái: </span>
                    <el-tooltip class="box-item filter-item__input" effect="dark" content="Lọc theo trạng thái"
                        placement="top-start">
                        <el-select v-model="filter.status" placeholder="---Chọn trạng thái---" style="width: 200px" class="m-2" clearable>
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
                    <el-table-column label="Ngày tạo" prop="createDate" show-overflow-tooltip sortable :formatter="formatDate"/>
                    <el-table-column label="Trạng thái" prop="statusStr" width="200" resizable>
                        <template #default="scope">
                            <div class="delete-options" v-if="scope.row.hovered">
                                <div>{{ scope.row.statusStr }}</div>
                                <el-button size="small" type="danger"
                                    @click="handleDelete(scope.$index, scope.row)">Delete</el-button>
                            </div>
                        </template>
                    </el-table-column>
                </el-table>
            </div>
            <div class="panel-footer">
                <div class="flex-grow"></div>
                <el-pagination v-model:current-page="currentPage" v-model:pageSize="pageSize" layout="prev, pager, next"
                    :total="totalItems" background @current-change="handleCurrentChange">
                </el-pagination>
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

.panel-footer {
    display: flex;
    background-color: #f5f5f5;
    padding: 5px 0;
}

.flex-grow {
    flex-grow: 1;
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