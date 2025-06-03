import React, { useEffect, useState, useMemo } from 'react';
import { Modal, Form, Table, Alert, Input, message } from "antd";
import { useGetRequestToAddInGroupAsync } from '../../../../storage/crud/personRequestsCrud' // Добавлены Alert, Input, message

const { Search } = Input; // Компонент для поиска
// TODo: Время будет,надо обязательно переделать, и вообще он не нужен будет, в модалке всё можно сжедать в ModalItem
const ObjectsChoiceModalForm = ({ control, requestConfig, groupConfig, id }) => {
    const { crud: crudData, columns, dataConverter, serverPaged } = requestConfig;
    const { crud: crudGroup } = groupConfig;

    const { useGetRequestToAddInGroupAsync } = crudData;

    const { useAddSubjectRangeAsync } = crudGroup;

    const { showRangeForm, setShowRangeForm } = control;
    const [form] = Form.useForm(); // Форма Antd (если нужны доп. поля)
    const [queryString, setQueryString] = useState(''); // Состояние для строки фильтра/поиска
    const [selectedRowKeys, setSelectedRowKeys] = useState([]); // Состояние для КЛЮЧЕЙ выбранных строк

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
    } = useGetRequestToAddInGroupAsync({
        pageNumber: tableParams.pagination.current,
        pageSize: tableParams.pagination.pageSize,
        filterDataReq: `&groupId=${id}&notInThisGroup=true`,
    });

    console.log('ObjectsChoiceModalForm - State on Render:', {
        isQueryLoading,
        isQueryFetching,
        dataFromServer,
        queryError
    });


    const [addItemsTrigger, addItemsResult] = useAddSubjectRangeAsync();
    const {
        isLoading: isMutationLoading,
        isError: isMutationError,
        error: mutationError,
        isSuccess: isMutationSuccess,
    } = addItemsResult;


    const processedTableData = useMemo(() => {
        const normalizedData = serverPaged ? dataFromServer?.data : dataFromServer;
        return dataConverter(normalizedData || []);
    }, [dataFromServer, serverPaged, dataConverter]);

    const onSubmit = () => {
        console.log('Submitting selected keys:', selectedRowKeys, 'for group id:', id);
        addItemsTrigger({ objects: selectedRowKeys, groupId: id });
    };

    const handleTableChange = (pagination, filters, sorter) => {
        setTableParams(prev => ({
            ...prev,
            pagination,
            // filters,
            // sorter,
        }));
    };

    const handleSearch = (value) => {
        setQueryString(value); // Обновляем строку поиска
        setTableParams(prev => ({
            ...prev,
            pagination: {
                ...prev.pagination,
                current: 1,
            }
        }));
    };

    // --- useEffect для обновления total в пагинации ---
    useEffect(() => {
        if (dataFromServer) {
            const total = serverPaged ? dataFromServer?.totalCount : dataFromServer?.length;
            setTableParams(prev => ({
                ...prev,
                pagination: {
                    ...prev.pagination,
                    total: total || 0,
                },
            }));
        }
    }, [dataFromServer, serverPaged]);

    // --- useEffect для закрытия окна при успехе мутации ---
    useEffect(() => {
        if (isMutationSuccess) {
            message.success('Элементы успешно добавлены!'); // Сообщение об успехе
            setShowRangeForm(false); // Закрываем окно
            setSelectedRowKeys([]); // Сбрасываем выбор
        }
    }, [isMutationSuccess, setShowRangeForm]);

    // --- Конфигурация для rowSelection ---
    const rowSelectionConfig = {
        selectedRowKeys: selectedRowKeys, // Управляется состоянием
        onChange: (newSelectedRowKeys) => {
            setSelectedRowKeys(newSelectedRowKeys);
            console.log(selectedRowKeys)
        },
    };

    return (
        <Modal
            title="Выберите элементы"
            open={showRangeForm}
            okText="Добавить"
            cancelText="Отмена"
            width={900}
            okButtonProps={{
                form: 'objects_choice_form_in_modal',
                htmlType: 'submit',
                loading: isMutationLoading,
                disabled: selectedRowKeys.length === 0,
            }}
            onCancel={() => setShowRangeForm(false)}
            destroyOnClose
        >
            <Form
                layout="vertical"
                form={form}
                id="objects_choice_form_in_modal"
                name="objects_choice_form_in_modal"
                onFinish={onSubmit}
            >
                <Search
                    placeholder="Поиск..."
                    allowClear
                    enterButton="Найти"
                    onSearch={handleSearch}
                    loading={isQueryFetching}
                    style={{ marginBottom: 16 }}
                />

                {queryError && (
                    <Alert message="Ошибка загрузки данных" description={JSON.stringify(queryError)} type="error" showIcon style={{ marginBottom: 16 }} />
                )}
                {isMutationError && (
                    <Alert message="Ошибка добавления" description={JSON.stringify(mutationError)} type="error" showIcon style={{ marginBottom: 16 }} />
                )}

                <Table
                    rowSelection={{
                        type: 'checkbox',
                        ...rowSelectionConfig,
                    }}
                    rowKey={(record) => record.id}
                    dataSource={processedTableData}
                    loading={isQueryLoading || isQueryFetching}
                    columns={columns}
                    scroll={{ y: 400 }}
                    pagination={{
                        ...tableParams.pagination,
                        position: ['bottomRight'],
                        showSizeChanger: true,
                    }}
                    onChange={handleTableChange}
                />
            </Form>
        </Modal>
    );
};

export default ObjectsChoiceModalForm;