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
import AdminLogin from '../components/pages/admin/AdminLogin.vue'
import NovelWorkspace from '../components/pages/NovelWorkspace.vue'
import NewChapter from '../components/pages/NewChapter.vue'

const routes = [
    { path: '/', name: 'HomePage', component: HomePage },
    { path: '/home', redirect: '/' },
    { path: '/login', name: 'Login', component: LoginPage },
    { path: '/signup', name: 'Signup', component: SignupPage },
    { path: '/admin', name: 'AdminPage', component: AdminPage, meta: { requiredAuth: true } },
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
        fullpath: '/dashboard/edit-chapter/:chapterId/:chapterNo',
        path: '/dashboard/edit-chapter/Chuong-:chapterNo',
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
    {path: '/test '}

]

const router = createRouter({
    history: createWebHistory(),
    routes,
    scrollBehavior(to, from, savedPosition) {
        return { top: 0 };
    }
})

router.beforeEach((to, from, next) => {
    if (((from.name === 'ReadingPage' || from.name === 'NovelDetails')) && to.name !== 'ReadingPage') {
        store.dispatch('novel/clearSession')
    }
    if (from.name === 'NovelWorkspace' && (to.name !== 'NewChapter' && to.name !== 'EditChapter')) {
        store.dispatch('novel/clearSession')
    }
    if (to.path.includes('/admin')) {

        if (to.path === '/admin/login' && store.getters["auth/getLoginStatus"]) {
            var user = store.getters["auth/getAuthData"]
            if (user.role == 'Admin') {
                next({
                    path: '/admin'
                })
            } else {
                next()
            }
        } else if (to.matched.some(record => record.meta.requiredAuth)) {
            if (!store.getters["auth/getLoginStatus"]) {
                console.log(store.getters["auth/getLoginStatus"])
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
                console.log(store.getters["auth/getLoginStatus"])
                next({
                    path: '/login'
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