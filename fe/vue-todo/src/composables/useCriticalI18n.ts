import { useI18n } from 'vue-i18n';

export function useCriticalI18n() {
  const { t } = useI18n();

  function tc(
    key: string,
    defaultValue: string,
    values?: Record<string, unknown>,
  ) {
    if (!values || Object.keys(values).length === 0) {
      return t(key, defaultValue);
    }
    return t(key, values, { default: defaultValue });
  }

  return { tc };
}
