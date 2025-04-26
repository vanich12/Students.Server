import {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetOneByIdAsync,
    useAddOneAsync,
    useEditOneAsync,
    useRemoveOneAsync,
} from '../crud/kindOrderCrud.js';
import { kindOrderModel } from '../models/index.js';

export default {
    detailsLink: 'kindOrder',
    hasDetailsPage: false,
    serverPaged: false,
    properties: kindOrderModel,
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
            title: 'Тип приказа',
            dataIndex: 'name',
            key: 'name',
        },
    ],
    dataConverter: (data) => data,
};