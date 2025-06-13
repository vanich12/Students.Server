import React from 'react';

import BaseComponent from './baseComponents/BaseComponent.jsx';
import { AutoComplete } from 'antd';



const rules = [
    {
        required: true,
        message: 'Необходимо заполнить email',
    },
    {
        type: 'email',
        message: 'Некорректно заполнен email',
    },
];

const formParams = {
    key: 'email',
    name: 'E-mail',
    rules,
};

const StartEndDate = (props) => (
    <BaseComponent
        {
            ...{ 
                ...props,
                formParams,
            }
        }
    />
);

export default StartEndDate;