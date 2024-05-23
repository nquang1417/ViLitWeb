<script>
export default {
    name: 'AccountLayout',
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

<template>
    <the-header :classList="{ 'is-hidden': !showHeader }" :height="40"></the-header>
    <el-container class="account-layout">
        <slot name="dialog"></slot>
        <el-main class="main-part">
            <slot></slot>
        </el-main>
    </el-container>
</template>

<style scoped>

.main-part {
    min-height: 564px;
}

</style>