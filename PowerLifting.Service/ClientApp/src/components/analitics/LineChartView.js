import React from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { Container } from "reactstrap";

export function LineChartView({ displayList, data, multidata = false }) {
  if (data.length == 0 && !multidata) {
    return (<></>);
  }

  return (
    <Container fluid>
      <LineChart width={1100} height={600} data={data} syncId="anyId" margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>

        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="name" />
        <YAxis />
        <Tooltip />
        <Legend layout="vertical" align="right" verticalAlign="middle" />
        {
          displayList.map(item =>
            multidata ?
              <Line type="monotone" data={item.data} name={item.name} dataKey={item.id} stroke={item.color} />
              : <Line type="monotone" name={item.name} dataKey={item.id} stroke={item.color} />
          )
        }
      </LineChart>
    </ Container>
  );
}
