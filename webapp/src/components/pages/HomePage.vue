<script lang="ts">
import axios from 'axios'
export default {
    name: 'HomePage',
    data() {
        return {
            // novels: [],
            novels: [] as {
                BookId: String,
                BookTitle: String,
                GenreId: String,
                Description: String,
                BookCover: String,
                AuthorName: String,
                UploaderId: String,
                Chapters: Number,
                LanguageCode: Number,
                LanguageName: String,
                Url: String,
                Status: String,
                CreateDate: String,
                CreateBy: String,
                UpdateDate: String,
                UpdateBy: String,
                coverUrl: String,
            }[]
        }
    },
    mounted() {
        this.loadNovel()
    },
    methods: {
        async loadNovel() {
            var url = "https://localhost:44367/api/BookInfo"
            try {
                var response = await axios.get(url);
                this.novels = JSON.parse(JSON.stringify(response.data));
               
                this.novels.forEach(novel => {
                    // novel.coverUrl = `src/assets/uploads/${novel.Url}/${novel.BookCover}`;
                    
                    novel.coverUrl = novel.BookCover
                })
            } catch (e) {
                console.error(e);
                this.novels = [
                    { 
                        BookId: '1',
                        BookTitle: 'book1',
                    },
                    {
                        BookId: '2',
                        BookTitle: 'book2',
                    },
                    {
                        BookId: '3',
                        BookTitle: 'book3',
                    },
                    {
                        BookId: '4',
                        BookTitle: 'book4',
                    },
                    {
                        BookId: '5',
                        BookTitle: 'book5',
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
            <el-col v-for="(novel, index) in novels" :key="novel.BookId" :xs="10" :sm="8" :md="6" :lg="4" :xl="6" >
                <PostCard :cover="novel.coverUrl" :title="novel.BookTitle" :novelId="novel.BookId"></PostCard>
            </el-col>
        </el-row>
        <div class="flex-grow"></div>
        <span class="demonstration">Có thể bạn thích</span>
        <el-row :gutter="12">
            <el-col v-for="(novel, index) in novels" :key="novel.BookId" :xs="10" :sm="8" :md="6" :lg="4" :xl="6" >
                <PostCard :cover="novel.coverUrl" :title="novel.BookTitle" :novelId="novel.BookId"></PostCard>
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