import {
    useAddPFDOAsync,
    useAddRostatAsync,
} from '../crud/reportsCrud.js';
import { personHistoryModel } from '../models/index.js';
export default {
    detailsLink: 'report',
    hasDetailsPage: true,
    serverPaged: true,
    properties: personHistoryModel,
    crud: {
        useAddPFDOAsync,
        useAddRostatAsync,
    },
    columns: [
        {
            title: 'Фамилия',
            dataIndex: 'newFamily',
            key: 'newFamily',
        },
        {
            title: 'Имя',
            dataIndex: 'newName',
            key: 'newName',
        },
        {
            title: 'Отчество',
            dataIndex: 'newPatron',
            key: 'newPatroon',
        },
        {
            title: 'Дата изменения',
            dataIndex: 'changeDate',
            key: 'changeDate',
        },
        {
            title: 'Тип Изменения',
            dataIndex: 'changeType',
            key: 'changeType',
        },
    ],
    filters: [
    ],
    dataConverter: (data) => data,
};