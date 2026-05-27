import { createI18n } from 'vue-i18n';

import { getStoredLocale } from '@/lib/localeStorage';
import en from '@/locales/en';
import vi from '@/locales/vi';

const i18n = createI18n({
  legacy: false,
  locale: getStoredLocale(),
  fallbackLocale: 'en',
  messages: {
    en,
    vi,
  },
  missingWarn: import.meta.env.DEV,
  fallbackWarn: import.meta.env.DEV,
  missing: (locale, key) => {
    if (import.meta.env.DEV) {
      console.warn(`[i18n] missing key "${key}" in locale "${locale}"`);
    }
  },
});

export default i18n;
