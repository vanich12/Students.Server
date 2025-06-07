import {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetOneByIdAsync,
    useAddOneAsync,
    useEditOneAsync,
    useRemoveOneAsync,
} from '../crud/orderCrud.js';
import { orderModel } from '../models/index.js';
import React from 'react';

import OrderDate from '../../components/shared/business/OrderDate.jsx';

export default {
    detailsLink: 'order',
    hasDetailsPage: true,
    serverPaged: false,
    properties: orderModel,
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
            title: 'Номер приказа',
            dataIndex: 'number',
            key: 'number',
        },
        {
            title: 'Вид приказа',
            dataIndex: 'kindOrderName',
            key: 'kindOrderName',
        },
        {
            title: 'Дата',
            dataIndex: 'date',
            key: 'date',
        },
        {
            title: 'Группа',
            dataIndex: 'groupName',
            key: 'groupName',
        },
        {
            title: 'Заявка',
            dataIndex: 'requestFullName',
            key: 'requestFullName',
        },
    ],
    dataConverter: (response) => {
        const dataArray = response?.data || response;
        if (!Array.isArray(dataArray)) {
            return [];
        }

        return dataArray.map(({ date, ...props }) => {
            const orderDate1 = (
                <OrderDate value={date} mode='info' />
            );
            return { ...props, orderDate1 };
        });
    },
};