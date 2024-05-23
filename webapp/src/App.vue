<script>
export default {
  name: 'App',
  components: [
  ],
  
}
</script>

<template>
    <!-- <the-header></the-header> -->
    
    <el-container class="page-view">
      <router-view/>
    </el-container>
    <the-footer></the-footer>
</template>

<style scoped>
.el-container {
  opacity: 1 !important;
  flex-direction: column;
  top: 0;
  left: 0;
  min-height: 100vh;
}

.page-view {
  margin: 0;
  width: 100%;
}

</style>
