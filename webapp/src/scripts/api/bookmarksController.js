const baseUrl = 'http://localhost:10454/api/Bookmarks'
var url = ``

export default axios => ({
    getAll(userId, token) {
        url = `${baseUrl}/all?userId=${userId}`
        return axios.get(url, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    addBookmarks(payload, token) {
        url = `${baseUrl}/new-bookmark`
        return axios.post(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    filter(payload, pageNo, token) {
        url = `${baseUrl}/filter?page=${pageNo}`
        return axios.post(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    delete(id, token) {
        url = `${baseUrl}/delete?bookmarkId=${id}`
        return axios.delete(url, {
            headers: {
                'access_token': `${token}`                
            }
        })
    },
    getBookmarkStatus(userId, chapterId, token) {
        url = `${baseUrl}/is-bookmarked?userId=${userId}&chapterId=${chapterId}`
        return axios.get(url, {
            headers: {
                'access_token': `${token}`                
            }
        })
    }
})