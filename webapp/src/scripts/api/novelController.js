const baseUrl = 'http://localhost:10454/api/BookInfo'
var url = ``

export default axios => ({
    getAll() {
        url = `${baseUrl}/all`
        return axios.get(url)
    },
    getDetails(id) {
        url = `${baseUrl}/details?bookId=${id}`
        return axios.get(url)
    },
    getNewUpdates() {
        url = `${baseUrl}/new-updates`
        return axios.get(url)
    },
    search(title) {
        url = `${baseUrl}/search?title=${title}`
        return axios.get(url)
    },
    filter(payload, pageNo) {
        url = `${baseUrl}/filter?page=${pageNo}`
        return axios.post(url, payload)
    },
    addNovel(payload, token) {
        url = `${baseUrl}/add`
        return axios.post(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    updateNovel(id, payload, token, owner) {
        url = `${baseUrl}/update?bookId=${id}`
        return axios.put(url, payload, {
            headers: {
                'Content-Type': 'application/json-patch+json',
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    },
    deleteNovel(id, token, owner) {
        url = `${baseUrl}/delete?bookId=${id}`
        return axios.delete(url, {
            headers: {
                'access_token': `${token}`,
                'ownerId': `${owner}`
            }
        })
    }
})