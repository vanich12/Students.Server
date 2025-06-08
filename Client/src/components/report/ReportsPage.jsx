import React, { useState } from 'react';
import { Layout } from '../shared/layout/index.js';
import { Card, Button, DatePicker, Space, Typography, Divider, message } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import config from '../../storage/catalogConfigs/report.js'
import dayjs from 'dayjs';
import { useAddPFDOAsync } from '../../storage/crud/reportsCrud'

// Деструктурируем нужные компоненты для удобства
const { Title, Paragraph } = Typography;
const { RangePicker } = DatePicker;

const ReportsPage = () => {
    const { fields, properties, detailsLink, crud, columns, serverPaged, dataConverter } = config;
    const {useAddPFDOAsync} = crud;
    const [ addPFDO, { error, isLoading } ] = useAddPFDOAsync();

    // Состояние для хранения выбранного диапазона дат
    const [dateRange, setDateRange] = useState(null);
    // Состояние для отслеживания процесса загрузки
    const [loading, setLoading] = useState(false);

    const handleDownloadFRDO = async () => {
        if (!dateRange || !dateRange[0] || !dateRange[1]) {
            message.warning('Пожалуйста, выберите период для формирования отчета.');
            return;
        }

        // Форматируем даты в строку 'YYYY-MM-DD' для отправки на бэкенд
        const startDate = dateRange[0].format('YYYY-MM-DD');
        const endDate = dateRange[1].format('YYYY-MM-DD');

        try {
            // Шаг 2.4: Вызываем триггер мутации.
            // Метод unwrap() возвращает промис, который либо успешно разрешится с данными,
            // либо будет отклонен с ошибкой. Это позволяет использовать try/catch.
            const response = await addPFDO({ startDate, endDate }).unwrap();

            // Вся остальная логика остается почти такой же,
            // потому что мы получаем тот же самый объект Response.
            if (!response.ok) {
                throw new Error(`Ошибка сети: ${response.statusText}`);
            }

            const contentDisposition = response.headers.get('content-disposition');
            let filename = 'FRDO_Report.xlsx'; // Имя по умолчанию
            if (contentDisposition && contentDisposition.includes('attachment')) {
                const filenameMatch = contentDisposition.match(/filename="?([^"]+)"?/);
                if (filenameMatch && filenameMatch[1]) {
                    filename = filenameMatch[1];
                }
            }

            const blob = await response.blob();
            const downloadUrl = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadUrl;
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            link.parentNode.removeChild(link);
            window.URL.revokeObjectURL(downloadUrl);

            message.success('Отчет успешно сформирован и скачан!');

        } catch (error) {
            console.error("Ошибка при скачивании отчета:", error);
            // RTK Query может вернуть более детальную ошибку
            const errorMessage = error.data?.message || 'Не удалось скачать отчет. Пожалуйста, попробуйте еще раз.';
            message.error(errorMessage);
        }
    };

    return (
        <Layout title="Формирование отчетов">
            <Space direction="vertical" size="large" style={{ width: '100%' }}>
                {/* --- Карточка для отчета ФРДО --- */}
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
                            // Можно задать форматы или локализацию, если нужно
                        />

                        <Paragraph style={{ marginBottom: 0 }}>
                            <strong>2. Сформируйте и скачайте отчет:</strong>
                        </Paragraph>
                        <Button
                            type="primary"
                            icon={<DownloadOutlined />}
                            loading={loading}
                            onClick={handleDownloadFRDO}
                        >
                            {loading ? 'Формирование...' : 'Сформировать и скачать .xlsx'}
                        </Button>
                    </Space>
                </Card>

                {/* --- Здесь можно добавить карточку для другого отчета, например, Росстат --- */}
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