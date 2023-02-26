import { GetToken, RefreshToken } from './TokenActions';

export async function GetAsync(url) { return await ActionAsync(url, "GET", null); }
export async function PostAsync(url, payload = null) { return await ActionAsync(url, "POST", payload); }
export async function PutAsync(url, payload = null) { return await ActionAsync(url, "PUT", payload); }
export async function DeleteAsync(url, payload = null) { return await ActionAsync(url, "DELETE", payload); }

async function ActionAsync(url, method, payload = null) {
  const requestOptions = {
    method: method,
    headers: GetHeaders(),
    body: payload === null ? null : JSON.stringify(payload),
  };

  var funcCall = async () => { return await fetch(url, requestOptions); };
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