/*
 entityName - кусок константы из reducer'a которую мы не знаем.
 
 Префикс константы считаем извесным
 REQUEST_
 RECEIVE_
 и т.д.
 */

export async function PostAsync(url, payload) {
  const response = await fetch(
    url,
    {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    });

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