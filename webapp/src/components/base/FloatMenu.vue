<script lang="ts">
export default {
    name: 'FloatMenu',
    data() {
        return {
            
        }
    },
}

</script>

<template>
    <div class="float-menu">
        <div class="menu-wrap">
            <el-button class="float-menu-item" type="default"  @click="this.$router.push('/')">
                <i class="fa-solid fa-backward"></i>
            </el-button>
            <div class="divider"></div>
            <el-button class="float-menu-item" type="default"  @click="this.$router.push('/')">
                <i class="fa-solid fa-house"></i>
            </el-button>
            <div class="divider"></div>
            <!-- <el-button class="float-menu-item" type="default"  @click="this.$router.push('/')">
                <i class="fa-solid fa-font"></i>
            </el-button> -->
            <font-setup classList="float-menu-item" @changeStyle="(style) => this.$emit('changeStyle',style)"></font-setup>
            <div class="divider"></div>
            <!-- <el-button class="float-menu-item" type="default"  @click="this.$router.push('/')">
                <i class="fa-solid fa-info"></i>
            </el-button> -->
            <base-drawer classList="float-menu-item"></base-drawer>
            <div class="divider"></div>
            <el-button class="float-menu-item" type="default"  @click="this.$router.push('/')">
                <i class="fa-solid fa-bookmark"></i>
            </el-button>
            <div class="divider"></div>
            <el-button class="float-menu-item" type="default"  @click="this.$router.push('/')">
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
    color: #242424;
    border-radius: 6px;
    padding: 15px;
}

.float-menu-item:hover {
    color: #10b18e;
}
.divider {
    border-bottom: 1px solid #ccc;
}
</style>