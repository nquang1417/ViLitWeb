import usersController from './vilUsersController'
import novelController from './novelController'
import genresController from './genresController'
import chaptersController from './chaptersController'
import bookmarksController from './bookmarksController'
import commentsController from './commentsController'
import reviewsController from './reviewsController'
import reportsController from './reportsController'
import notiController from './notiController'
import tagsController from './tagsController'
import reactionController from './reactionController'

export default axios => ({
    getApiFactories() {
        const factories = {
            users: usersController(axios),
            novels: novelController(axios),
            genres: genresController(axios),
            chapters: chaptersController(axios),
            bookmarks: bookmarksController(axios),
            comments: commentsController(axios),
            reviews: reviewsController(axios),
            reports: reportsController(axios),
            notifications: notiController(axios),
            tags: tagsController(axios),
            reactions: reactionController(axios),
        }
        return factories
    }
})