import { beforeEach, describe, expect, it } from 'vitest';
import { createApp, defineComponent, h } from 'vue';
import { createI18n } from 'vue-i18n';

import { useCriticalI18n } from '@/composables/useCriticalI18n';

function mountWithI18n(
  messages: Record<string, Record<string, unknown>>,
  render: () => string,
) {
  const i18n = createI18n({
    legacy: false,
    locale: 'vi',
    fallbackLocale: 'en',
    messages: messages as never,
    missingWarn: false,
    fallbackWarn: false,
    missing: () => undefined,
  });

  let output = '';
  const App = defineComponent({
    setup() {
      output = render();
      return () => h('div');
    },
  });

  const app = createApp(App);
  app.use(i18n);
  app.mount(document.createElement('div'));
  return output;
}

describe('useCriticalI18n', () => {
  beforeEach(() => {
    document.body.innerHTML = '';
  });

  it('returns defaultValue when key is missing in all locales', () => {
    const result = mountWithI18n({ en: {}, vi: {} }, () => {
      const { tc } = useCriticalI18n();
      return tc('common.login', 'Login');
    });

    expect(result).toBe('Login');
  });

  it('returns defaultValue with named values when key is missing', () => {
    const result = mountWithI18n({ en: {}, vi: {} }, () => {
      const { tc } = useCriticalI18n();
      return tc('common.greet', 'Hello {name}', { name: 'An' });
    });

    expect(result).toBe('Hello An');
  });

  it('uses translation when fallback locale has the key', () => {
    const result = mountWithI18n(
      { en: { common: { login: 'Login EN' } }, vi: {} },
      () => {
        const { tc } = useCriticalI18n();
        return tc('common.login', 'Login');
      },
    );

    expect(result).toBe('Login EN');
  });
});
