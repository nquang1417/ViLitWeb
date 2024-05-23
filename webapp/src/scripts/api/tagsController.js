const baseUrl = 'http://localhost:10454/api/Tags'
var url = ``

export default axios => ({
    getTag(id) {
        url = `${baseUrl}/get?id=${id}`
        return axios.get(url)
    },
    add(payload) {
        url = `${baseUrl}/add`
        return axios.post(url, payload)
    },
    multiAdd(payload) {
        url = `${baseUrl}/multi-add`
        return axios.post(payload)
    }
})