import React from 'react';
import BaseComponent from './baseComponents/BaseComponent.jsx';
import { Typography } from 'antd';

const { Text } = Typography;

const DefaultInfoComponent = ({ value }) => {

    return (
        <Text>{value}</Text>
    );
};

const components = {
    info: DefaultInfoComponent,
    editableInfo: DefaultInfoComponent,
    form: DefaultInfoComponent,
    filter: DefaultInfoComponent,
    edit: DefaultInfoComponent,
    modal: DefaultInfoComponent,
    conflictInfo:DefaultInfoComponent,
    conflict: DefaultInfoComponent,
};

const formParams = {
    rules: [
        {
            required: false,
        },
    ],
};

const Age = (props) => (
    <BaseComponent
        {
            ...{ 
                ...props,
                mode: 'info',
                components,
                formParams,
            }
        }
    />
);

export default Age;