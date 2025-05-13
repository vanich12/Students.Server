import React, { useEffect, useState, useMemo, useCallback } from 'react'
import { Modal, Form, Table, Alert, Spin, Button } from "antd"
import DropdownMenu from '../../business/baseComponents/DropdownMenu'
import { request } from 'axios'

// модальое окно для отображения в нем чьих либо данных

const SelectModalItemsForm = ({ control, config, filterString,request,modalFooter, modalTitle}) => {
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
    const shouldSkipQuery = !showForm || !filterString;
    console.log(!showForm || !filterString)

    const {
        data: dataFromServer,
        isLoading: isQueryLoading,
        isFetching: isQueryFetching,
        error: queryError,

    } = useGetAllPagedAsync(
        {
            pageNumber: tableParams.pagination.current,
            pageSize: tableParams.pagination.pageSize,
            filterDataReq: filterString,
        },
        {
            skip: shouldSkipQuery,
        }
    );
    const processedEntityData = useMemo(() => {
        console.log("[SelectModalItemsForm] Recalculating processedEntityData. Input:", dataFromServer);
        const normalizedData = dataFromServer ? dataFromServer?.data : dataFromServer;
        return dataConverter(normalizedData || []);
    }, [dataFromServer, serverPaged, dataConverter]);

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
        type: 'radio', // Позволяет выбрать только одну строку
        selectedRowKeys: selectedRowKey ? [selectedRowKey] : [], // Передаем ID текущей выбранной строки
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
                {/*    <DropdownMenu
                        open={menuVisible}
                        menuPosition={menuPosition}
                        items={menuItems}
                        menuClose={handleMenuClose}
                    />*/}
                </>
            </Modal>
        </>
    );
};

export default SelectModalItemsForm;