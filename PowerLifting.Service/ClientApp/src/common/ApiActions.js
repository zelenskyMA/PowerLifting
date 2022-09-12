

export async function PostAsync(url, payload = null) {
  const requestOptions = {
    method: "POST",
    headers: { "Accept": "application/json", "Content-Type": "application/json", },
    body: payload === null ? null : JSON.stringify(payload),
  };

  const response = await fetch(url, requestOptions);
  if (response.ok) {
    const data = await response.json();
    return data;
  }

  const err = await response.json();
  throw err;
}


export async function GetAsync(url) {
  const response = await fetch(url);

  if (response.ok) {
    const data = await response.json();
    return data;
  }

  const err = await response.json();
  throw err;
}
