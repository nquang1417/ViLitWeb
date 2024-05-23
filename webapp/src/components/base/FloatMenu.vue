<script lang="js">
export default {
    name: 'FloatMenu',
    props: ['next','prev', 'currentChapter', 'bookmarked'],
    methods: {
    },
}

</script>

<template>
    <div class="float-menu">
        <div class="menu-wrap">
            <el-button class="float-menu-item" type="default"  @click="this.$emit('prevClick')" :disabled="prev">
                <i class="fa-solid fa-backward"></i>
            </el-button>
            <div class="divider"></div>
            <el-button class="float-menu-item" type="default"  @click="this.$emit('homeClick')">
                <i class="fa-solid fa-house"></i>
            </el-button>
            <div class="divider"></div>
            <font-setup classList="float-menu-item" @changeStyle="(style) => this.$emit('changeStyle',style)"></font-setup>
            <div class="divider"></div>
            <div class="divider"></div>
            <el-button class="float-menu-item" type="default"  @click="this.$emit('saveBookmark')" v-if="!bookmarked">
                <!-- <i class="fa-solid fa-bookmark"></i> -->
                <i class="fa-solid fa-bookmark"></i>
            </el-button> 
            <el-button class="float-menu-item is-bookmarked" type="default"  @click="this.$emit('saveBookmark')" v-if="bookmarked">
                <!-- <i class="fa-solid fa-bookmark"></i> -->
                <i class="fa-solid fa-bookmark"></i>
            </el-button> 
            <div class="divider"></div>
            <el-button class="float-menu-item" type="default"  @click="this.$emit('nextClick')" :disabled="next">
                <i class="fa-solid fa-forward"></i>
            </el-button>
        </div>
    </div>
</template>

<style scoped>
.float-menu {
    position: fixed;
    bottom: 0;
    right: 0;
    margin-right: 20px;
    margin-bottom: 20px;
    z-index: 2044;
    background-color: #fff;
    overflow: hidden;
    border-radius: 6px;
}

.float-menu .menu-wrap {
    display: flex;
    flex-direction: column;    
    /* padding: 2px 0; */
    border: 1px solid #ccc;
    border-radius: 6px;
    
}

.float-menu-item {
    margin: 0;
    border: none;
    font-size: 24px;
    height: fit-content;
    border-radius: 6px;
    padding: 15px;
}

.float-menu-item:not(.is-disabled):not(.is-bookmarked){
    color: #242424;
} 

.is-bookmarked {
    color: #0070e0;
}

.float-menu-item:hover:not(.is-disabled) {
    color: #409eff;
}
.divider {
    border-bottom: 1px solid #ccc;
}
</style>