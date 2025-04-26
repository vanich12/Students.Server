import React, { useState, useCallback, useMemo, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Table, ConfigProvider } from 'antd';
import { IPagedPageData } from '../../../domain/interfaces'
import FilterPanel from "../catalogProvider/FilterPanel.tsx";


const { Column } = Table;

const EntityTable = ({ config }) => {
    const { fields, properties, detailsLink, crud, columns, serverPaged, dataConverter } = config;
    const { useGetAllPagedAsync, useRemoveOneAsync, useAddOneAsync, useGetOneByIdAsync, useEditOneAsync } = crud;
    const [queryString, setQueryString] = useState('');
    const [query, setQuery] = useState({});
    const [data, setData] = useState<IPagedPageData<any>>(null);
    const [loading, setLoading] = useState(false);
    const [tableParams, setTableParams] = useState({
        pagination: {
            current: 1,
            pageSize: 10,
        },
    });
    const navigate = useNavigate();

    // происхожит запрос при каждом ререндере, че?
    const { 
        data: dataFromServer,
        error,
        isLoading,
        isFetching,
        refetch
    } = useGetAllPagedAsync({ 
        pageNumber: tableParams.pagination.current, 
        pageSize: tableParams.pagination.pageSize, 
        filterDataReq: queryString 
    });

    useEffect(() => {
        if (!isLoading && !isFetching) {

            const normalizedData = serverPaged ? dataFromServer?.data : dataFromServer;
            console.log(normalizedData);
            const total = serverPaged ? dataFromServer?.totalCount : dataFromServer?.length;
            setData(normalizedData);
            console.log(dataConverter(data));
            setLoading(false);
            setTableParams({
                ...tableParams,
                pagination: {
                  ...tableParams.pagination,
                  total,
                  position: ['bottomLeft'],
                },
            });
        }
    }, [
        dataFromServer,
        tableParams.pagination?.current,
        tableParams.pagination?.pageSize,
      /*tableParams?.sortOrder,
        tableParams?.sortField,
        JSON.stringify(tableParams.filters)*/,
    ]);

    const generateQueryString = useCallback(() => {
        return Object.entries(query)
            .map(([key, value]) => `&${key}=${value}`)
            .join('');
    }, [query]);

    // когда изменяется ссылка на generateQueryString, тогда и срабатывает
    // useEffect
    useEffect(() => {
        setQueryString(generateQueryString());
        console.log(queryString);
    }, [generateQueryString]);

    const handleTableChange = (pagination, filters, sorter) => {
        setTableParams({
          pagination,
          filters,
          sortOrder: Array.isArray(sorter) ? undefined : sorter.order,
          sortField: Array.isArray(sorter) ? undefined : sorter.field,
        });
    
        // `dataSource` is useless since `pageSize` changed
        if (pagination.pageSize !== tableParams.pagination?.pageSize) {
          setData([]);
        }
    };

    const openDetailsInfo = useCallback((item) => {
        navigate(`/${detailsLink}/${item.id}`);
    });


    return (
        <>
            <FilterPanel
                config={config}
                query={query}
                setQuery={setQuery}
            />
            <Table
                rowKey={(record) => record.id}
                dataSource={dataConverter(data)}
                pagination={tableParams.pagination}
                loading={loading}
                onChange={handleTableChange}
                columns={columns}
                onRow={(record) => {
                    return {
                      onClick: ({ target }: any) => {
                        if (target.tagName.toLowerCase() === 'td') {
                            openDetailsInfo(record);
                        }
                      },
                      style: { cursor: 'pointer' },
                    };
                }}
            />
        </>
    );
};
export default EntityTable;
