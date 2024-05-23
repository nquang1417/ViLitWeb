const baseUrl = 'http://localhost:10454/api/Comments'
var url = ``

export default axios => ({
    filter(chapterId, pageNo) {
        url = `${baseUrl}/filterComments?chapterId=${chapterId}&page=${pageNo}`
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
    delete(cmtId, token) {
        url = `${baseUrl}/delete?commentId=${cmtId}`
        return axios.delete(url, {
            headers: {
                'access_token': `${token}`
            }
        })
    }
})