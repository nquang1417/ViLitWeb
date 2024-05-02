const baseUrl = 'http://localhost:10454/api/Users'
var url = ``
export default axios => ({
    getUsers(userId) {
        url = `${baseUrl}/${userId}`
        return axios.get(url)        
    },
    getAllUsers(token) {
        url = `${baseUrl}/all`
        return axios.get(url, {
            headers: {
                "access_token": `${token}`
            }
        })
    },
    register(payload) {
        url = `${baseUrl}/add`
        return axios.post(url, payload, {
            headers: {
                'Content-Type': 'application/json-patch+json'
            }
        })
    },
    updateUser(payload, token) {
        url = `${baseUrl}/update`
        return axios.put(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    // searchUser(payload) {
    //     if (payload.username) {
    //         url = `${baseUrl}/search?username=${payload.username}`
    //     }
    //     return axios.get(url)
    // },
    changePass(payload, token) {
        url = `${baseUrl}/change-password`
        return axios.put(url, payload, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    changeStatus(userId, status, token) {
        url = `${baseUrl}/change-status?userid=${userId}&status=${status}`
        return axios.put(url, null, {
            headers: {
                'access_token': `${token}`
            }
        })
    },
    deleteUser(userId, token) {
        url = `${baseUrl}/delete?id=${userId}`
        return axios.delete(url, {
            headers: {
                'access_token': `${token}`
            }
        })
    }
})