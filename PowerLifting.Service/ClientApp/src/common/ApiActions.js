
export async function PostAsync(url, payload = null) {
  const requestOptions = {
    method: "POST",
    headers: GetHeaders(),
    body: payload === null ? null : JSON.stringify(payload),
  };

  const response = await fetch(url, requestOptions);
  return ResponseHandler(response);
}

export async function GetAsync(url) {
  const response = await fetch(url, { method: "GET", headers: GetHeaders() });
  return ResponseHandler(response);
}



async function ResponseHandler(response) {
  const data = await response.json();

  if ([401, 403].includes(response.status)) {
    localStorage.removeItem('token');
    window.location.replace("/");
  }

  if (response.ok) { return data; }
  throw data;
}

function GetHeaders() {
  const token = localStorage.getItem("token");
  if (token) {
    return { "Accept": "application/json", "Content-Type": "application/json", "Authorization": `Bearer ${token}` };
  }

  return { "Accept": "application/json", "Content-Type": "application/json" };
}