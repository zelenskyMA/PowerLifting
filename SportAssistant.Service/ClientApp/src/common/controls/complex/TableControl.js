import 'bootstrap/dist/css/bootstrap.min.css';
import React from "react";
import { useTranslation } from 'react-i18next';
import { useFilters, useGlobalFilter, usePagination, useTable } from 'react-table';
import { Col, Input, InputGroup, InputGroupText, Row } from 'reactstrap';

function defaultRowClick(row) { }
function defaultRowDblClick(row) { }

export function TableControl({ columnsInfo, data,
  rowDblClick = defaultRowDblClick, rowClick = defaultRowClick, pageSize = 5, hideFilter = false }) {

  const columns = React.useMemo(() => columnsInfo, []);
  const { t } = useTranslation(); //language pack implementation

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
    state: { pageIndex },
  } = useTable(
    {
      columns,
      data,
      initialState: { pageIndex: 0, pageSize: pageSize, hiddenColumns: ['id', 'userId'] },
    },
    useFilters,
    useGlobalFilter,
    usePagination
  );

  if (data?.length == 0) {
    return (<p><em>{t('common.noRecords')}</em></p>);
  }

  var filterColumn = headerGroups[0].headers.find(t => t.id === 'name');

  return (
    <>
      <FilterPanel column={filterColumn} gotoPage={gotoPage} hideFilter={data?.length <= pageSize || !filterColumn || hideFilter} lngStr={t} />

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
          {page.map((row, index) => {
            prepareRow(row)
            return (
              <tr key={index} {...row.getRowProps()} role="button" onDoubleClick={() => rowDblClick(row)} onClick={() => rowClick(row)}>
                {row.cells.map(cell => {
                  return <td {...cell.getCellProps()}>{cell.render('Cell')}</td>
                })}
              </tr>
            )
          })}
        </tbody>
      </table>

      {data?.length > pageSize &&
        <PaginationPanel lngStr={t}
          canPreviousPage={canPreviousPage} canNextPage={canNextPage} pageOptions={pageOptions} pageCount={pageCount}
          gotoPage={gotoPage} nextPage={nextPage} previousPage={previousPage} pageIndex={pageIndex} />
      }
    </>
  )
}

function FilterPanel({ column, gotoPage, hideFilter, lngStr }) {
  if (hideFilter) { return (<></>); }

  return (
    <Row>
      <Col xs={6} md={{ offset: 6 }}>
        <InputGroup>
          <InputGroupText>{lngStr('control.filter')}:</InputGroupText>
          <Input xs={2}
            className="form-control"
            value={column.filterValue || ""}
            onChange={e => {
              column.setFilter(e.target.value || undefined);
              gotoPage(0);
            }}
            placeholder={lngStr('control.filterPlaceholder')}
          />
        </InputGroup>
      </Col>
    </Row>
  );
}

function PaginationPanel({
  lngStr,
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
      <Col xs={8}>
        <ul className="pagination">
          <li className="page-item" role="button" onClick={() => gotoPage(0)} disabled={!canPreviousPage}>
            <a className="page-link ">{'<<'}</a>
          </li>
          <li className="page-item " role="button" onClick={() => previousPage()} disabled={!canPreviousPage}>
            <a className="page-link">{'<'}</a>
          </li>
          <li>
            <a className="page-link disabled">
              <strong>{pageIndex + 1}</strong> {lngStr('common.outOf')} <strong>{pageOptions.length}</strong>{' '}
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
