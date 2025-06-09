import React, { useState } from 'react';
import { Layout } from '../shared/layout/index.js';
import { Card, Button, DatePicker, Space, Typography, Divider, message } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import { useAddPFDOReportMutation } from '../../storage/services/reportApi';


const { Title, Paragraph } = Typography;
const { RangePicker } = DatePicker;

const ReportsPage = () => {
    const [addPFDOReport, { isLoading }] = useAddPFDOReportMutation();

    const [dateRange, setDateRange] = useState(null);

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

                        <Paragraph style={{ marginBottom: 0 }}>
                            <strong>2. Сформируйте и скачайте отчет:</strong>
                        </Paragraph>
                        <Button
                            type="primary"
                            icon={<DownloadOutlined />}
                            loading={isLoading}
                            onClick={handleDownloadFRDO}
                        >
                            {isLoading ? 'Формирование...' : 'Сформировать и скачать .xlsx'}
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
        </Layout>
    );
};

export default ReportsPage;