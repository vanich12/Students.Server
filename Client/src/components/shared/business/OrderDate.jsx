import React from 'react';
import _ from 'lodash';
import Date from './common/Date.jsx'

const rules = [
    {
        required: true,
        message: 'Необходимо заполнить дату приказа',
    },
];

const defaultFormParams = {
    key: 'birthDate',
    name: 'Введите дату приказал',
    normalize: (value) => value,
    rules,
    hasFeedback: true,
};

const OrderDate = ({ formParams, ...props }) => {
    return (
        <Date
            {
                ...{
                    defaultValue: '1990-03-05',
                    ...props,
                    formParams: _.merge({}, defaultFormParams, formParams),
                }
            }
        />
    );
};

export default OrderDate;