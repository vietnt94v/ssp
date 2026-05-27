export const LOCALE_STORAGE_KEY = 'app-locale';

export type AppLocale = 'en' | 'vi';

export function getStoredLocale(): AppLocale {
  if (typeof localStorage === 'undefined') {
    return 'en';
  }
  const value = localStorage.getItem(LOCALE_STORAGE_KEY);
  if (value === 'vi' || value === 'en') {
    return value;
  }
  return 'en';
}

export function setStoredLocale(locale: AppLocale): void {
  if (typeof localStorage === 'undefined') {
    return;
  }
  localStorage.setItem(LOCALE_STORAGE_KEY, locale);
}
