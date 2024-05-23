<script>
import { inject, ref } from 'vue'
import { mapActions, mapGetters } from 'vuex'
import { ElMessageBox, ElMessage } from 'element-plus'
import dayjs from "dayjs"
export default {
    name: 'AdminNovel',
    data() {
        return {
            novels: [],
            currentPage: 1,
            pageSize: 20,
            totalItems: 0,
            filter: {},
            formVisible: false,
            loading: false,
            search: ref(''),
            selectedNovel: {},
            selectedChapter: {},
            chapterTableVisible: false,
            chapterVisible: false,
            lockVisible: false,
            lockedInfo: {},
        }
    },
    computed: {
        filterTableData() {
            return this.novels.filter(this.checkFilter)
        },
        ...mapGetters('auth', {
            getUser: 'getAuthData'
        })
    },
    mounted() {
        this.$api = inject('$api')
        this.loadNovel(1)
    },
    methods: {
        checkFilter(data) {
            var result = !this.search 
                         || data.bookTitle.toLowerCase().startsWith(this.search.toLowerCase()) 
                         || data.authorName.toLowerCase().startsWith(this.search.toLowerCase())
            if (this.filter.status != null && typeof (this.filter.status) == 'number') {                
                result = result && (data.status === this.filter.status)                
            }
            return result
        },
        formatDate(row, col) {
            var date = dayjs(row.createDate).format("DD/MM/YYYY HH:mm:ss")
            return date
        },
        async loadNovel(page) {
            await this.$api.novels.filter({},page)
                .then(response => {
                    this.novels = JSON.parse(JSON.stringify(response.data.data.map(item => {
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
                    })))
                    this.totalItems = response.data.totals
                })
                .catch(error => {
                    console.error(error)
                })
        },
        handleRowClick(row, col) {
            this.selectedNovel = row
            if (col.no != 4) {
                this.formVisible = true
            }
        },
        handleDelete(index, row) {
            ElMessageBox.alert(
                `Xóa vĩnh viễn tiểu thuyết khỏi cơ sở dữ liệu. \n Bạn có muốn tiếp tục?`,
                'Cảnh báo',
                {
                    confirmButtonText: 'OK',
                    cancelButtonText: 'Cancel',
                    type: 'warning',
                }
            )
                .then(() => {
                    var token = this.getUser.token
                    var owner = row.uploaderId
                    var bookId = row.bookId
                    return this.$api.novels.deleteNovel(bookId, token, owner)                    
                })
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({
                            type: 'success',
                            message: 'Xóa tiểu thuyết thành công', duration: 1500
                        })
                        this.loadNovel(this.currentPage)
                    }
                })
        },
        handleListClick() {
            this.chapterTableVisible = true
        },
        mouseEnter(row) {
            row.hovered = true
        },
        mouseLeave(row) {
            row.hovered = false
        },  
        async save(myForm) { 
            var bookId = this.selectedNovel.bookId
            var token = this.getUser.token
            var owner = this.selectedNovel.uploaderId
            await this.$api.novels.updateNovel(bookId, myForm.formData, token, owner)
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({message: 'Cập nhật thành công', type: 'success', duration: 1500})
                        this.loadNovel(this.currentPage)
                    }
                })
            
            this.formVisible = false
        },
        handleChapterRowClick(row) {
            this.chapterVisible = true
            this.loadChapter(row.chapterId)
        },
        async loadChapter(id) {
            await this.$api.chapters.getChapterById(id)
                .then(response => {
                    this.selectedChapter = response.data.chapter
                    var  selectedChapterContent = response.data.file.fileContents
                    var decodedContent = atob(selectedChapterContent)
                    var utf8decoder = new TextDecoder('utf-8')
                    var text = utf8decoder.decode(new Uint8Array([...decodedContent].map(char => char.charCodeAt(0))))
                    this.selectedChapter.fileContents = text
                })
        },
        async lockNovel() {
            var bookId = this.selectedNovel.bookId
            var token = this.getUser.token
            await this.$api.novels.lockNovel(bookId, this.lockedInfo, token)
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({ message: `Đã khóa tiểu thuyết ${this.selectedNovel.bookTitle}`, type: 'success', duration: 1500 })
                        this.loadNovel(this.currentPage)
                    }
                })
                .catch(error => {
                    console.error(error)
                })
            this.chapterTableVisible = false
            this.chapterVisible = false
            this.formVisible = false
            this.lockVisible = false
        },
        async unlockNovel() {
            var bookId = this.selectedNovel.bookId
            var token = this.getUser.token
            await this.$api.novels.unlockNovel(bookId, token)
                .then(response => {
                    if (response.status == 200) {
                        ElMessage({ message: `Đã mở khóa tiểu thuyết ${this.selectedNovel.bookTitle}`, type: 'success', duration: 1500 })
                        this.loadNovel(this.currentPage)
                    }
                })
                .catch(error => {
                    console.error(error)
                })
            this.formVisible = false
        },
        handleCurrentChange(val) {
            this.loadNovel(val)
        }
    }
}
</script>

<template>
    <admin-layout>
        <template #header-content>Quản lý tiểu thuyết</template>
        <template #dialog>
            <el-dialog v-model="lockVisible" width="30%" title="Xác nhận khóa tiểu thuyết" >
                <el-form ref="banForm" :model="lockedInfo" label-position="right" label-width="auto">
                    <el-form-item label="Thời hạn">
                        <el-select v-model="lockedInfo.duration" placeholder="-- Chọn thời hạn khóa tài khoản --">
                            <el-option label="3 Ngày" :value="3"></el-option>
                            <el-option label="7 Ngày" :value="7"></el-option>
                            <el-option label="30 Ngày" :value="30"></el-option>
                            <el-option label="3 Tháng" :value="90"></el-option>
                            <el-option label="Vô thời hạn" :value="9999"></el-option>
                        </el-select>
                    </el-form-item>
                    <el-form-item label="Lý do">
                        <el-input type="textarea" v-model="lockedInfo.reason"  :autosize="{ minRows: 5 }"></el-input>
                    </el-form-item>
                </el-form>
                <template #footer>
                    <div class="dialog-footer">
                        <div class="flex-grow"></div>
                        <el-button type="primary" @click="lockNovel">Xác nhận</el-button>
                        <el-button @click="this.lockVisible = false">Hủy</el-button>                        
                    </div>
                </template>
            </el-dialog>
            <el-dialog v-model="formVisible" width="70%" top="50px" title="Thông tin tiểu thuyết">
                <novel-info :novelId="this.selectedNovel.bookId" ref="form"></novel-info>
                <template #footer>
                    <div class="dialog-footer">
                        <el-button @click="handleListClick">Danh sách chương</el-button>
                        <el-button type="danger" plain @click="this.lockVisible = true" v-if="this.selectedNovel.status != 3">Khóa truyện</el-button>
                        <el-button type="danger" plain @click="unlockNovel" v-if="this.selectedNovel.status == 3">Mở Khóa truyện</el-button>
                        <div class="flex-grow"></div>
                        <el-button type="primary"  @click="save(this.$refs.form)">Lưu</el-button>
                        <el-button @click="this.formVisible = false">Đóng</el-button>                        
                    </div>
                </template>
            </el-dialog>

            <el-dialog v-model="chapterTableVisible" width="70%" title="Danh sách chương">
                <chapter-list :novelId="this.selectedNovel.bookId" ref="chapterList" @chapter-row-click="(row) => handleChapterRowClick(row)"></chapter-list>
                <template #footer>
                    <div class="dialog-footer">
                        <div class="flex-grow"></div>
                        <!-- <el-button type="primary"  @click="">Lưu</el-button> -->
                        <el-button @click="this.chapterTableVisible = false">Đóng</el-button>                        
                    </div>
                </template>
            </el-dialog>
            <el-dialog v-model="chapterVisible" width="60%" top="50px" title="Nội dung chương">
                <div class="content-wrapper" v-html="this.selectedChapter.fileContents"></div>
                <template #footer>
                    <div class="dialog-footer">
                        <div class="flex-grow"></div>
                        <!-- <el-button type="primary"  @click="">Lưu</el-button> -->
                        <el-button @click="this.chapterVisible = false">Đóng</el-button>                        
                    </div>
                </template>
            </el-dialog>
        </template>
        <div class="panel-default">
            <div class="panel-header">
                <span>Danh sách tiểu thuyết</span>
            </div>
            <div class="panel-extra">
                <!-- thêm các bộ lọc -->
                <div class="filter-item">
                    <span class="filter-item__label">Tìm kiếm: </span>
                    <el-tooltip class="box-item filter-item__input" effect="dark"
                        content="Tìm kiếm theo tên tiểu thuyết, tên tác giả" placement="top-start">
                        <el-input v-model="search" size="medium" placeholder="Tìm kiếm ..." />
                    </el-tooltip>
                </div>
                <div class="filter-item">
                    <span class="filter-item__label">Lọc trạng thái: </span>
                    <el-tooltip class="box-item filter-item__input" effect="dark" content="Lọc theo trạng thái"
                        placement="top-start">
                        <el-select v-model="filter.status" placeholder="---Chọn trạng thái---" class="m-2" clearable style="width: 200px">
                            <el-option label="Đang tiến hành" :value=1></el-option>
                            <el-option label="Hoàn thành" :value=2></el-option>
                            <el-option label="Tạm ngưng" :value=0></el-option>
                        </el-select>
                    </el-tooltip>
                </div>
            </div>
            <div class="panel-body">
                <el-table v-loading="loading" :data="filterTableData" style="width: 100%" @row-click="handleRowClick"
                    @cell-mouse-enter="mouseEnter" @cell-mouse-leave="mouseLeave">

                    <el-table-column label="Tên truyện" prop="bookTitle" width="300" show-overflow-tooltip />
                    <el-table-column label="Tên tác giả" prop="authorName" width="200" show-overflow-tooltip />                    
                    <el-table-column label="Ngày đăng" prop="createDate" show-overflow-tooltip sortable
                        :formatter="formatDate" />
                    <el-table-column label="Ngày cập nhật" prop="updateDate" show-overflow-tooltip sortable
                        :formatter="formatDate" />
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
.flex-grow {
    flex-grow: 1;
}

.panel-footer {
    display: flex;
    background-color: #f5f5f5;
    padding: 5px 0;
}

.dialog-footer {
    display: flex;
}
.panel-default {
    display: flex;
    flex-direction: column;
    margin: 15px;
    border: 1px solid #ccc;
    border-radius: 8px;
}

.content-wrapper {
    max-height: 70vh;
    overflow: auto;
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