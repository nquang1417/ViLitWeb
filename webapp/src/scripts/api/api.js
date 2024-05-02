import usersController from './vilUsersController'
import novelController from './novelController'
import chaptersController from './chaptersController'

export default axios => ({
    getApiFactories() {
        const factories = {
            users: usersController(axios),
            novels: novelController(axios),
            chapters: chaptersController(axios)
        }
        return factories
    }
})