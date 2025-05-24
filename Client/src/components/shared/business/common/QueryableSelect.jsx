import React, { useState, useEffect } from 'react';
import _ from 'lodash';
import BaseComponent from '../baseComponents/BaseComponent.jsx';
import { Typography, Select, Radio, theme } from 'antd'

const { Text } = Typography;

const DefaultInfoComponent = ({ dataById, formParams }) => {
    const { labelKey } = formParams;
    return (
        <Text>{dataById?.[labelKey]}</Text>
    );
};



const DefaultEditFormComponent = ({ onChange, placeholder, formParams, dataById, allData }) => {
    const { key, labelKey } = formParams;
    return (
        <Select
            key={key}
            showSearch
            defaultValue={dataById?.[labelKey]}
            style={{ minWidth: '200px' }}
            placeholder={placeholder}
            filterOption={(input, option) =>
                (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
            onChange={onChange}
            options={(allData || []).map((d) => ({
                value: d.id,
                label: d[labelKey],
            }))}
        />
    );
};

// UI - представления в зависимости от режима отображения
const components = {
    info: DefaultInfoComponent,
    editableInfo: DefaultInfoComponent,
    form: DefaultEditFormComponent,
    edit: DefaultEditFormComponent,
    filter: DefaultEditFormComponent,
    conflict: DefaultEditFormComponent,
    conflictInfo:DefaultInfoComponent
};

const rules = [
    {
        required: true,
        message: 'Необходимо выбрать значение',
    },
];

const defaultFormParams = {
    key: 'defaultKey!',
    labelKey: 'name',
    name: 'Какой-то список',
    rules,
};

const QueryableSelect = ({ formParams, ...props }) => (
    <BaseComponent
        {
            ...{
                components,
                placeholder: 'Выберите значение',
                ...props,
                formParams: _.merge({}, defaultFormParams, formParams
                ),
            }
        }
    />
);

export default QueryableSelect;