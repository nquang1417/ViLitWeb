<script>
export default {
    name: 'DashboardHome',

}
</script>
<template>
    <!-- <el-row :gutter="16">
        <el-col :span="8">
            <div class="statistic-card">
                <el-statistic :value="95000">
                    <template #title>
                        <div style="display: inline-flex; align-items: center; padding-top: 20px">
                            Daily active users
                            <el-tooltip effect="dark" content="Number of users who logged into the product in one day"
                                placement="top">
                                <el-icon style="margin-left: 4px" :size="12">
                                    <Warning />
                                </el-icon>
                            </el-tooltip>
                        </div>
                    </template>
                </el-statistic>
                <div class="statistic-footer">
                    <div class="footer-item">
                        <span>than yesterday</span>
                        <span class="green">
                            24%
                            <el-icon>
                                <CaretTop />
                            </el-icon>
                        </span>
                    </div>
                </div>
            </div>
        </el-col>
        <el-col :span="8">
            <div class="statistic-card">
                <el-statistic :value="693700">
                    <template #title>
                        <div style="display: inline-flex; align-items: center; padding-top: 20px">
                            Monthly Active Users
                            <el-tooltip effect="dark" content="Number of users who logged into the product in one month"
                                placement="top">
                                <el-icon style="margin-left: 4px" :size="12">
                                    <Warning />
                                </el-icon>
                            </el-tooltip>
                        </div>
                    </template>
                </el-statistic>
                <div class="statistic-footer">
                    <div class="footer-item">
                        <span>month on month</span>
                        <span class="red">
                            12%
                            <el-icon>
                                <CaretBottom />
                            </el-icon>
                        </span>
                    </div>
                </div>
            </div>
        </el-col>
        <el-col :span="8">
            <div class="statistic-card">
                <el-statistic :value="72000" title="New transactions today">
                    <template #title>
                        <div style="display: inline-flex; align-items: center; padding-top: 20px">
                            New transactions today
                        </div>
                    </template>
                </el-statistic>
                <div class="statistic-footer">
                    <div class="footer-item">
                        <span>than yesterday</span>
                        <span class="green">
                            16%
                            <el-icon>
                                <CaretTop />
                            </el-icon>
                        </span>
                    </div>
                    <div class="footer-item">
                        <el-icon :size="14">
                            <ArrowRight />
                        </el-icon>
                    </div>
                </div>
            </div>
        </el-col>
    </el-row> -->
</template>

<style scoped>
:global(h2#card-usage ~ .example .example-showcase) {
    background-color: var(--el-fill-color) !important;
}

.el-statistic {
    --el-statistic-content-font-size: 28px;
}

.el-row:has(.statistic-card) {
    margin: 15px 0;
}

.statistic-card {
    height: 100%;
    padding: 0 20px;
    border-radius: 4px;
    border: 1px solid #eee;
    background-color: #f0f4ff;
}

.statistic-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
    font-size: 12px;
    color: var(--el-text-color-regular);
    margin-top: 16px;
    padding-bottom: 20px;
}

.statistic-footer .footer-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.statistic-footer .footer-item span:last-child {
    display: inline-flex;
    align-items: center;
    margin-left: 4px;
}

.green {
    color: var(--el-color-success);
}

.red {
    color: var(--el-color-error);
}
</style>