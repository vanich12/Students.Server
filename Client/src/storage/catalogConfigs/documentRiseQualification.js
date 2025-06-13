import {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetOneByIdAsync,
    useAddOneAsync,
    useEditOneAsync,
    useRemoveOneAsync,
} from '../crud/documentRiseQualificationCrud.js';
import { documentRiseQualificationModel } from '../models/index.js'

export default {
    detailsLink: 'documentRiseQualification',
    hasDetailsPage: false,
    serverPaged: false,
    properties: documentRiseQualificationModel,
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
        {
            title: 'Дата выдачи удостоверения',
            dataIndex: 'date',
            key: 'date',
        },
        {
            title: 'Номер выдачи удостоверения',
            dataIndex: 'number',
            key: 'number',
        },
        {
            title: 'Заявка, на которую выдается документ',
            dataIndex: 'requestId',
            key: 'requestId',
        },
    ],
    dataConverter: (data) => data,
};