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
            dataIndex: 'numberOrder',
            key: 'numberOrder',
        },
        {
            title: 'Вид приказа',
            dataIndex: 'kindOrder',
            key: 'kindOrder',
        },
        {
            title: 'Дата',
            dataIndex: 'orderDate',
            key: 'orderDate',
        },
        {
            title: 'Группа',
            dataIndex: 'startGroupDate',
            key: 'startGroupDate',
        },
        {
            title: 'Обучающийся',
            dataIndex: 'student',
            key: 'student',
        },
    ],
    dataConverter: (data) => {
        return data?.map(({ orderDate, ...props }) => {
            const orderDate1 = (
                <OrderDate value={orderDate} mode='info' />
            );
            return { ...props, orderDate1 };
        });
    },
};