/*
 entityName - кусок константы из reducer'a которую мы не знаем.
 
 Префикс константы считаем извесным
 REQUEST_
 RECEIVE_
 и т.д.
 */

export function Create(entityName, payload) {
  return fetch(
    `${entityName}/create`, //url
    {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    })
}

export function Update(entityName, payload) {
  return fetch(
    `${entityName}/update`, //url
    {
      method: 'POST',
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    })
}

export async function GetAsync(url) {
  const response = await fetch(url);
  debugger;
  var data = await response.json();
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