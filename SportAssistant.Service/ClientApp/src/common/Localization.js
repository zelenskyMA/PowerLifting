
export const Locale = "ru-ru"; // window.navigator.language;

export function WeightCount(baseValue, percentageValue) {
  if (baseValue == 0 || baseValue == null || percentageValue == 0) {
    return 1;
  }

  var value = (baseValue / 100) * percentageValue;

  if (value <= 0) { return 1; }
  if (value >= 500) { return 500; }

  return Math.ceil(value);
}

export function DateToLocal(dateString) {
  var dateValue = new Date(dateString);
  return dateValue.toLocaleString(Locale, { dateStyle: "medium" });
}

export function DateToUtc(date) {
  var result = new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000);
  return result;
}