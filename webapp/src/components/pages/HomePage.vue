<script lang="js">
import axios from 'axios'
import { inject } from 'vue'
export default {
    name: 'HomePage',
    data() {
        return {
            // novels: [],
            novels: [],
        }
    },
    mounted() {
        this.loadNovel()
    },
    computed: {
        $api() {
            return inject('$api')
        }
    },
    methods: {
        async loadNovel() {
            var url = "http://localhost:10454/api/BookInfo/new-updates"
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
        <div class="cover-img"></div>
        <el-row>
            <div class="left-slide">
                <span class="demonstration">Mới cập nhật</span>
                <el-row :gutter="10">
                    <el-col v-for="(novel, index) in novels" :key="novel.bookId" :span="4">
                        <PostCard v-if="index < 20" :cover="novel.coverUrl != '' ? novel.coverUrl : undefined" :title="novel.bookTitle"
                            :novelId="novel.bookId"></PostCard>
                    </el-col>
                </el-row>
            </div>
        </el-row>


        <div class="flex-grow"></div>
        <el-row>
            <div class="left-slide">
                <span class="demonstration">Có thể bạn thích</span>
                <el-row :gutter="15">
                    <el-col v-for="(novel, index) in novels" :key="novel.bookId" :xs="10" :sm="8" :md="6" :lg="4"
                        :xl="6">
                        <PostCard v-if="index < 20" :cover="novel.coverUrl != '' ? novel.coverUrl : undefined" :title="novel.bookTitle"
                            :novelId="novel.bookId"></PostCard>
                    </el-col>
                </el-row>
            </div>
            <!-- <div class="right-slide">
                <span class="demonstration">Theo dõi</span>

            </div> -->
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
    /* margin-bottom: 24px; */
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
.cover-img {
    height: 300px;
    overflow: hidden;
    background-image: url('https://trusteid.mioa.gov.mk/wp-content/plugins/uix-page-builder/uixpb_templates/images/UixPageBuilderTmpl/default-cover-2.jpg');
    background-size: cover;
    background-repeat: no-repeat;
    background-attachment: scroll;
    background-position-y: center;
    background-position-x: center;
    margin-bottom: 20px;
}
/* .left-slide {
    width: 70%;
}

.right-slide {
    margin-left: 20px;
    width: calc(30% - 20px);
} */
</style>