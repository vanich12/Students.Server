import React, { useEffect, useState, useMemo } from 'react';
import { Modal, Form, Table, Alert, Input, message } from "antd"; // Добавлены Alert, Input, message

const { Search } = Input; // Компонент для поиска

const ObjectsChoiceModalForm = ({ control, requestConfig, groupConfig, id }) => {
    const { crud: crudData, columns, dataConverter, serverPaged } = requestConfig;
    const { crud: crudGroup } = groupConfig;

    const { useGetAllPagedAsync } = crudData;

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
    } = useGetAllPagedAsync({
        pageNumber: tableParams.pagination.current,
        pageSize: tableParams.pagination.pageSize,
        filterDataReq: queryString,
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

    // --- Обработчики ---
    const onSubmit = () => {
        console.log('Submitting selected keys:', selectedRowKeys, 'for group id:', id);
        // Вызываем ТРИГГЕР мутации, передавая массив КЛЮЧЕЙ (ID)
        // Убедитесь, что ваш API ожидает поле 'objects' и массив ID
        addItemsTrigger({ objects: selectedRowKeys, groupId: id });
    };

    const handleTableChange = (pagination, filters, sorter) => {
        setTableParams(prev => ({
            ...prev,
            pagination, // Обновляем пагинацию
            // filters, // Можно добавить обработку фильтров
            // sorter, // Можно добавить обработку сортировки
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
            // Обновляем состояние при изменении
        },
    };

    // --- Рендер ---
    if (!showRangeForm) {
        return null;
    }

    return (
        <Modal
            title="Выберите элементы"
            open={showRangeForm}
            okText="Добавить"
            cancelText="Отмена"
            width={900}
            okButtonProps={{
                form: 'objects_choice_form_in_modal', // ID формы
                htmlType: 'submit',                  // Тип кнопки
                loading: isMutationLoading,          // Показываем загрузку при отправке
                disabled: selectedRowKeys.length === 0, // Блокируем, если ничего не выбрано
            }}
            onCancel={() => setShowRangeForm(false)}// Или другая подходящая ширина
            destroyOnClose
            // Убрали style={{minWidth:'180vh'}} - это скорее всего ошибка, ширина задается через width
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

                {/* Ошибки */}
                {queryError && (
                    <Alert message="Ошибка загрузки данных" description={JSON.stringify(queryError)} type="error" showIcon style={{ marginBottom: 16 }} />
                )}
                {isMutationError && (
                    <Alert message="Ошибка добавления" description={JSON.stringify(mutationError)} type="error" showIcon style={{ marginBottom: 16 }} />
                )}

                {/* Таблица */}
                <Table
                    rowSelection={{ // Используем стандартный выбор строк
                        type: 'checkbox',
                        ...rowSelectionConfig,
                    }}
                    rowKey={(record) => record.id} // Ключ строки (убедитесь, что ID уникальны)
                    dataSource={processedTableData} // Обработанные данные
                    loading={isQueryLoading || isQueryFetching} // Загрузка данных для таблицы
                    columns={columns} // Колонки
                    scroll={{ y: 400 }} // Ограничение высоты
                    pagination={{ // Настройки пагинации
                        ...tableParams.pagination,
                        position: ['bottomRight'],
                        showSizeChanger: true, // Разрешить менять кол-во на странице
                    }}
                    onChange={handleTableChange} // Обработчик пагинации/фильтров/сортировки
                    // Убрали onRow - используем rowSelection
                />
            </Form>
        </Modal>
    );
};

export default ObjectsChoiceModalForm;