<script>
export default {
    name: 'DashboardPage',
    data() {
        return {
        }
    },
}
</script>

<template>
    <dashboard-layout>
        <template #header-content>Tổng quan</template>
        <router-view ></router-view>
    </dashboard-layout>
</template>

<style scoped>

</style>