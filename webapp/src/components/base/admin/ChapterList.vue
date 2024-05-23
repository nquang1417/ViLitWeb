<script>
import {inject} from 'vue'
import dayjs from "dayjs"

export default {
    name: 'ChapterList',
    data() {
        return {
            chapters: [],
            totalItems: 0,
            pageSize: 20,
            currentPage: 1,
        }
    },
    props: ['novelId'],
    emits: ['chapter-row-click'],
    mounted() {
        this.$api = inject('$api')
        this.loadChapters(this.novelId, 1)
    },
    watch: {
        novelId() {
            this.loadChapters(this.novelId, 1)
        }
    },  
    methods: {
        async loadChapters(novelId, page) {
            await this.$api.chapters.getChapters(novelId, page)
                .then(response => {
                    this.chapters = JSON.parse(JSON.stringify(response.data.data.map(item => {
                        item.updateDate = dayjs(item.updateDate).format("DD/MM/YYYY HH:mm");
                        item.createDate = dayjs(item.createDate).format("DD/MM/YYYY HH:mm");
                        return item;
                    })));
                    this.totalItems = response.data.totals
                })
        },
        handleRowClick(row, col) {
            this.$emit('chapter-row-click', row)
        },
        handleCurrentChange(val) {
            this.loadChapters(this.novelId, val)
        }
    }
}
</script>

<template>
    <div class="chapter-list">
        <el-table :data="this.chapters" stripe style="width: 100%" v-loading="loading" @row-click="handleRowClick"
            @cell-mouse-enter="mouseEnter" @cell-mouse-leave="mouseLeave"
            :default-sort="{ prop: 'chapterNum', order: 'ascending' }">
            <el-table-column prop="chapterNum" label="STT" :align="'left'" width="80" />
            <el-table-column prop="chapterTitle" label="Tiêu đề" :align="'left'" min-width="200" />
            <el-table-column prop="updateDate" label="Ngày cập nhật" :align="'right'">

            </el-table-column>
            <template #empty>
                <div>Không có bản nháp nào</div>
                <el-button type="primary" @click="newDraft">Tạo bản nháp mới</el-button>
            </template>
        </el-table>
        <div class="pagination" height="40px">
            <div class="flex-grow"></div>
            <el-pagination v-model:current-page="currentPage" v-model:page-size="pageSize" :background="true"
                layout="prev, pager, next, jumper" :total="totalItems" @current-change="handleCurrentChange" />
        </div>
    </div>
</template>

<style scoped>
.chapter-list {
    max-height: 50vh;
    overflow: auto;
}

.pagination {
    display: flex;
    background-color: #f5f5f5;
    padding: 5px 0;
}

.flex-grow {
    flex-grow: 1;
}
</style>