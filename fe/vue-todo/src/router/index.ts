import { createRouter, createWebHistory } from 'vue-router'

export const routes = [
  {
    path: '/',
    name: 'home',
    component: () => import('@/views/HomeView.vue'),
  },
  {
    path: '/about',
    name: 'about',
    component: () => import('@/views/AboutView.vue'),
    meta: {
      requiresAuth: true,
    },
  },
  {
    path: '/demo',
    name: 'demo',
    component: () => import('@/views/DemoView.vue'),
  },
  {
    path: '/setting',
    component: () => import('@/views/setting/SettingLayout.vue'),
    children: [
      {
        path: '',
        name: 'setting',
        component: () => import('@/views/setting/SettingIndexView.vue'),
      },
      {
        path: 'profile',
        name: 'setting-profile',
        component: () => import('@/views/setting/SettingProfileView.vue'),
      },
      {
        path: 'profile/:anotheraccount',
        name: 'setting-profile-account',
        component: () => import('@/views/setting/SettingProfileAccountView.vue'),
        props: true,
      },
    ],
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

router.beforeEach((to, from) => {
  if (to.meta.requiresAuth) {
    return { name: 'home' }
  }
})

export default router
