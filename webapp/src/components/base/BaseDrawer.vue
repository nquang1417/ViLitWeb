<script lang="ts">
export default {
    name:'BaseDrawer',
    data() {
        return {
            drawer: false,
            direction: 'ltr',            
        }
    },
    props: {
        classList: String,
        title: String,
        chapters: Array
    },
    methods: {
        handleClick() {
            this.drawer = true
        }
    },
}
</script>

<template>
    <slot name="button"></slot>
    <el-button :class="classList" type="default"  @click="handleClick">
        <i class="fa-solid fa-info"></i>
    </el-button>
    <el-drawer v-model="drawer" title="I am the title" :direction="direction">
        <template #header>
            <div class="novel-title">
                <img class="cover" src="https://phutungnhapkhauchinhhang.com/wp-content/uploads/2020/06/default-thumbnail.jpg" height="60" alt="cover"/>
                
                <div class="title">I am the title</div> 
            </div>                       
        </template>
        <ul class="chapters-list">
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">asdfaaaaaaaaaaassssssaaaaaaaaaaaaaaaaaaaaaaaaaaasdf</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
            <li class="chapters-list-item">
                <a href="#">dsadfasdfasdfa</a>
            </li>
        </ul>
    </el-drawer>
    
</template>


<style scoped>
.novel-title {
    display: flex;
    align-items: center;
}
.novel-title .cover {
    margin-right: 5px;
    max-width: 50px;
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

.chapters-list {
    list-style: none;
    padding: 0;
    display: flex;
    flex-direction: column;   
}

.chapters-list .chapters-list-item {
    
    display: block;
    box-sizing: border-box;
    padding: 10px;
    border-radius: 6px;
}
.chapters-list .chapters-list-item:hover {
    background-color: #eee;
    cursor: pointer;
}


.chapters-list .chapters-list-item a {
    margin-bottom: 5px;
    word-wrap: break-word;
    color: #242424;
}

</style>

<style>
.el-drawer__header {
    background-color: #242424;
    color: #fff;
    margin: 0;
    padding: 20px;
}
.el-drawer__body {
    overflow: auto;
    align-items: center;
}

</style>