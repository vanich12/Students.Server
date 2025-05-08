import React, { useEffect, useState, useMemo, useCallback } from 'react'
import { Modal, Form, Table, Alert, Spin, Button } from "antd"
import DropdownMenu from '../../business/baseComponents/DropdownMenu'

// модальое окно для отображения в нем чьих либо данных, с дроплаун меню
const ModalItemsForm = ({ control, modalCongig}) => {
    const {config, menuItems} = modalCongig;
    const {detailsLink, crud , columns , dataConverter , serverPaged } = config;
    const { useGetAllPagedAsync} = crud;
    // надо подумать как передавать флаг закрытия формы
    const { showForm, setShowForm,onRowHandleFunction} = control;

    const [menuVisible, setMenuVisible] = useState(false);
    const [currentEntity, setCurrentEntity] = useState();
    const [menuPosition, setMenuPosition] = useState({ x: 0, y: 0 });
    const [tableParams, setTableParams] = useState({
        pagination: {
            current: 1,
            pageSize: 10,
            total: 0,
        },
    });

    const {
        data: dataFromServer,
        isLoading: isQueryLoading,
        isFetching: isQueryFetching,
        error: queryError,
    } = useGetAllPagedAsync({
        pageNumber: tableParams.pagination.current,
        pageSize: tableParams.pagination.pageSize,
        filterDataReq:"",
    });

    const processedEntityData = useMemo(() => {
        console.log("[ModalItemsPanel] Recalculating processedEntityData. Input:", dataFromServer);
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

    // закрытие Dropdown
    const handleMenuClose = useCallback((visible) => {а
        if (!visible) {
            setMenuVisible(false);
        }
    }, []);

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
                open={setShowForm}
                footer={modalFooter}
                width={900}
                onCancel={() => setShowForm(false)}
                destroyOnClose
            >
                    <>
                        {queryError && (
                            <Alert message="Ошибка загрузки данных" description={JSON.stringify(queryError)} type="error" showIcon style={{ marginBottom: 16 }} />
                        )}
                        <Table
                            rowKey={(record) => record.id}
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
                                        } else {
                                            if (menuVisible) setMenuVisible(false);
                                        }
                                    },
                                    onContextMenu: (event) => {
                                        handleRowContextMenu(record, event);
                                    },
                                };
                            }
                            }
                            onChange={handleTableChange}
                        />
                        <DropdownMenu
                            open={menuVisible}
                            menuPosition={menuPosition}
                            items={menuItems}
                            menuClose={handleMenuClose}
                        />
                    </>
            </Modal>


        </>
    );
};

export default ModalItemsForm;