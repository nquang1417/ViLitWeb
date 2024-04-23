<script>
export default {
    name: 'FontSetup',
    data() {
        return {
            dialogFormVisible: false,
            pageStyle: {
                fontFamily: 'Roboto',
                fontSize: 16,
                textAlign: 'justify',
                padding: 20,
            },
            fonts: [
                'Roboto', 'Minion', 'Bookly', 'Times New Roman'
            ]
        }
    },
    props: {
        classList: String,
    },
    watch: {
        pageStyle: {
            handler(value) {
                console.log(value)
                this.$emit('changeStyle', this.pageStyle)
            },
            deep: true
        }
    },
    methods: {
        handleClick() {
            this.dialogFormVisible = true
        }
    },
}
</script>

<template>
    <el-button :class="classList" type="default" @click="handleClick">
        <i class="fa-solid fa-font"></i>
    </el-button>
    <el-dialog v-model="dialogFormVisible" title="Tùy chỉnh" style="width: 30%;">
        <el-form :label-width="100">
            <el-form-item label="Font">
                <el-select v-model="pageStyle.fontFamily">
                    <el-option v-for="font in fonts" :key="fonts.indexOf(font)" :value="font" :label="font"></el-option>
                </el-select>
            </el-form-item>
            <el-form-item label="Cỡ chữ">
                <el-slider v-model="pageStyle.fontSize" :step="2" :min="10" :max="64" show-input></el-slider>
            </el-form-item>
            <el-form-item label="Căn chỉnh">
                <el-radio-group v-model="pageStyle.textAlign">
                    <el-radio-button label="left">
                        <i class="fa-solid fa-align-left"></i>
                    </el-radio-button>
                    <el-radio-button label="center">
                        <i class="fa-solid fa-align-center"></i>
                    </el-radio-button>
                    <el-radio-button label="right">
                        <i class="fa-solid fa-align-right"></i>
                    </el-radio-button>
                    <el-radio-button label="justify">
                        <i class="fa-solid fa-align-justify"></i>
                    </el-radio-button>
                </el-radio-group>
            </el-form-item>
            <el-form-item label="Căn lề">
                <el-input-number v-model="pageStyle.padding" class="mx-4" :min="15" :max="40" style="width:100px"/>
            </el-form-item>
        </el-form>
    </el-dialog>
</template>

<style scoped>
.float-menu-item {
    margin: 0;
    border: none;
    font-size: 24px;
    height: fit-content;
    color: #242424;
    border-radius: 6px;
    padding: 15px;
}

.float-menu-item:hover {
    color: #10b18e;
}

.el-slider {
    margin-top: 0;
    margin-left: 12px;
}

:deep(.el-slider .el-input-number) {
    width: 100px;
}

</style>