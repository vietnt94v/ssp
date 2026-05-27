import { defineStore } from 'pinia';
import { ref } from 'vue';

import i18n from '@/i18n';
import {
  getStoredLocale,
  setStoredLocale,
  type AppLocale,
} from '@/lib/localeStorage';

export const useLocaleStore = defineStore('locale', () => {
  const locale = ref<AppLocale>(getStoredLocale());

  function setLocale(next: AppLocale) {
    locale.value = next;
    i18n.global.locale.value = next;
    setStoredLocale(next);
  }

  return { locale, setLocale };
});
