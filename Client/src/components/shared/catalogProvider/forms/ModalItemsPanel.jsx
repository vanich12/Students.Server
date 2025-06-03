import React, { useEffect, useState, useMemo, useCallback } from 'react'
import { Modal, Form, Table, Alert, Spin, Button } from "antd"
import DropdownMenu from '../../business/baseComponents/DropdownMenu'
import { useNavigate } from 'react-router-dom'


const ModalItemsPanel = ({ control, requestConfig, studentConfig,id }) => {
    const {detailsLink: studentDetailsLink, crud: crudStudentData, columns: studentColumns, dataConverter: studentDataConverter, serverPaged: studentServerPaged } = studentConfig;
    const {detailsLink: requestDetailsLink, crud: crudRequestData, columns: requestColumns, dataConverter: requestDataConverter, serverPaged: requestServerPaged } = requestConfig;

    const { useGetReqByStudentId } = crudRequestData;
    const { useGetAllPagedAsync } = crudStudentData;

    const [currentEntity, setCurrentEntity] = useState();
    const [menuVisible, setMenuVisible] = useState(false);
    const [viewMode, setViewMode] = useState('students');
    const [studentQueryString, setStudentQueryString] = useState();
    const [menuPosition, setMenuPosition] = useState({ x: 0, y: 0 });
    const navigate = useNavigate();

    const { showStudentInGroupForm, setShowStudentInGroupForm, removeFromGroupTrigger } = control;
    const [form] = Form.useForm(); // Форма Antd (если нужны доп. поля)


    const [tableParams, setTableParams] = useState({
        pagination: {
            current: 1,
            pageSize: 10,
            total: 0,
        },
    });

    const [trigger, result] = useGetReqByStudentId();

    const processedRequestsData = useMemo(() => {
        return requestDataConverter(result.data || []);
    }, [result.data, requestDataConverter]);

    const {
        data: studentDataFromServer,
        isLoading: isQueryLoading,
        isFetching: isQueryFetching,
        error: queryError,
        refetch: refetchStudents,
    } = useGetAllPagedAsync({
        pageNumber: tableParams.pagination.current,
        pageSize: tableParams.pagination.pageSize,
        filterDataReq: `&groupId=${id}`,
    });

    const handleDeleteStudent = useCallback(async (student, refetch) => {
        if (!student || !student.id) {
            console.log("Нет данных студента для удаления");
            return;
        }
        console.log(student,id)
       await removeFromGroupTrigger({studentId : student.id,groupId: id});
        refetch();
    },[removeFromGroupTrigger]);


    const processedStudentData = useMemo(() => {
        console.log("[ModalItemsPanel] Recalculating processedStudentData. Input:", studentDataFromServer);
        const normalizedData = studentServerPaged ? studentDataFromServer?.data : studentDataFromServer;
        return studentDataConverter(normalizedData || []);
    }, [studentDataFromServer, studentServerPaged, studentDataConverter]);


    const handleTableChange = (pagination, filters, sorter) => {
        console.log("handleTableChange")
        setTableParams(prev => ({
            ...prev,
            pagination,
            // filters,
            // sorter,
        }));
    };

    // --- useEffect для обновления total в пагинации ---
/*    useEffect(() => {
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
    }, [dataFromServer, serverPaged]);*/

    const handleMenuClose = useCallback((visible) => {
        // Вызывается Dropdown'ом при клике вне меню или выборе пункта
        if (!visible) {
            setMenuVisible(false);
        }
    }, []);

    const chooseNewStudent = useCallback((student) => {
        setViewMode('requests');
        // запрос на заявки студента к серверу
        trigger(student.id);
    }, [trigger]);

    const handleBackToStudents =()=>{
        setViewMode('students');

        if (result.reset) {
            result.reset();
        }
    }
// используется при сбросе , закрытии модального окна
    useEffect(() => {
        if (!showStudentInGroupForm) {
            setViewMode('students');
            setCurrentEntity(null);
            setStudentQueryString('');
            if (result.reset) {
                result.reset();
            } else if (result.originalArgs !== undefined) {
            }
        }
    }, [showStudentInGroupForm]);

    const modalTitle = useMemo(() => (
        viewMode === 'requests'
            ? `Заявки студента: ${currentEntity?.fullName || currentEntity?.family || '...'}`
            : "Список студентов" // Или оригинальный заголовок
    ), [viewMode, currentEntity]);

    const modalFooter = useMemo(()=>(
        viewMode === 'students'
            ? [ <Button key="cancel" onClick={() => setShowStudentInGroupForm(false)}>Отмена</Button> ]
            : [
                <Button key="back" onClick={handleBackToStudents}>Назад к студентам</Button>,
                <Button key="close" onClick={() => setShowStudentInGroupForm(false)}>Закрыть</Button>
            ]
    ), [viewMode, setShowStudentInGroupForm]);

    const studentMenuItems = useMemo(() => [
        {
            key: 'delete',
            label: 'Удалить из группы',
            // В onClick вызываем обработчик, используя данные из state (currentRowRecord)
            onClick: () => {
                handleDeleteStudent(currentEntity,refetchStudents);
                // Меню закроется автоматически при клике на пункт
            },
            danger: true,
        },
        {
            key: 'navigateToEdit',
            label: 'Редактировать',
            onClick: () => {
                openDetailsInfo(currentEntity,studentDetailsLink)
            },
        }

    ], [handleDeleteStudent, currentEntity]);

    const requestMenuItems = useMemo(() =>[
        {
            key: 'navigateToEdit',
            label: 'Редактировать',
            onClick: () => {
                openDetailsInfo(currentEntity,requestDetailsLink)
            },
        }
    ],[currentEntity])

    const openDetailsInfo = useCallback((item,detailsLink) => {
        navigate(`/${detailsLink}/${item.id}`);
    },[]);

    const handleRowContextMenu = useCallback((record, event) => {
        event.preventDefault();
        if (!record) return;
        setCurrentEntity(record);
        setMenuPosition({ x: event.clientX, y: event.clientY });
        console.log(event.clientX,  event.clientY);
        setMenuVisible(true);
    }, []);

    return (
        <>
        <Modal
            title={modalTitle}
            open={showStudentInGroupForm}
           footer={modalFooter}
            width={900}
            onCancel={() => setShowStudentInGroupForm(false)}
            destroyOnClose
        >
            { viewMode === 'students' && (
                <>
                    {queryError && (
                        <Alert message="Ошибка загрузки данных" description={JSON.stringify(queryError)} type="error" showIcon style={{ marginBottom: 16 }} />
                    )}

                    <Table
                        rowKey={(record) => record.id}
                        dataSource={processedStudentData}
                        loading={isQueryLoading || isQueryFetching}
                        columns={studentColumns}
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
                                        chooseNewStudent(record);
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
                        items={studentMenuItems}
                        menuClose={handleMenuClose}
                    />
                </>
                )}
            {viewMode === 'requests' && (
                <>
                    {result.isError && (
                        <Alert message="Ошибка загрузки заявок" description={JSON.stringify(result.error)} type="error" showIcon style={{ marginBottom: 16 }} />
                    )}
                    {(result.isLoading || result.isFetching) && (
                        <Spin tip="Загрузка заявок..." style={{ display: 'block', textAlign: 'center', margin: '20px 0' }} />
                    )}
                    {!result.isLoading && !result.isFetching && !result.isError && (
                        <Table
                            rowKey="id"
                            dataSource={processedRequestsData}
                            columns={requestColumns}
                            pagination={false}
                            scroll={{ y: 400 }}
                            onRow={(record, rowIndex) => {
                                return {
                                    onContextMenu: (event) => {
                                        handleRowContextMenu(record, event);
                                    },
                                };

                            }}
                        />
                    )}
                    <DropdownMenu
                        open={menuVisible}
                        menuPosition={menuPosition}
                        items={requestMenuItems}
                        menuClose={handleMenuClose}
                    />
                </>
            )}
        </Modal>


        </>
    );
};

export default ModalItemsPanel;