/// <reference path="../inputs/inputcheckbox.js" />
import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend } from 'recharts';
import { Container } from "reactstrap";

// linesDataList - данные для линии/линий графика. Ось Y (вертикаль).
// chartXDots - все точки графика, в которых могут быть данные. Ось X (горизонталь).
// multidata - флаг для ризличной отрисовки линий. Когда она одна, все проще.
export function LineChartControl({ linesDataList, chartXDots = null, multidata = true }) {
  const { t } = useTranslation();

  // формируем доп. свойство в объектах массива под тоггл. True по дефолту, чтоб новые данные не прятались.
  const [lineProps, setLineProps] = useState(
    linesDataList.length == 0 ? [] :
      linesDataList.reduce(
        (item, key) => {
          item.name = item.name ? item.name : t('analitics.commonLineName');
          item[key] = true;
          return item;
        })
  );

  if (chartXDots?.length == 0) { return (<></>); }
  if (chartXDots?.length > 25) { return (<>{t('analitics.chartOverload')}</>); }

  var colors = [
    '#000000', '#FF0000', '#0000FF', '#728C00', '#C19A6B', '#FF00FF', '#800000', '#008000', '#FFDB58', '#FFD700',
    '#00BFFF', '#00FFFF', '#31906E', '#3C565B', '#C04000', '#827839', '#B8860B', '#806517', '#212F3C', '#641E16',
    '#145A32', '#1B4F72', '#7D6608', '#17A589', '#808000'];

  const ToggleLine = (e) => { setLineProps({ ...lineProps, [e.value]: !lineProps[e.value] }); };

  return (
    <Container fluid>
      <LineChart width={1100} height={600} data={chartXDots} margin={{ top: 5, right: 30, left: 20, bottom: 5 }}>
        <CartesianGrid strokeDasharray="3 3" />
        <XAxis dataKey="name" />
        <YAxis />
        <Tooltip />
        <Legend layout="vertical" align="right" verticalAlign="middle" onClick={ToggleLine} />
        {
          // Для каждой линии подбираем соответствующее точке (на оси Х) значение из ее собственного массивы данных.
          linesDataList.map((item, i) =>
            multidata ?
              <Line
                type="monotone"
                key={'lineChart_' + i}
                name={item.name}
                dataKey={(currentDot) => getValue(currentDot, item.data)}
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


// currentDot - точка на графике, для которой ищем реальные данные.
// lineData - массив реальных данных по категории (1-ой линии графика).
function getValue(currentDot, lineData) {
  const lineIndex = lineData.findIndex(obj => obj.name === currentDot.name);
  return lineIndex === -1 ? 0 : lineData[lineIndex].value;
};