<script>
export default {
    name: 'ReportForm',
    props: ['data', 'formRef', 'reportType'],
    data() {
        return {
            formData: {},
        }
    },
    emits: ['update:data'],
    watch: {
        formData: {
            handler() {
                this.$emit('update:data', this.formData)
            },
            deep: true
        },
        data: {
            handler() {
                this.formData = {...this.data}
            },
            deep: true
        }
    },
    mounted() {
        this.formData.reportedType = this.reportType
    },
}
</script>
<template>
    <div class="info-dialog">
        <el-form ref="editForm" class="info-dialog__form" :model="formData" label-position="right" label-width="auto">

            <el-form-item>                
                <el-radio-group v-model="formData.reportName" v-if="reportType==1">
                    <el-radio value="Nội dung khiêu dâm">Nội dung khiêu dâm</el-radio>
                    <el-radio value="Hành vi tiêu cực">Hành vi tiêu cực</el-radio>
                    <el-radio value="Tiết lộ thông tin cá nhân">Tiết lộ thông tin cá nhân</el-radio>
                    <el-radio value="Các nội dung không phù hợp khác">Các nội dung không phù hợp khác</el-radio>
                </el-radio-group>
                <el-radio-group v-model="formData.reportName" v-if="reportType==0">
                    <el-radio value="Lời lẽ khiêu khích">Lời lẽ khiêu khích</el-radio>
                    <el-radio value="Phân biệt chủng tộc">Phân biệt chủng tộc</el-radio>
                    <el-radio value="Tiết lộ thông tin mật">Tiết lộ thông tin mật</el-radio>
                    <el-radio value="Các nội dung không phù hợp khác">Các nội dung không phù hợp khác</el-radio>
                </el-radio-group>
            </el-form-item>
            <el-form-item >
                <el-input type="textarea" placeholder="Nhập lý do của bạn ở đây ..." v-model="formData.message" :autosize="{ minRows: 5 }"></el-input>
            </el-form-item>
        </el-form>
    </div>
</template>

<style scoped>
.el-select {
    flex: 1;
}

.el-input {
    flex: 1;
}


.el-radio-group {
    flex-direction: column;
    align-items: baseline;
}
</style>