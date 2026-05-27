import { beforeEach, describe, it, expect } from 'vitest'
import { createPinia } from 'pinia'
import { createRouter, createMemoryHistory } from 'vue-router'
import { flushPromises, mount } from '@vue/test-utils'
import App from '../App.vue'
import i18n from '../i18n'
import { LOCALE_STORAGE_KEY } from '@/lib/localeStorage'
import { routes } from '../router'

describe('App', () => {
  beforeEach(() => {
    localStorage.removeItem(LOCALE_STORAGE_KEY)
    i18n.global.locale.value = 'en'
  })

  it('mounts renders properly', async () => {
    const router = createRouter({
      history: createMemoryHistory(import.meta.env.BASE_URL),
      routes,
    })
    router.push('/')
    await router.isReady()

    const wrapper = mount(App, {
      global: {
        plugins: [createPinia(), i18n, router],
      },
    })
    await flushPromises()

    expect(wrapper.text()).toContain('Todos')
    expect(wrapper.text()).toContain('Home')
    expect(wrapper.text()).toContain('About')
  })
})
