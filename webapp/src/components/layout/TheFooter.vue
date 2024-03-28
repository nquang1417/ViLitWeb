<script lang="ts">
export default {
    name: 'TheFooter',
    data() {
        return {
            options: [
                {
                    value: 'Tiếng Việt',
                    label: 'Tiếng Việt',
                },
                {
                    value: 'English',
                    label: 'English',
                },
            ],
            value: 'Tiếng Việt'
        }
    },
}
</script>

<template>
    <el-footer height="60">
        <el-row :gutter="12">
            <el-col :span="3">
                <img src="@/assets/logo/logo.png" height="60">
            </el-col>
            <div class="flex-grow"></div>
            <el-col :span="5">Liên hệ</el-col>
            <el-col :span="5">Tài nguyên</el-col>
            <el-col :span="5">Đội ngũ</el-col>
            <el-col :span="3">
                <el-select v-model="value" placeholder="Select">
                    <el-option v-for="item in options" :key="item.value" :label="item.label" :value="item.value"/>
                </el-select>
            </el-col>
        </el-row>
    </el-footer>
</template>

<style scoped>
.el-footer {
    /* position: static; */
    /* bottom: 0; */
    background-color: #8888f8 !important;
    z-index: 100;
    color: #fff;
    /* width: 100%; */
    margin-top: 5px;
}

.el-row {
    padding-top: 5px;
}

.flex-grow {
    flex-grow: 1;
}
</style>