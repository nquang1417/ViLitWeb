const state = () => {
    var  novel = JSON.parse(sessionStorage.getItem('novel')) ?? {}
    var chapter = JSON.parse(sessionStorage.getItem('chapter')) ?? {}
    var sesstionGenre = JSON.parse(sessionStorage.getItem('genre')) ?? {}
    return {
        novelInfo: {
            bookId: novel.bookId ?? '',
            bookTitle: novel.bookTitle ?? '',
            genreId: novel.genreId ?? '',     
            genreName: novel.genreName ?? '',
            description: novel.description ?? '',
            bookCover: novel.bookCover ?? '',
            CoverUrl: novel.CoverUrl ?? '',
            authorName: novel.authorName ?? '',
            uploaderId: novel.uploaderId ?? '',
            chapters: novel.chapters ?? 0,
            languagueCode: novel.languagueCode ?? 1,
            languageName: novel.languageName ?? 'vi',
            url: novel.url ?? '',
            views: novel.views ?? 0,
            followers: novel.followers ?? 0,
            reviews: novel.reviews ?? 0,
            comments: novel.comments ?? 0,
            averageRating: novel.averageRating ?? 0,
            // Tags: [],
            status: novel.status ?? '1',
            createBy: novel.createBy ?? '',
            createDate: novel.createDate ?? '',
            updateBy: novel.updateBy ?? '',
            updateDate: novel.updateDate ??  ''
        },
        chapterInfo: {
            chapterId: chapter.chapterId ?? '',
            chapterTitle: chapter.chapterTitle ?? '',
            chapterNum: chapter.chapterNum ?? 0
        },
        genre: {
            genreId: sesstionGenre.genreId,
            genreName: sesstionGenre.genreName,
        }
    }
}

import { pad } from '../utils/utils.js'
const getters = {
    getNovelInfo(state) {
        return state.novelInfo
    },
    getNewChapterId(state) {
        var newChapter = state.Chapters + 1
        return `${state.BookId}-${pad(newChapter, 5)}`
    },
    getNewChapter(state) {
        return state.Chapters + 1
    },
    getChapterInfo(state) {
        return state.chapterInfo
    },
    getGenre(state) {
        return state.genre
    }
}

const mutations = {
    saveNovelInfo(state, data) {
        sessionStorage.setItem('novel', JSON.stringify(data));
        state.novelInfo = data
    },
    saveChapterInfo(state, data) {
        sessionStorage.setItem('chapter', JSON.stringify(data));
        state.chapterInfo = data
    },
    setGenre(state, data) {
        sessionStorage.setItem('genre', JSON.stringify(data));
        state.genre = data
    },
    clearInfo(state) {
        var newdata = {
            bookId: '',
            bookTitle: '',
            genreId: '',
            genreName: '',
            description: '',
            bookCover: '',
            coverUrl: '',
            authorName: '',
            uploaderId: '',
            chapters: 0,
            languagueCode: 1,
            languageName: 'vi',
            url: '',
            views: 0,
            followers: 0,
            reviews: 0,
            comments: 0,
            averageRating: 0,
            // Tags: [],
            status: '1',
            createBy: '',
            createDate: '',
            updateBy: '',
            updateDate: ''
        }
        var newChapter = {
            chapterId: '',
            chapterTitle: '',
            chapterNum: 0
        }
        
        state.novelInfo = newdata
        state.chapterInfo = newChapter
        sessionStorage.clear()
    }
}

const actions = {
    updateNovel({commit}, data) {
        commit("saveNovelInfo", data)
    },
    updateChapter({commit}, data) {
        commit("saveChapterInfo", data)
    },
    commitGenre({commit}, data) {
        commit("setGenre", data)
    },
    clearSession({commit}) {
        commit("clearInfo")
    }
}

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions
}