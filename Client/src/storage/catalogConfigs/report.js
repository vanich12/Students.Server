import {
    useAddPFDOAsync,
    useAddPFDOAsyncFromClient,
    useGetPFDOPreviewQuery,
} from '../crud/reportsCrud.js';
import { FRDOModel } from '../models/index.js';
import { EditOutlined } from '@ant-design/icons';
import { Button } from 'antd';


const createReportConfig = ({ onEdit = (record) => console.warn('Обработчик onEdit не предоставлен', record) }) => {
    return {
        detailsLink: 'report',
        hasDetailsPage: true,
        serverPaged: true,
        properties: FRDOModel,
        crud: {
            useAddPFDOAsync,
            useAddPFDOAsyncFromClient,
            useGetPFDOPreviewQuery,
        },
        columns: [
            {
                title: 'ФИО',
                key: 'fullName',
                width: 250,
                fixed: 'left',
                render: (_, record) =>
                    `${record.recipientLastName || ''} ${record.recipientName || ''} ${record.recipientPatronymic || ''}`.trim(),
            },
            {
                title: 'СНИЛС',
                dataIndex: 'recipientSNILS',
                key: 'recipientSNILS',
                width: 140,
            },
            {
                title: 'Наименование программы',
                dataIndex: 'nameAdditionalProfessionalProgram',
                key: 'nameAdditionalProfessionalProgram',
                width: 350,
                ellipsis: true,
            },
            {
                title: 'Серия документа',
                dataIndex: 'seriesDocuments',
                key: 'seriesDocuments',
                width: 120,
            },
            {
                title: 'Номер документа',
                dataIndex: 'documentNumber',
                key: 'documentNumber',
                width: 150,
            },
            {
                title: 'Рег. номер',
                dataIndex: 'registrationNumber',
                key: 'registrationNumber',
                width: 150,
            },
            {
                title: 'Дата выдачи',
                dataIndex: 'dateOfIssueDocument',
                key: 'dateOfIssueDocument',
                width: 130,
            },
            {
                title: 'Действия',
                key: 'action',
                fixed: 'right',
                width: 140,
                align: 'center',
                render: (_, record) => (
                    <Button
                        type="link"
                        icon={<EditOutlined />}
                        // Теперь onClick вызывает переданную функцию onEdit
                        onClick={() => onEdit(record)}
                    >
                        Редактировать
                    </Button>
                ),
            },
        ],
        filters: [],
        dataConverter: (data) => data,
    };
};

// Экспортируем функцию по умолчанию
export default createReportConfig;