const baseUrl = 'http://localhost:10454/api/Reports'
var url = ``

export default axios => ({
    getAll(token) {
        url = `${baseUrl}/all`
        return axios.get(url, {
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
    add(payload, token) {
        url = `${baseUrl}/report`
        return axios.post(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    update(payload, token) {
        url = `${baseUrl}/update`
        return axios.put(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    delete(id, token) {
        url = `${baseUrl}/delete?id=${id}`
        return axios.delete(url, {
            headers: {
                'access_token': `${token}`
            }
        })
    }
})