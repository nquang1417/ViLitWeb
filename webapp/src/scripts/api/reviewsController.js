const baseUrl = 'http://localhost:10454/api/Reviews'
var url = ``

export default axios => ({
    filter(bookId, pageNo) {
        url = `${baseUrl}/filter?bookId=${bookId}&page=${pageNo}`
        return axios.get(url)
    },
    add(payload, token) {
        url = `${baseUrl}/add`
        return axios.post(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    edit(payload, token) {
        url = `${baseUrl}/edit`
        return axios.put(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    delete(reviewId, token) {
        url = `${baseUrl}/delete?reviewId=${reviewId}`
        return axios.delete(url, {
            headers: {
                'access_token': `${token}`
            }
        })
    }
})