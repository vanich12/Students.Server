import React, { useEffect, useState, useMemo, useCallback } from 'react'
import { Modal, Table, Alert } from "antd"

// модальное окно для отображения в нем чьих либо данных
const SelectModalItemsForm = ({ control, config, filterString,request,modalFooter, modalTitle, data, useDataHook,
    dataHookArgs}) => {
    const {detailsLink, crud , columns , dataConverter , serverPaged } = config;
    const { useGetAllPagedAsync} = crud;

    // надо подумать как передавать флаг закрытия формы
    const { showForm, setShowForm,onRowHandleFunction} = control;
    const [menuVisible, setMenuVisible] = useState(false);
    const [currentEntity, setCurrentEntity] = useState();
    const [selectedRowKey, setSelectedRowKey] = useState(null);
    const [menuPosition, setMenuPosition] = useState({ x: 0, y: 0 });
    const [tableParams, setTableParams] = useState({
        pagination: {
            current: 1,
            pageSize: 10,
            total: 0,
        },
    });

    // Выбираем, какой хук использовать: переданный или из config
    const actualUseDataHook = useDataHook || useGetAllPagedAsync;

    // Формируем аргументы для хука
    const queryArgs = dataHookArgs !== undefined ? dataHookArgs : {
        pageNumber: tableParams.pagination.current,
        pageSize: tableParams.pagination.pageSize,
        filterDataReq: filterString,
    };

    const {
        data: dataFromServerResponse,
        isLoading: isQueryLoading,
        isFetching: isQueryFetching,
        error: queryError,
    } = actualUseDataHook(
        queryArgs,

    );

    const dataItems = dataFromServerResponse?.data || dataFromServerResponse || [];
    const totalItems = dataFromServerResponse?.totalCount || dataFromServerResponse?.total || dataItems.length;


    const processedEntityData = useMemo(() => {
        console.log("[SelectModalItemsForm] Recalculating processedEntityData. Input:", dataItems);
        return dataConverter(dataItems || []);
    }, [dataItems, dataConverter]);

    useEffect(() => {
        if (!setShowForm) {
            setCurrentEntity(null);
        }
    }, [showForm]);

    const handleRowContextMenu = useCallback((record, event) => {
        event.preventDefault();
        if (!record) return;
        setCurrentEntity(record);
        setMenuPosition({ x: event.clientX, y: event.clientY });
        setMenuVisible(true);
    }, []);


    const handleMenuClose = useCallback((visible) => {
        if (!visible) {
            setMenuVisible(false);
        }
    }, []);

    const handleRowSelectChange = (selectedKeys, selectedRows) => {

        if (selectedKeys.length > 0) {
            setSelectedRowKey(selectedKeys[0]);
        } else {
            setSelectedRowKey(null);
        }
    };

    const rowSelectionConfig = {
        type: 'radio',
        selectedRowKeys: selectedRowKey ? [selectedRowKey] : [],
        onChange: handleRowSelectChange,
    };
    const handleTableChange = (pagination, filters, sorter) => {
        console.log("handleTableChange")
        setTableParams(prev => ({
            ...prev,
            pagination,
            // filters,
            // sorter,
        }));
    };

    return (
        <>
            <Modal
                title={modalTitle}
                open={showForm}
                footer={modalFooter}
                width={900}
                onCancel={() => {
                    setShowForm(false)

                }}
                destroyOnClose
            >
                <>
                    {queryError && (
                        <Alert message="Ошибка загрузки данных" description={JSON.stringify(queryError)} type="error" showIcon style={{ marginBottom: 16 }} />
                    )}
                    <Table
                        rowKey={(record) => record.id}
                        rowSelection={rowSelectionConfig}
                        dataSource={processedEntityData}
                        loading={isQueryLoading || isQueryFetching}
                        columns={columns}
                        scroll={{ y: 400 }}
                        pagination={{
                            ...tableParams.pagination,
                            position: ['bottomRight'],
                            showSizeChanger: true,
                        }}
                        onRow={(record, rowIndex) => {
                            return {
                                onClick: (event) => {
                                    if (event.target.tagName?.toLowerCase() === 'td' || event.target.tagName?.toLowerCase() === 'span') {
                                        onRowHandleFunction(record);
                                        setSelectedRowKey(record.id);
                                    } else {
                                        if (menuVisible) setMenuVisible(false);
                                    }
                                },
                                onContextMenu: (event) => {
                                    handleRowContextMenu(record, event);
                                    setSelectedRowKey(record.id);
                                },
                            };
                        }
                        }
                        onChange={handleTableChange}
                    />
                </>
            </Modal>
        </>
    );
};

export default SelectModalItemsForm;