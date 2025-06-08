import {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetOneByIdAsync,
     useAddOneAsync,
    useEditOneAsync,
    useRemoveOneAsync,
} from '../crud/personHistoryCrud.js';
import { personHistoryModel } from '../models/index.js';
export default {
    detailsLink: 'personHistory',
    hasDetailsPage: true,
    serverPaged: true,
    properties: personHistoryModel,
    crud: {
        useGetAllAsync,
        useGetAllPagedAsync,
        useGetOneByIdAsync,
        useAddOneAsync,
        useEditOneAsync,
        useRemoveOneAsync,
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