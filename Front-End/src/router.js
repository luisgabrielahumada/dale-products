import { createRouter, createWebHashHistory } from 'vue-router';
import App from './App.vue';

const routes = [
    {
        path: '/',
        name: 'Login',
        component: () => import('./pages/Login.vue')
    },
    {
        path: '/',
        name: 'app',
        component: App,
        children: [
            {
                path: '/home',
                name: 'home',
                component: () => import('./pages/Home.vue')
            },
        ]
    },
];

const router = createRouter({
    history: createWebHashHistory(),
    routes,
});

export default router;
