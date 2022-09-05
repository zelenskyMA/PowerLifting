

export async function PostAsync(url, payload = null) {
  const requestOptions = {
    method: "POST",
    headers: { "Accept": "application/json", "Content-Type": "application/json", },
    body: payload === null ? null : JSON.stringify(payload),
  };

  const response = await fetch(url, requestOptions);
  const data = await response.json();
  return data;
}

export async function GetAsync(url) {
  const response = await fetch(url);
  const data = await response.json();
  return data;
}

export function Get(id, entityName, dispatch) {
  fetch(`${entityName}/get?id=${id}`)
    .then(response => response.json())
    .then(data => {
      dispatch({ type: `RECEIVE_${entityName.toUpperCase()}S`, result: data });
    });

  dispatch({ type: `REQUEST_${entityName.toUpperCase()}S` });
}