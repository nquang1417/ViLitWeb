const state = () => {
    var  novel = JSON.parse(sessionStorage.getItem('novel')) ?? {}
    var chapter = JSON.parse(sessionStorage.getItem('chapter')) ?? {}
    return {
        novelInfo: {
            BookId: novel.BookId ?? '',
            BookTitle: novel.BookTitle ?? '',
            GenreId: novel.GenreId ?? '',     
            GenreName: novel.GenreName ?? '',
            Description: novel.Description ?? '',
            BookCover: novel.BookCover ?? '',
            CoverUrl: novel.CoverUrl ?? '',
            AuthorName: novel.AuthorName ?? '',
            UploaderId: novel.UploaderId ?? '',
            Chapters: novel.Chapters ?? 0,
            LanguagueCode: novel.LanguagueCode ?? 1,
            LanguageName: novel.LanguageName ?? 'vi',
            Url: novel.Url ?? '',
            Views: novel.Views ?? 0,
            Followers: novel.Followers ?? 0,
            Reviews: novel.Reviews ?? 0,
            Comments: novel.Comments ?? 0,
            AverageRating: novel.AverageRating ?? 0,
            // Tags: [],
            Status: novel.Status ?? '1',
            CreateBy: novel.CreateBy ?? '',
            CreateDate: novel.CreateDate ?? '',
            UpdateBy: novel.UpdateBy ?? '',
            UpdateDate: novel.UpdateDate ??  ''
        },
        chapterInfo: {
            ChapterId: chapter.ChapterId ?? '',
            ChapterTitle: chapter.ChapterTitle ?? '',
            ChapterNum: chapter.ChapterNum ?? 0
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
    clearInfo(state) {
        var newdata = {
            BookId: '',
            BookTitle: '',
            GenreId: '',
            GenreName: '',
            Description: '',
            BookCover: '',
            CoverUrl: '',
            AuthorName: '',
            UploaderId: '',
            Chapters: 0,
            LanguagueCode: 1,
            LanguageName: 'vi',
            Url: '',
            Views: 0,
            Followers: 0,
            Reviews: 0,
            Comments: 0,
            AverageRating: 0,
            // Tags: [],
            Status: '1',
            CreateBy: '',
            CreateDate: '',
            UpdateBy: '',
            UpdateDate: ''
        }
        var newChapter = {
            ChapterId: '',
            ChapterTitle: '',
            ChapterNum: 0
        }
        state.novelInfo = newdata
        state.chapterInfo = newChapter
        sessionStorage.clear()
    }
}

const actions = {
    // async editInfo({commit}, payload) {
    //     var url = ``
    // }
    updateNovel({commit}, data) {
        commit("saveNovelInfo", data)
    },
    updateChapter({commit}, data) {
        commit("saveChapterInfo", data)
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