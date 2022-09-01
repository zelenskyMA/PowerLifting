import React from "react";

import { useTable, useFilters, useGlobalFilter, useAsyncDebounce, usePagination } from 'react-table'
import { Col, Container, InputGroup, Row, Input, InputGroupText } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';


function FilterPanel({ globalFilter, setGlobalFilter, gotoPage }) {
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
                gotoPage(0);
              }}
              placeholder={`введите название`}
            />
          </InputGroup>
        </Col>
      </Row>
    </Container>
  );
}

function PaginationPanel({
  canPreviousPage,
  canNextPage,
  pageOptions,
  pageCount,
  gotoPage,
  nextPage,
  previousPage,
  pageIndex
}) {

  return (
    <Container fluid>
      <Row>
        <Col xs={6} md={{ offset: 6 }}>
          <ul className="pagination">
            <li className="page-item" onClick={() => gotoPage(0)} disabled={!canPreviousPage}>
              <a className="page-link">First</a>
            </li>
            <li className="page-item" onClick={() => previousPage()} disabled={!canPreviousPage}>
              <a className="page-link">{'<'}</a>
            </li>
            <li className="page-item" onClick={() => nextPage()} disabled={!canNextPage}>
              <a className="page-link">{'>'}</a>
            </li>
            <li className="page-item" onClick={() => gotoPage(pageCount - 1)} disabled={!canNextPage}>
              <a className="page-link">Last</a>
            </li>
            <li>
              <a className="page-link">
                Page{' '}
                <strong>
                  {pageIndex + 1} of {pageOptions.length}
                </strong>{' '}
              </a>
            </li>
            <li>
              <a className="page-link">
                <input
                  className="form-control"
                  type="number"
                  defaultValue={pageIndex + 1}
                  onChange={e => {
                    const page = e.target.value ? Number(e.target.value) - 1 : 0
                    gotoPage(page)
                  }}
                  style={{ width: '100px', height: '20px' }}
                />
              </a>
            </li>{' '}
          </ul>
        </Col>
      </Row>
    </Container>
  );
}

export function TableView({ columns, data }) {

  const {
    getTableProps,
    getTableBodyProps,
    headerGroups,
    prepareRow,
    page,
    canPreviousPage,
    canNextPage,
    pageOptions,
    pageCount,
    gotoPage,
    nextPage,
    previousPage,
    state,
    state: { pageIndex },
    setGlobalFilter,
  } = useTable(
    {
      columns,
      data,
      initialState: { pageIndex: 0, pageSize: 10 },
    },
    useFilters,
    useGlobalFilter,
    usePagination
  );

  return (
    <>
      <FilterPanel globalFilter={state.globalFilter} setGlobalFilter={setGlobalFilter} gotoPage={gotoPage} />
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
          {page.map((row, i) => {
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
      <PaginationPanel
        canPreviousPage={canPreviousPage} canNextPage={canNextPage} pageOptions={pageOptions} pageCount={pageCount}
        gotoPage={gotoPage} nextPage={nextPage} previousPage={previousPage} pageIndex={pageIndex} />
    </>
  )
}