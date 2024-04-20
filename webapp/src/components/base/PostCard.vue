<template>
    <el-card :body-style="{ padding: '0px' }" :id="novelId" @click="viewDetail()">
        <div class="thumb">
            <img :src="cover" class="image" />
            <div class="novel-title">{{ title }}</div>
        </div>
        
        <!-- <div style="padding: 14px">
            <div class="bottom">
                <time class="time">{{}}</time>
                <el-button class="button"> View </el-button>
            </div>
        </div> -->
    </el-card>
</template>
  
<script >

export default {
    name: 'PostCard',
    data() {
        return {
            coverUrl: {}
        }
    },
    props: {
        cover: {
            type: String,
            default: "https://shadow.elemecdn.com/app/element/hamburger.9cf7b091-55e9-11e9-a976-7f4d0b07eef6.png"
        },
        title: String,
        novelId: String,
    },
    computed: {
        imageUrl() {
            const relativePath = this.cover.replace(/\\/g, '/')
            console.log(relativePath)
            return require(`../${relativePath}`)
        }
    },
    methods: {
        viewDetail() {
            console.log(this.novelId)
            this.$router.push({
                path: `novel/${this.novelId}`,
            })
        }
    }
}
</script>
  
<style>
.el-card {
    cursor: pointer;
    margin: 10px 0;
}


.el-card:hover {
    scale: 1.025;
    color: #409EFF;
    border-color: #409EFF;
}

.time {
    font-size: 12px;
    color: #999;
}

.thumb {
    position: relative;
    
}

.novel-title {
    position: absolute;
    display: flex;
    align-items: center;
    justify-content: center;
    bottom: 0;
    text-align: center;
    width: 100%;
    background-color: #00000075;
    font-size: 13.5px;
    color: #fff;
    min-height: 32px;
    align-self: center;
}

.bottom {
    margin-top: 13px;
    line-height: 12px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.button {
    padding: 0;
    min-height: auto;
}

.image {
    width: 100%;
    display: block;
}
</style>
  