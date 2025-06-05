import React, { useState, useEffect } from 'react';
import _ from 'lodash';
import BaseComponent from '../baseComponents/BaseComponent.jsx';
import { Typography, Select, Radio, theme, Flex, Button } from 'antd'

const { Text } = Typography;

const DefaultInfoComponent = ({ dataById, formParams }) => {
    const { labelKey } = formParams;
    return (
        <Text>{dataById?.[labelKey]}</Text>
    );
};


const DefaultConflictResolveForm = ({currentValue, newValue,handleRadioChange, dataById, formParams, crud})=>
{
    const { useGetOneByIdAsync } = crud;
    const { data: newDataById } = useGetOneByIdAsync(newValue);
    const { labelKey } = formParams;
    const { token } = theme.useToken();

    return (
    <>
        <Radio.Group style={{ width: '100%' }} onChange={handleRadioChange}>
            {/*значение Radio - кнопки - это id элемента, а выводится дата по Id*/}
            <Radio value={currentValue} style={{ display: 'block', height: 'auto', whiteSpace: 'normal', marginBottom: 8, padding: 8, border: `1px solid ${token.colorBorder}`, borderRadius: 4 }}>
                <Text strong>Текущее у персоны:</Text><br/>
                <Text style={{ wordBreak: 'break-all' }}>
                    {String(currentValue === null || currentValue === undefined ? ' (пусто) ' : dataById?.[labelKey])}
                </Text>
            </Radio>
            <Radio  value={newValue} style={{ display: 'block', height: 'auto', whiteSpace: 'normal', padding: 8, border: `1px solid ${token.colorBorder}`, borderRadius: 4 }}>
                <Text strong>Из заявки:</Text><br/>
                <Text style={{ wordBreak: 'break-all' }}>
                    {String(newValue === null || newValue === undefined ? ' (пусто) ' : newDataById?.[labelKey])}
                </Text>
            </Radio>
        </Radio.Group>
    </>)
}

const DefaultEditFormComponent = ({ onChange, placeholder, formParams, dataById, allData, options }) => {
    const { key, labelKey,optionRender } = formParams;
    const currentOptions = options ?? (allData || []).map((d) => ({
        value: d.id,
        label: d[labelKey],
    }));
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
            options={currentOptions}
            optionRender={optionRender}
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
    conflictInfo:DefaultInfoComponent,
    conflictResolve: DefaultConflictResolveForm,
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