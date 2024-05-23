const baseUrl = 'http://localhost:10454/api/Reactions'
var url = ``

export default axios => ({
    getFollowingStatus(userId, bookId, token) {
        url = `${baseUrl}/followsStatus?userId=${userId}&bookId=${bookId}`
        return axios.get(url, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    followBook(userId, bookId, token) {
        url = `${baseUrl}/follow-book?userId=${userId}&bookId=${bookId}`
        return axios.post(url, null, {
            headers: {
                'access_token': `${token}`
            }
        })
    }
})