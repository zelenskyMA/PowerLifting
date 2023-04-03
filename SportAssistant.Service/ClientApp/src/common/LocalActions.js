
export const Locale = "ru-ru"; // window.navigator.language;

export function DateToLocal(dateString) {
  var dateValue = new Date(dateString);
  return dateValue.toLocaleString(Locale, { dateStyle: "medium" });
}

export function DateToUtc(date) {
  var result = new Date(date.getTime() - date.getTimezoneOffset() * 60 * 1000);
  return result;
}