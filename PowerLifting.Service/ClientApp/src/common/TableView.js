import React from "react";

import { useTable, useFilters, useGlobalFilter, useAsyncDebounce, usePagination } from 'react-table'
import { Col, Container, InputGroup, Row, Input, InputGroupText } from 'reactstrap';
import 'bootstrap/dist/css/bootstrap.min.css';

function FilterPanel({ globalFilter, setGlobalFilter, gotoPage }) {
  const [value, setValue] = React.useState(globalFilter);
  const onChange = useAsyncDebounce(value => { setGlobalFilter(value || undefined) }, 200);

  return (
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
    <Row>
      <Col xs={4} md={{ offset: 8 }}>
        <ul className="pagination">
          <li className="page-item" role="button" onClick={() => gotoPage(0)} disabled={!canPreviousPage}>
            <a className="page-link ">{'<<'}</a>
          </li>
          <li className="page-item " role="button" onClick={() => previousPage()} disabled={!canPreviousPage}>
            <a className="page-link">{'<'}</a>
          </li>
          <li>
            <a className="page-link disabled">
              Страниц:{' '}<strong>{pageIndex + 1}</strong> из <strong>{pageOptions.length}</strong>{' '}
            </a>
          </li>
          <li className="page-item" role="button" onClick={() => nextPage()} disabled={!canNextPage}>
            <a className="page-link">{'>'}</a>
          </li>
          <li className="page-item" role="button" onClick={() => gotoPage(pageCount - 1)} disabled={!canNextPage}>
            <a className="page-link">{'>>'}</a>
          </li>
          <li>
            <a className="page-link">
              <Input
                className="form-control"
                type="number"
                defaultValue={pageIndex + 1}
                onChange={e => {
                  const page = e.target.value ? Number(e.target.value) - 1 : 0
                  gotoPage(page)
                }}
                style={{ width: '75px', height: '24px' }}
              />
            </a>
          </li>{' '}
        </ul>
      </Col>
    </Row>
  );
}

export function TableView({ columnsInfo, data, rowDblClick, pageSize = 5 }) {
  const columns = React.useMemo(() => columnsInfo, []);

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
      initialState: { pageIndex: 0, pageSize: pageSize, hiddenColumns: ['id'] },
    },
    useFilters,
    useGlobalFilter,
    usePagination
  );

  return (
    <Container fluid>
      <FilterPanel globalFilter={state.globalFilter} setGlobalFilter={setGlobalFilter} gotoPage={gotoPage} />
      <table className="table table-striped" aria-labelledby="tabelLabel" {...getTableProps()}>
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
              <tr {...row.getRowProps()} onDoubleClick={() => rowDblClick(row)}>
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
    </Container>
  )
}