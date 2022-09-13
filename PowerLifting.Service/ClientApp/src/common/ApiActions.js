import { GetToken, RefreshToken } from './AuthActions';

export async function PostAsync(url, payload = null) {
  const requestOptions = {
    method: "POST",
    headers: GetHeaders(),
    body: payload === null ? null : JSON.stringify(payload),
  };

  var funcCall = async () => { return await fetch(url, requestOptions); };
  return await ExecuteRequest(funcCall);
}

export async function GetAsync(url) {
  var funcCall = async () => { return await fetch(url, { method: "GET", headers: GetHeaders() }) };
  return await ExecuteRequest(funcCall);
}


async function ExecuteRequest(funcCall) {
  var response = await funcCall();

  if ([401, 403].includes(response.status)) {
    if (await RefreshToken()) { // repeate on successfull token refresh
      response = await funcCall();
    }
  }

  const data = await response.json();
  if (response.ok) {
    return data;
  }

  throw data;
}

function GetHeaders() {
  const token = GetToken();
  if (token) {
    return { "Accept": "application/json", "Content-Type": "application/json", "Authorization": `Bearer ${token}` };
  }

  return { "Accept": "application/json", "Content-Type": "application/json" };
}