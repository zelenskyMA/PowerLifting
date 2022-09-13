import Cookies from 'js-cookie';

/* Ожидаем, что сервер возвращает json объект с 2 полями: token, refreshToken.
   Один ложится локально, второй в безопасные куки. */

const storageName = "token";

export function GetToken() { return localStorage.getItem(storageName); }

export function SetToken(tokenData) {
  Cookies.set(storageName, tokenData.refreshToken, { expires: 1 });
  localStorage.setItem(storageName, tokenData.token);
}

export async function RefreshToken() {
  localStorage.removeItem(storageName);

  var refreshToken = Cookies.get(storageName);
  if (refreshToken == null) {
    window.location.replace("/");
  }

  const response = await fetch("user/refreshToken",
    {
      method: "GET",
      headers: { "Accept": "application/json", "Content-Type": "application/json", "Authorization": `Bearer ${refreshToken}` }
    });

  if ([401, 403].includes(response.status)) {
    Cookies.remove(storageName);
    window.location.replace("/");
  }

  const data = await response.json();
  SetToken(data);
}
