const baseUrl = 'http://localhost:10454/api/BookChapters'
var url = ``

export default axios => ({
    getSummaries(bookId) {
        url = `${baseUrl}/all-summary?bookId=${bookId}`
        return axios.get(url)
    },
    getChapters(bookId, pageNo) {
        url = `${baseUrl}/get-chapters?bookId=${bookId}&page=${pageNo}`
        return axios.get(url)
    },
    getDrafts(bookId, pageNo, token, owner) {
        url = `${baseUrl}/get-drafts?bookId=${bookId}&page=${pageNo}`
        return axios.get(url, {
            headers: {
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    },
    getDeleteds(bookId, pageNo, token, owner) {
        url = `${baseUrl}/get-deleteds?bookId=${bookId}&page=${pageNo}`
        return axios.get(url, {
            headers: {
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    },
    getChapterById(chapterId) {
        url = `${baseUrl}/get-chapter?chapterId=${chapterId}`
        return axios.get(url)
    },
    getChapterAudio(chapterId) {
        url = `${baseUrl}/get-audio?chapterId=${chapterId}`
        return axios.get(url, { responseType: "blob",})
    },
    getNewChapter(bookId, token) {
        url = `${baseUrl}/new-chapter?bookId=${bookId}`
        return axios.get(url, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    addChapter(payload, token, owner) {
        url = `${baseUrl}/add`
        return axios.post(url, payload, {
            headers: {
                'Content-Type': 'application/json-patch+json',
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    },
    uploadChapter(chapter, content, token, owner) {
        url = `${baseUrl}/upload-chapter?chapterId=${chapter.chapterId}`
        var txtEncode = new TextEncoder('utf-8').encode(content)
        var file = new Blob([txtEncode], { type: 'text/plain' })
        var formData = new FormData()
        formData.append('file', file, chapter.fileName)
        return axios.post(url, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    },
    uploadFromFile(chapter, file, token, owner) {
        url = `${baseUrl}/upload-chapter?chapterId=${chapter.chapterId}`
        var formData = new FormData()
        formData.append('file', file, chapter.fileName)
        return axios.post(url, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    },
    changeStatus(chapterId, status, token, owner) {
        url = `${baseUrl}/change-status?chapterId=${chapterId}&status=${status}`
        return axios.put(url, null, {
            headers: {
                'Content-Type': 'application/json-patch+json',
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    },
    emptyTrash(bookId, token, owner) {
        url = `${baseUrl}/empty-trash?bookId=${bookId}`
        return axios.delete(url, {
            headers: {
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    }
})