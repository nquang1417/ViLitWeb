<template>
    <el-container class="layout-default">
        <div class="background-img"></div>
        <the-header :classList="{ 'is-hidden': !showHeader }" ></the-header>
        <el-main>
            <slot></slot>
        </el-main>
        <!-- <the-footer></the-footer> -->
    </el-container>
</template>

<script>
export default {
    name: 'LayoutDefault',
    data() {
        return {
            showHeader: true,
            lastScrollPosition: 0,
            scrollOffset: 40,
        }
    },
    mounted() {
        this.lastScrollPosition = window.scrollY
        window.addEventListener('scroll', this.onScroll)
    },
    beforeDestroy() {
        window.removeEventListener('scroll', this.onScroll)
    },
    methods: {
        // Toggle if navigation is shown or hidden
        onScroll() {
            if (window.scrollY < 0) {
                return
            }
            if (Math.abs(window.scrollY - this.lastScrollPosition) < this.scrollOffset) {
                return
            }
            this.showHeader = window.scrollY < this.lastScrollPosition
            this.lastScrollPosition = window.scrollY
        },
    },
}
</script>

<style scoped>
.background-img {
  /* background-image: url('@/assets/background.jpg'); */
  background-size: cover;
  background-repeat: no-repeat;
  background-attachment: scroll;
  background-position-y: center;
  background-position-x: center;
  opacity: 0.85;
  content: "";
  position: fixed;
  top: 0px;
  right: 0px;
  bottom: 0px;
  left: 0px;
}

.el-main {
    padding: 0;
    padding-top: 30px;
    overflow: visible;
    background-color: #fff;
    z-index: 1;
}

.el-aside {
    background-color: blue;
    z-index: 10;
}
</style>