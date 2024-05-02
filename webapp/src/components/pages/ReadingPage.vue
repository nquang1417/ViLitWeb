<script lang="js">
import axios from 'axios'
import { pad } from '../../scripts/utils/utils.js'
import { mapActions, mapGetters } from 'vuex'
import { inject } from 'vue'

export default {
    name: 'ReadingPage',
    data() {
        return {
            chapters: [],
            dynamicStyle: {
                fontSize: 16,
                textAlign: 'justify',
                fontFamily: 'Roboto',
                padding: 20
            },
            novel: {},
            chapter: {},
            chapterContent: {},
            audioList: [],
            audioChapter: null,
            currentChapter: {
                key: '',
                value: 0,
                label: ''
            },
            prev: 0,
            next: 0,
        }
    },
    mounted() {
        this.loadChapter()
        this.loadNovel()
    },
    computed: {
        dynStyle() {
            var style = ""
            style += 'font-size:' + this.dynamicStyle.fontSize + 'px;'
            style += 'text-align:' + this.dynamicStyle.textAlign + ';'
            style += 'font-family:' + this.dynamicStyle.fontFamily + ';'
            style += 'padding:' + this.dynamicStyle.padding + 'px;'
            return style
        },
        noMore() {
            if (this.selectPage >= this.totalPage) {
                return true;
            } else {
                false
            }
        },
        $api() {
            return inject('$api')
        },
        ...mapGetters('novel', {
            getNovel: 'getNovelInfo',
            getChapter: 'getChapterInfo'
        }),
    },
    props: ['title', 'chapterId', 'chapterNo'],
    watch: {
        $route(to, from) {
            this.loadChapter()
            this.loadNovel()
        }
    },
    methods: {
        ...mapActions('novel', {
            updateChapter: 'updateChapter'
        }),
        async loadChapter() {
            var id = this.getChapter.chapterId
            console.log(this.$api)

            var url = `http://localhost:10454/api/BookChapters/get-chapter?chapterId=${id}`
            await axios.get(url)
                .then(response => {
                    this.chapter = response.data.chapter
                    this.chapterContent.file = response.data.file.fileContents
                    this.currentChapter.key = response.data.chapter.chapterId
                    this.currentChapter.value = response.data.chapter.chapterNum
                    this.currentChapter.label = response.data.chapter.chapterTitle
                    this.prev = this.currentChapter.value - 1
                    this.next = this.currentChapter.value + 1
                    return this.$api.chapters.getChapterAudio(id)
                })
                .then(response => {
                    var audioContent = new Blob([response.data], { type: 'audio/wav' })
                    var url = URL.createObjectURL(audioContent);
                    this.chapter.audio = url
                    this.audioList.push({
                        name: `${this.chapter.chapterTitle}`,
                        url: url
                    })
                })
                .catch(e => {
                    console.error(e)
                })
            var decodedContent = atob(this.chapterContent.file)
            var utf8decoder = new TextDecoder('utf-8')
            
            var text = utf8decoder.decode(new Uint8Array([...decodedContent].map(char => char.charCodeAt(0))))
            this.chapterContent.fileContents = text
            
        },
        handleBeforePlay(next) {
            // There are a few things you can do here...
            this.currentAudioName = this.audioList[this.$refs.audioPlayer.currentPlayIndex].name
            next() // Start playing
        },
        async loadNovel() {
            this.novel = this.getNovel
            // this.chapters = Array.from({ length: this.novel.Chapters }, (_, i) => ({                
            //     key: `${this.novelId}-${pad(i + 1, 5)}`,
            //     value: i + 1,
            //     label: `Chương ${i + 1}`
            // }))
            var url = `http://localhost:10454/api/BookChapters/all-summary?bookId=${this.getNovel.bookId}`
            await axios.get(url)
                .then(response => {
                    this.chapters = response.data.map(chapter => ({
                        key: chapter.chapterId,
                        value: chapter.chapterNum,
                        label: chapter.chapterTitle
                    }))
                })
                .catch(error => {
                    console.error(error)
                })
        },
        selectChapter(value) {
            var chapter = this.chapters.find(chapter => chapter.value == value)
            var chapTitle = `Chuong-${value}`
            var url = `/${this.title}/${chapTitle}`
            this.updateChapter({ chapterId: chapter.key, chapterTitle: chapTitle, chapterNum: value })
            this.$router.push(url);
        },
        nextChapter() {
            var value = this.currentChapter.value + 1
            var chapter = this.chapters.find(chapter => chapter.value == value)
            var chapTitle = `Chuong-${value}`
            var url = `/${this.title}/${chapTitle}`
            console.log(url)
            this.updateChapter({ chapterId: chapter.key, chapterTitle: chapTitle, chapterNum: value })
            this.$router.push(url);
        },
        prevChapter() {
            var value = this.currentChapter.value - 1
            var chapter = this.chapters.find(chapter => chapter.value == value)
            var chapTitle = `Chuong-${value}`
            var url = `/${this.title}/${chapTitle}`
            console.log(url)
            this.updateChapter({ chapterId: chapter.key, chapterTitle: chapTitle, chapterNum: value })
            this.$router.push(url);
        }

    }
}
</script>

<template>
    <reading-layout>
        <template #header-content>{{ this.novel.BookTitle }}</template>
        <template #chapter-nav>
            <div class="chapter-nav">
                <el-button class="prev" type="primary" @click="prevChapter">Chương trước</el-button>
                <!-- <el-select v-model="this.chapter.ChapterTitle" class="m-2">
                    
                </el-select> -->
                <el-select-v2 v-model="currentChapter.value" :options="chapters" placeholder="Please select"
                    @change="selectChapter" />
                <!-- <el-select v-model="dynamicStyle.fontSize" class="m-2" style="width: 60px;">
                    <el-option v-for="item in 46" :key="item" :label="item" :value="item">
                    </el-option>
                </el-select> -->
                <el-button class="next" type="primary" @click="nextChapter">Chương sau</el-button>

            </div>
        </template>
        <div class="audio">
            <!-- <audio-player ref="audioPlayer" :audio-list="this.audioList.map(elm => elm.url)" :before-play="handleBeforePlay"
                :show-prev-button="false" :show-next-button="false" theme-color="#409EFF" />                 -->
            <audio controls :src="this.chapter.audio" type="audio/wav"></audio>
        </div>
        <!-- <div class="content-wrapper">
            <pre class="chapter-content" :style="dynStyle">{{ this.chapterContent.fileContents }}</pre>
        </div> -->
        <div class="content-wrapper" :style="dynStyle"  v-html="this.chapterContent.fileContents">
        </div>
        <float-menu @changeStyle="(style) => dynamicStyle = style"></float-menu>
        <template #comments>
            <div class="cmt-box__header">Comments</div>
            <div class="cmt-box__main">
                <div class="chapter-cmt-group">
                    <div id="chapter-cmt-0001" class="chapter-cmt-item">
                        <div class="cmt__wrapper">
                            <div class="cmt-avatar">
                                <el-avatar icon="UserFilled" />
                            </div>
                            <div class="cmt-data">
                                <div class="cmt-username">Kryev1</div>
                                <div class="cmt-content">kjdfhakldhflajsdfksffhlshflkjasah</div>
                                <div class="cmt-footer">3 tháng</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="cmt-box__footer">
                <el-input type="textarea"></el-input>
            </div>
        </template>
    </reading-layout>
</template>

<style scoped>
.content-wrapper {
    border: 1px solid #eee;
    border-radius: 6px;
    text-align: justify;
}

.chapter-content {
    text-wrap: balance;
}

.chapter-nav {
    display: flex;
    align-items: center;
    /* justify-content: center; */
    width: fit-content;
    gap: 7px;
    margin: 20px auto 0;
}

.cmt-box__header {
    font-size: 24px;
    padding: 5px 15px 5px;
    border-bottom: 1px solid #ccc;
    background-color: #eee;
}

.cmt-box__main {
    padding: 15px;
    border-bottom: 1px solid #ccc;
}

.cmt-box__footer {
    padding: 15px;
}

.cmt__wrapper {
    display: flex;
    gap: 10px;
}

.cmt-data {
    display: flex;
    flex-direction: column;
    padding: 5px;
    width: 100%;
    gap: 5px;
    background-color: #eee;
    border-radius: 6px;
}

.cmt-data .cmt-username {
    /* font-weight: bold; */
    font-family: 'Roboto Bold';
    border-bottom: 1px solid #ccc;
}

.cmt-data .cmt-content {
    /* border-top: 1px solid #ddd; */
    text-align: justify;
}
</style>