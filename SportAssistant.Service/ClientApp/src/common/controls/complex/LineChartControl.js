/// <reference path="../inputs/inputcheckbox.js" />
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { Container } from "reactstrap";

export function LineChartControl({ displayList, data = null, multidata = false }) {
  const { t } = useTranslation();
  const [lineProps, setLineProps] = useState(
    displayList.reduce(
      (item, key) => {
        item[key] = true;
        return item;
      })
  );

  if (data?.length == 0) { return (<></>); }
  if (data?.length > 25) { return (<>{t('analitics.chartOverload')}</>); }

  var colors = [
    '#000000', '#FF0000', '#0000FF', '#728C00', '#C19A6B', '#FF00FF', '#800000', '#008000', '#FFDB58', '#FFD700',
    '#00BFFF', '#00FFFF', '#31906E', '#3C565B', '#C04000', '#827839', '#B8860B', '#806517', '#212F3C', '#641E16',
    '#145A32', '#1B4F72', '#7D6608', '#17A589', '#808000'];

  const ToggleLine = (e) => { setLineProps({ ...lineProps, [e.value]: !lineProps[e.value] }); };

  return (
    <Container fluid>
      <LineChart width={1100} height={600} data={data} margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>
        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="name" />
        <YAxis />
        <Tooltip />
        <Legend layout="vertical" align="right" verticalAlign="middle" onClick={ToggleLine} />
        {
          displayList.map((item, i) =>
            multidata ?
              <Line
                type="monotone"
                key={'lineChart_' + i}
                name={item.name}
                dataKey={(data) => getValue(data, item.data)}
                stroke={colors[i]}
                hide={lineProps[item.name] === true}
                dot={false} /> :
              <Line
                type="monotone"
                key={'lineChart_' + i}
                name={item.name}
                dataKey={item.id}
                stroke={colors[i]} />
          )
        }
      </LineChart>
    </ Container>
  );
}

// Пробегая по основным данным, для каждой линии подбираем соответствующее значение из ее собственного массивы данных.
// data - массив данных в LineChart, основные данные. Там должны быть все возможные зачения ключа, а данные - заглушки.
// myData - массив данных, записанный в конкретную линию графика. В этом массиве реальные данные для графика.
function getValue(data, myData) {
  const index = myData.findIndex(obj => obj.name === data.name);
  return index === -1 ? 0 : myData[index].value;
};