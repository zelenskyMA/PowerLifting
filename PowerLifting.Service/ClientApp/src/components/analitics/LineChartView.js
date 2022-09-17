import React from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { Container } from "reactstrap";

export function LineChartView({ displayList, data = null, multidata = false }) {
  if (data?.length == 0) {
    return (<></>);
  }

  return (
    <Container fluid>
      <LineChart width={1100} height={600} data={data} margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>

        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="name" />
        <YAxis />
        <Tooltip />
        <Legend layout="vertical" align="right" verticalAlign="middle" />
        {
          displayList.map(item =>
            multidata ?
              <Line type="monotone" name={item.name} dataKey={(data) => getValue(data, item.data)} stroke={item.color} dot={false} /> :
              <Line type="monotone" name={item.name} dataKey={item.id} stroke={item.color} />
          )
        }
      </LineChart>
    </ Container>
  );
}

// Пробегая по основным данным, для каждой линии подбираем соответствующее значение из ее собственного массивы данных.
// data - массив данных в LineChart, основные данные. Там должны быть все возможные зачения ключа, а данные - заглушки.
// myData - массив данных, записанный в конкретную линию графика. В этом массиве реальные данные для графика.
function getValue (data, myData) {
  const index = myData.findIndex(obj => obj.name === data.name);
  return index === -1 ? 0 : myData[index].value;
};