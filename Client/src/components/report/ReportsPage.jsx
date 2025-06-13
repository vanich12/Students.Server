import React, { useCallback, useEffect, useMemo, useState } from 'react'
import { Layout } from '../shared/layout/index.js';
import { Card, Button, DatePicker, Space, Typography, Divider, message, Modal, Table } from 'antd'
import { DownloadOutlined, EyeOutlined } from '@ant-design/icons';
import { useAddPFDOReportMutation, useGetPFDOPreviewQuery } from '../../storage/services/reportApi'
import StandartSubmitDataHookForm from '../shared/catalogProvider/forms/StandartSubmitDataHookForm'
import createReportConfig from '../../storage/catalogConfigs/report'
import SubmitForm from '../shared/catalogProvider/forms/SubmitForm'


const { Title, Paragraph } = Typography;
const { RangePicker } = DatePicker;
// компонент ужасен, все что связано с формой надо переделать, это как затычка, тест
const ReportsPage = () => {

    // ШАГ 1: Объявляем handleEdit и оборачиваем в useCallback.
    const handleEdit = useCallback((record) => {
        console.log(record);
        setCurrentReportItem(record);
        setPreviewEditVisible(true);
    }, []);


    const reportConfig = useMemo(() => createReportConfig({ onEdit: handleEdit }), [handleEdit]);

    const [addPFDOReport, { isLoading: isDownloading }] = useAddPFDOReportMutation();
    const {crud} = reportConfig;
    const {useAddPFDOAsyncFromClient} = crud;

    const [PFDOReportFromClient, { isLoading }] = useAddPFDOAsyncFromClient();

    const [previewParams, setPreviewParams] = useState(null);
    const [reportArray, setReportArray] = useState([]);
    const [currentReportItem, setCurrentReportItem] = useState(null);
    const [previewVisible, setPreviewVisible] = useState(false);
    const [previewEditVisible, setPreviewEditVisible] = useState(false);

    const { data: previewData, error: previewError, isLoading: isPreviewLoading } = useGetPFDOPreviewQuery(previewParams, {
        skip: !previewParams,
    });
    const [dateRange, setDateRange] = useState(null);

    const handlePreview = () => {
        if (!dateRange || !dateRange[0] || !dateRange[1]) {
            message.warning('Пожалуйста, выберите период для предпросмотра.');
            return;
        }
        const startDate = dateRange[0].format('YYYY-MM-DD');
        const endDate = dateRange[1].format('YYYY-MM-DD');

        // Запускаем RTK Query хук, передавая ему параметры
        setPreviewParams({ startDate, endDate });
        setPreviewVisible(true);
    };

    useEffect(() => {
        if (previewData) {
            const dataWithUniqueKeys = previewData.map(item => {
                // Создаем надежный композитный ключ
                const uniqueKey = `${item.registrationNumber}-${item.recipientSNILS}-${item.documentNumber}`;
                return { ...item, key: uniqueKey };
            });
            setReportArray(dataWithUniqueKeys);
        } else {
            setReportArray([]);
        }
    }, [previewData]);

    const handleDownloadFRDO = async () => {
        if (!dateRange || !dateRange[0] || !dateRange[1]) {
            message.warning('Пожалуйста, выберите период для формирования отчета.');
            return;
        }

        const startDate = dateRange[0].format('YYYY-MM-DD');
        const endDate = dateRange[1].format('YYYY-MM-DD');

        try {

            const arrayBuffer = await addPFDOReport({ startDate, endDate }).unwrap();

            const blob = new Blob([arrayBuffer], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
            });

            const filename = `Отчет_ФРДО_${startDate}_${endDate}.xlsx`;

            const downloadUrl = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadUrl;
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();

            document.body.removeChild(link);
            window.URL.revokeObjectURL(downloadUrl);

            message.success('Отчет успешно сформирован и скачан!');

        } catch (error) {
            console.error("Ошибка при скачивании отчета:", error);
            // Ошибка теперь приходит из нашего rawBaseQuery
            const errorMessage = error.error || 'Не удалось скачать отчет. Пожалуйста, попробуйте еще раз.';
            message.error(errorMessage);
        }
    };
    const handleDownloadFRDOFromClient = async () => {
        if (!dateRange || !dateRange[0] || !dateRange[1]) {
            message.warning('Пожалуйста, выберите период для формирования отчета.');
            return;
        }

        const startDate = dateRange[0].format('YYYY-MM-DD');
        const endDate = dateRange[1].format('YYYY-MM-DD');

        try {

            const arrayBuffer = await PFDOReportFromClient(reportArray).unwrap();

            const blob = new Blob([arrayBuffer], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
            });

            const filename = `Отчет_ФРДО_${startDate}_${endDate}.xlsx`;

            const downloadUrl = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadUrl;
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();

            document.body.removeChild(link);
            window.URL.revokeObjectURL(downloadUrl);

            message.success('Отчет успешно сформирован и скачан!');

        } catch (error) {
            console.error("Ошибка при скачивании отчета:", error);
            // Ошибка теперь приходит из нашего rawBaseQuery
            const errorMessage = error.error || 'Не удалось скачать отчет. Пожалуйста, попробуйте еще раз.';
            message.error(errorMessage);
        }
    };

    const handleEditSubmit = async (updatedValuesFromForm) => {
        console.log(updatedValuesFromForm)
        await setReportArray(prevArray =>
            prevArray.map(item => {
                if (item.key === currentReportItem.key) {
                    // Возвращаем новую, объединенную запись
                    return { ...item, ...updatedValuesFromForm };
                } else {
                    return item;
                }
            })
        );
        setPreviewEditVisible(false);
        setCurrentReportItem(null);
        console.log(reportArray);
    };
    return (
        <Layout title="Формирование отчетов">
            <Space direction="vertical" size="large" style={{ width: '100%' }}>
                <Card bordered={true}>
                    <Title level={4}>Отчет для ФИС ФРДО</Title>
                    <Paragraph type="secondary">
                        Выгрузка данных о выданных документах об образовании в формате Excel для
                        последующей загрузки в федеральный реестр.
                    </Paragraph>

                    <Divider />

                    <Space direction="vertical" size="middle">
                        <Paragraph style={{ marginBottom: 0 }}>
                            <strong>1. Выберите период выдачи документов:</strong>
                        </Paragraph>
                        <RangePicker
                            onChange={(dates) => setDateRange(dates)}
                        />
                        <Paragraph><strong>2. Выполните действие:</strong></Paragraph>
                        {/* Группа кнопок */}
                        <Space>
                            <Button
                                type="default"
                                icon={<EyeOutlined />}
                                loading={isPreviewLoading}
                                onClick={handlePreview}
                            >
                                Предпросмотр
                            </Button>
                            <Button
                                type="primary"
                                icon={<DownloadOutlined />}
                                loading={isDownloading}
                                onClick={handleDownloadFRDOFromClient}
                            >
                                {isDownloading ? 'Формирование...' : 'Скачать .xlsx'}
                            </Button>
                        </Space>
                        <Paragraph style={{ marginBottom: 0 }}>
                            <strong>2. Сформируйте и скачайте отчет:</strong>
                        </Paragraph>
                        <Button
                            type="primary"
                            icon={<DownloadOutlined />}
                            loading={isDownloading}
                            onClick={handleDownloadFRDO}
                        >
                            {isDownloading ? 'Формирование...' : 'Сформировать и скачать .xlsx'}
                        </Button>
                    </Space>
                </Card>

                <Card bordered={true}>
                    <Title level={4}>Отчет для Росстата (Форма 1-ПК)</Title>
                    <Paragraph type="secondary">
                        Статистический отчет о деятельности организации, осуществляющей образовательную деятельность
                        по программам профессионального обучения.
                    </Paragraph>
                    <Divider />
                    <Button disabled>Сформировать (в разработке)</Button>
                </Card>
            </Space>

            <Modal
                title="Предпросмотр и редактирование данных"
                open={previewVisible}
                onCancel={() => setPreviewVisible(false)}
                footer={[ <Button key="back" onClick={() => setPreviewVisible(false)}>Закрыть</Button> ]}
                width="95%"
            >
                <Table
                    columns={reportConfig.columns}
                    // ШАГ 5: Таблица всегда использует наш локальный, редактируемый массив
                    dataSource={reportArray}
                    loading={isPreviewLoading}
                    // Используем наше новое свойство 'key'
                    rowKey="key"
                    scroll={{ x: 'max-content' }}
                    pagination={{ pageSize: 10 }}
                />
                {previewError && <p style={{color: 'red'}}>Ошибка загрузки данных: {previewError.data || previewError.error}</p>}
            </Modal>

            {previewEditVisible && currentReportItem && (
                <SubmitForm
                    control={{ showAddOneForm:previewEditVisible, setShowAddOneForm:setPreviewEditVisible }}
                    properties={reportConfig.properties}
                    title={"Редактирование записи"}
                    data={currentReportItem}
                    onSubmit={handleEditSubmit}
                />
            )}
        </Layout>
    );
};

export default ReportsPage;