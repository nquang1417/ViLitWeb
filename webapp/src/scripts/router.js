import { createRouter, createWebHistory } from 'vue-router'
import store from './store'

import HomePage from '../components/pages/HomePage.vue'
import NewNovel from '../components/pages/NewNovel.vue'
import DashboardPage from '../components/pages/DashboardPage.vue'
import DashboardHome from '../components/pages/DashboardHome.vue'
import NovelManagement from '../components/pages/NovelManagement.vue'
import ReadingPage from '../components/pages/ReadingPage.vue'
import NovelDetails from '../components/pages/NovelDetails.vue'
import LoginPage from '../components/pages/LoginPage.vue'
import SignupPage from '../components/pages/SignupPage.vue'
import AdminPage from '../components/pages/admin/AdminPage.vue'
import AdminHome from '../components/pages/admin/AdminHome.vue'
import AdminLogin from '../components/pages/admin/AdminLogin.vue'
import UserManagement from '../components/pages/admin/UserManagement.vue'
import NovelWorkspace from '../components/pages/NovelWorkspace.vue'
import NewChapter from '../components/pages/NewChapter.vue'
import ProfilePage from '../components/pages/ProfilePage.vue'
import BookshelfPage from '../components/pages/BookshelfPage.vue'
import BookmarksPage from '../components/pages/BookmarksPage.vue'
import AdminNovel from '../components/pages/admin/AdminNovel.vue'
import GenrePage from '../components/pages/GenrePage.vue'
import ReviewPage from '../components/pages/ReviewPage.vue'
import UpdateUser from '../components/pages/UpdateUser.vue'
import ReportManagement from '../components/pages/admin/ReportManagement.vue'

const routes = [
    { path: '/', name: 'HomePage', component: HomePage },
    { path: '/home', redirect: '/' },
    { path: '/login', name: 'Login', component: LoginPage },
    { path: '/signup', name: 'Signup', component: SignupPage },
    { 
        path: '/admin', name: 'AdminPage', component: AdminPage, meta: { requiredAuth: true },
        children: [
            {
                path: '', name: 'AdminHome', component: AdminHome, meta: { requiredAuth: true },
            }
        ]
    },    
    { path: '/admin/user-management', name: 'UserManagement', component: UserManagement, meta: {requiredAuth: true}},
    { path: '/admin/report-management', name: 'ReportManagement', component: ReportManagement, meta: {requiredAuth: true}},
    { path: '/admin/novel-management', name: 'AdminNovel', component: AdminNovel, meta: {requiredAuth: true}},
    { path: '/admin/login', name: 'AdminLogin', component: AdminLogin },
    {
        path: '/dashboard', name: 'DashboardPage', component: DashboardPage, meta: { requiredAuth: true },
        children: [
            {
                path: '',
                name:'DashboardHome',
                component: DashboardHome,
                meta: { requiredAuth: true }
            },
        ]
    },
    {
        path: '/dashboard/new-novel',
        name: 'NewNovel',
        component: NewNovel,
        meta: { requiredAuth: true }
    },
    {
        path: '/dashboard/:novelTitle/new-chapter',
        name: 'NewChapter',
        component: NewChapter,
        meta: { requiredAuth: true }
    },
    {
        path: '/dashboard/edit-chapter/:chapterId/',
        name: 'EditChapter',
        component: NewChapter,
        meta: { requiredAuth: true },
        props: true
    },
    {
        path: '/dashboard/workspace/:novelId',
        name: 'NovelWorkspace',
        component: NovelWorkspace,
        meta: { requiredAuth: true },
        props: true
    },
    { 
        path: '/dashboard/edit-novel/:novelId',
        name: 'EditNovel',
        component: NewNovel,
        meta: { requiredAuth: true },
        props: true,
    },
    {
        path: '/dashboard/novel-management',
        name: 'NovelManagement',
        component: NovelManagement,
        meta: { requiredAuth: true }
    },
    { path: '/novel/:novelId', name: 'NovelDetails', component: NovelDetails, },
    {
        path: '/:title/:chapterTitle/',
        name: 'ReadingPage',
        component: ReadingPage,
        props: true,
    },
    {
        path: '/profile/:userId',
        name: 'ProfilePage',
        component: ProfilePage,
        props: true,
        // meta: { requiredAuth: true}   
    },
    {
        path: '/bookshelf',
        name: 'BookshelfPage',
        component: BookshelfPage,
        meta: { requiredAuth: true}           
    },
    {
        path: '/bookmarks',
        name: 'BookmarksPage',
        component: BookmarksPage,
        meta: { requiredAuth: true}           
    },
    {
        path: '/genre',
        name: 'GenrePage',
        component: GenrePage,        
    },
    {
        path: '/novel/:novelName/review',
        name: 'ReviewPage',
        component: ReviewPage
    },
    {
        path: '/profile/edit',
        name: 'UpdateUser',
        component: UpdateUser,
        meta: { requiredAuth: true }
    },
    {
        path: '/profile/change-password',
        name: 'ChangePass',
        component: UpdateUser,
        meta: { requiredAuth: true}
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes,
    scrollBehavior(to, from, savedPosition) {
        return { top: 0 };
    }
})

router.beforeEach((to, from, next) => {
    // if (((from.name === 'ReadingPage' || from.name === 'NovelDetails')) && (to.name !== 'ReadingPage' && to.name !== 'ReviewPage')) {
    //     store.dispatch('novel/clearSession')
    // }
    // if (from.name === 'NovelWorkspace' && (to.name !== 'NewChapter' && to.name !== 'EditChapter')) {
    //     store.dispatch('novel/clearSession')
    // }
    if (to.path.includes('/admin')) {
        var user = store.getters["auth/getAuthData"]
        if (to.path === '/admin/login' && store.getters["auth/getLoginStatus"]) {            
            if (user.role == 'Admin') {
                next({
                    path: '/admin'
                })
            } else {
                store.dispatch('auth/logout')
                next({
                    path: '/admin/login'
                })
            }
        } else if (to.matched.some(record => record.meta.requiredAuth)) {
            if (!store.getters["auth/getLoginStatus"] || user.role != "Admin") {
                next({
                    path: '/admin/login'
                })
            } else {
                next()
            }
        } else {
            next()
        }
    } else {
        if (to.matched.some(record => record.meta.requiredAuth)) {
            if (!store.getters["auth/getLoginStatus"]) {
                next({
                    path: '/'
                })
            } else {
                next()            
            }
        } else {
            next()
        }
    }
})

export default router