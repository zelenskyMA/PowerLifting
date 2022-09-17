import React from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { Container } from "reactstrap";

export function LineChartPanel({ data, displayList }) {
  if (data.length == 0) {
    return (<></>);
  }

  return (
    <Container fluid>
      <LineChart width={800} height={600} data={data} margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>

        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="name" />
        <YAxis />
        <Tooltip />
        <Legend />

        {displayList.map(item =>
          <Line type="monotone" name={item.name} dataKey={item.id} stroke={item.color} />
        )}
      </LineChart>
    </ Container>
  );
}
