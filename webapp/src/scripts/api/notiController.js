const baseUrl = 'http://localhost:10454/api/Notifications'
var url = ``

export default axios => ({
    getNoti(userId, token) {
        url = `${baseUrl}/getNoti?userId=${userId}`
        return axios.get(url, {
            headers: {
                'access_token': `${token}`,
                'ownerId': `${userId}`
            }
        })
    },
    markReadAll(userId, token) {
        url = `${baseUrl}/readAll?userId=${userId}`
        return axios.put(url, null, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    markAsRead(notiId, token) {
        url = `${baseUrl}/markRead?notiId=${notiId}`
        return axios.put(url, null, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    delete(notiId, token) {
        url = `${baseUrl}/delete?notiId=${notiId}`
        return axios.delete(url, {
            headers: {
                'access_token': `${token}`
            }
        })
    }
})