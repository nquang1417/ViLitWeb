<template>
    
</template>
  
<script lang="ts" setup>
import { ref } from 'vue'
import {
    Close,
    Document,
    Menu as IconMenu,
    Location,
    Menu,
    Setting,
} from '@element-plus/icons-vue'

const isCollapse = ref(true)
</script>
  
<style>
.el-menu-vertical-demo:not(.el-menu--collapse) {
    width: 200px;
    min-height: 400px;
}
</style>
  