import {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetOneByIdAsync,
    useAddOneAsync,
    useEditOneAsync,
    useRemoveOneAsync,
} from '../crud/kindDocumentRiseQualificationCrud.js';
import { kindDocumentRiseQualificationModel } from '../models/index.js';

export default {
    detailsLink: 'kindDocumentRiseQualification',
    hasDetailsPage: false,
    serverPaged: false,
    properties: kindDocumentRiseQualificationModel,
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
            title: 'Вид документа',
            dataIndex: 'kindDocumentRiseQualificationId',
            key: 'kindDocumentRiseQualificationId',
        },
    ],
    dataConverter: (data) => data,
};