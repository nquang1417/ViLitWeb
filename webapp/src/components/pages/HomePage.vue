<script lang="js">
import axios from 'axios'
export default {
    name: 'HomePage',
    data() {
        return {
            // novels: [],
            novels: []
        }
    },
    mounted() {
        this.loadNovel()
    },
    methods: {
        async loadNovel() {
            var url = "http://localhost:10454/api/BookInfo/all"
            try {
                var response = await axios.get(url);
                this.novels = JSON.parse(JSON.stringify(response.data));
               
                this.novels.forEach(novel => {
                    // novel.coverUrl = `src/assets/uploads/${novel.Url}/${novel.BookCover}`;
                    
                    novel.coverUrl = novel.bookCover
                })
            } catch (e) {
                console.error(e);
                this.novels = [
                    { 
                        bookId: '1',
                        bookTitle: 'book1',
                    },
                    {
                        bookId: '2',
                        bookTitle: 'book2',
                    },
                    {
                        bookId: '3',
                        bookTitle: 'book3',
                    },
                    {
                        bookId: '4',
                        bookTitle: 'book4',
                    },
                    {
                        bookId: '5',
                        bookTitle: 'book5',
                    },
                ]
            }


        }
    }
}
</script>

<template>
    <layout-default>
        <!-- <el-row :gutter="64">
            <el-col :span="24">
                <span class="demonstration">Nổi bật</span>
                <BaseCarousel></BaseCarousel>
            </el-col>
            
        </el-row> -->
        <span class="demonstration">Mới cập nhật</span>
        <el-row :gutter="20">
            <el-col v-for="(novel, index) in novels" :key="novel.bookId" :xs="10" :sm="8" :md="6" :lg="4" :xl="6" >
                <PostCard :cover="novel.coverUrl" :title="novel.bookTitle" :novelId="novel.bookId"></PostCard>
            </el-col>
        </el-row>
        <div class="flex-grow"></div>
        <span class="demonstration">Có thể bạn thích</span>
        <el-row :gutter="12">
            <el-col v-for="(novel, index) in novels" :key="novel.bookId" :xs="10" :sm="8" :md="6" :lg="4" :xl="6" >
                <PostCard :cover="novel.coverUrl" :title="novel.bookTitle" :novelId="novel.bookId"></PostCard>
            </el-col>
        </el-row>
    </layout-default>
</template>

<style scoped>
.flex-grow {
    display: inline-flex;
    flex-grow: 1;
}

.home-page {
    padding: 0;
    overflow: auto;
}

.demonstration {
    display: block;
    color: #242424;
    font-size: 1.5rem;
    font-family: 'Roboto Medium';
    text-align: left;

}

.el-row {
    margin-bottom: 24px;
    max-width: inherit;
}

.m-list {
    list-style: none;
    text-align: left;
    color: #242424;
}

.m-list ul {
    padding: 0 10px;
}
</style>