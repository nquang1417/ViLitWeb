const baseUrl = 'http://localhost:10454/api/Genres'
var url = ``

export default axios => ({
    list() {
        url = `${baseUrl}/lists`
        return axios.get(url)
    },
    getDetails(id) {
        url = `${baseUrl}/details?genreId=${id}`
        return axios.get(url)
    }
})