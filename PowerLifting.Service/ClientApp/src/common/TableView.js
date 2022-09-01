import React from "react";

import { useTable, useFilters, useGlobalFilter, useAsyncDebounce } from 'react-table'
import { Col, Container, InputGroup, Row, Input, InputGroupText } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';


// Filter panel
function GlobalFilter({ globalFilter, setGlobalFilter, }) {
  const [value, setValue] = React.useState(globalFilter);
  const onChange = useAsyncDebounce(value => { setGlobalFilter(value || undefined) }, 200);

  return (
    <Container fluid>
      <Row>
        <Col xs={6} md={{ offset: 6 }}>
          <InputGroup>
            <InputGroupText>
              Фильтр списка:
            </InputGroupText>
            <Input xs={2}
              className="form-control"
              value={value || ""}
              onChange={e => {
                setValue(e.target.value);
                onChange(e.target.value);
              }}
              placeholder={`введите название`}
            />
          </InputGroup>
        </Col>
      </Row>
    </Container>
  )
}

export function TableView({ columns, data }) {

  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    rows,
    prepareRow,
    state,
    setGlobalFilter,
  } = useTable(
    {
      columns,
      data
    },
    useFilters,
    useGlobalFilter
  );

  return (
    <div>
      <GlobalFilter
        globalFilter={state.globalFilter}
        setGlobalFilter={setGlobalFilter}
      />
      <table className="table" {...getTableProps()}>
        <thead>
          {headerGroups.map(headerGroup => (
            <tr {...headerGroup.getHeaderGroupProps()}>
              {headerGroup.headers.map(column => (
                <th {...column.getHeaderProps()}>
                  {column.render('Header')}
                </th>
              ))}
            </tr>
          ))}
        </thead>
        <tbody {...getTableBodyProps()}>
          {rows.map((row, i) => {
            prepareRow(row)
            return (
              <tr {...row.getRowProps()}>
                {row.cells.map(cell => {
                  return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                })}
              </tr>
            )
          })}
        </tbody>
      </table>
      <br />
      <div>Showing the first 20 results of {rows.length} rows</div>
      <div>
        <pre>
          <code>{JSON.stringify(state.filters, null, 2)}</code>
        </pre>
      </div>
    </div>
  )
}