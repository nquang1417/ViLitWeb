<script>
import { inject, ref } from 'vue'
import axios from 'axios'
import dayjs from "dayjs"
import { mapActions, mapGetters } from 'vuex'
import { ElMessage } from 'element-plus'
export default {
    name: 'ReportManagement',
    data() {
        return {
            loading: false,
            search: ref(''),
            reports: [],
            currentPage: 1,
            pageSize: 20,
            totalItems: 0,
            filter: {},
            formVisible: false,
            selectedReport: {}
        }
    },
    computed: {
        filterTableData() {
            return this.reports.filter(this.checkFilter)
        },
        ...mapGetters('auth', {
            getUser: 'getAuthData'
        })
    },
    mounted() {
        this.$api = inject('$api')        
        this.loadReports(1)
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
        async loadReports(page) {
            await this.$api.reports.filter({}, page, this.getUser.token)
                .then(response => {
                    this.reports = JSON.parse(JSON.stringify(response.data.data.map(item => {                        
                        switch (item.status) {
                            case 0:
                                item.statusStr = "Chưa xử lý"
                                break
                            case 1:
                                item.statusStr = "Đã xử lý"
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
            this.selectedReport = row
            if (col.no != 3) {
                this.formVisible = true
            }
        },
        async handleDelete(index, row) {
            var token = this.getUser.token
            await this.$api.reports.delete(row.reportsId, token)
                .then(response => {
                    if (response.status == 200) {
                        this.reports = this.reports.filter(user => user.reportsId != row.reportsId)
                        ElMessage({message:'Xóa báo cáo thành công!', type: 'success', duration: 1500})
                    }
                })
                .catch(error => {
                    console.error(error)
                })
        },
        async save(myForm) {
            await this.$api.reports.update(myForm.formData, this.getUser.token)
                .then(response => {
                    if (response.status == 200) {
                        this.loadReports(this.currentPage)
                        ElMessage({message: "Cập nhật thành công", type: 'success', duration: 1500})
                    }
                })
                .catch(error => {
                    console.error(error)
                })
            this.formVisible = false
        },
        mouseEnter(row) {
            row.hovered = true
        },
        mouseLeave(row) {
            row.hovered = false
        },
        handleCurrentChange(val) {
            this.loadReports(val)
        }
    },
}
</script>

<template>
    <admin-layout>
        <template #header-content>Quản lí báo cáo</template>
        <template #dialog>
            <!-- <user-info :dialogVisible="this.formVisible"></user-info>         -->
            <el-dialog v-model="formVisible" width="70%" top="50px" title="Thông tin báo cáo" >
                <report-info :data="this.selectedReport" :admin="true" :reportType="this.selectedReport.reportedType"  ref="form"></report-info>
                <template #footer>
                    <div class="dialog-footer">
                        <el-button type="primary" @click="save(this.$refs.form)">Lưu</el-button>
                        <el-button @click="this.formVisible = false">Hủy</el-button>                        
                    </div>
                </template>
            </el-dialog>
        </template>
        <div class="panel-default">
            <div class="panel-header">
                <span>Danh sách báo cáo</span>
            </div>
            <div class="panel-extra">
                <!-- thêm các bộ lọc -->
                <!-- <div class="filter-item">
                    <span class="filter-item__label">Tìm kiếm: </span>
                    <el-tooltip class="box-item filter-item__input" effect="dark"
                        content="Tìm kiếm theo username hoặc email" placement="top-start">
                        <el-input v-model="search" size="medium" placeholder="Tìm kiếm ..." />
                    </el-tooltip>
                </div> -->
                <div class="filter-item">
                    <span class="filter-item__label">Lọc trạng thái: </span>
                    <el-tooltip class="box-item filter-item__input" effect="dark" content="Lọc theo trạng thái"
                        placement="top-start">
                        <el-select v-model="filter.status" placeholder="---Chọn trạng thái---" style="width: 200px" class="m-2" clearable>
                            <el-option label="Đã xử lý" :value=1></el-option>
                            <!-- <el-option label="Tạm khóa" :value=2></el-option> -->
                            <el-option label="Chưa xử lý" :value=0></el-option>
                        </el-select>
                    </el-tooltip>
                </div>
            </div>
            <div class="panel-body">
                <el-table v-loading="loading" :data="filterTableData" style="width: 100%" @row-click="handleRowClick"
                    @cell-mouse-enter="mouseEnter" @cell-mouse-leave="mouseLeave">

                    <el-table-column label="Tên báo cáo" prop="reportName" width="200" show-overflow-tooltip />
                    <!-- <el-table-column label="Id Người báo cáo" prop="senderId" width="150" show-overflow-tooltip /> -->

                    <!-- <el-table-column label="Id đối tượng bị báo cáo" prop="reportedEntityId" show-overflow-tooltip /> -->
                    <el-table-column label="Nội dung" prop="message" show-overflow-tooltip />
                    
                    <el-table-column label="Ngày tạo" prop="createDate" width="200" show-overflow-tooltip sortable :formatter="formatDate"/>
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