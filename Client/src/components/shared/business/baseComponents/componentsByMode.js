import React from 'react';
import { Input, Radio, theme, Typography } from 'antd'
const { Text } = Typography;

const DefaultInfoComponent = ({ value }) => (
    <Text>{value}</Text>
);

const DefaultEditableInfoComponent = ({ value }) => (
    <Text>{value}</Text>
);

const DefaultEditComponent = ({ value, onChange, defaultValue, formParams, placeholder }) => {
    const { key } = formParams;

    return (
        <Input
            key={key}
            allowClear
            value={value}
            onChange={onChange}
            defaultValue={defaultValue}
            placeholder={placeholder}
            type="textarea"
        />
    );
};

const DefaultConflictResolveForm = ({currentValue, newValue,handleRadioChange, formParams})=>
{
    const { labelKey } = formParams;
    const { token } = theme.useToken();

    return (
        <>
            <Radio.Group style={{ width: '100%' }} onChange={handleRadioChange}>
                {/*значение Radio - кнопки - это id элемента, а выводится дата по Id*/}
                <Radio value={currentValue} style={{ display: 'block', height: 'auto', whiteSpace: 'normal', marginBottom: 8, padding: 8, border: `1px solid ${token.colorBorder}`, borderRadius: 4 }}>
                    <Text strong>Текущее у персоны:</Text><br/>
                    <Text style={{ wordBreak: 'break-all' }}>
                        {String(currentValue === null || currentValue === undefined ? ' (пусто) ' : currentValue)}
                    </Text>
                </Radio>
                <Radio  value={newValue} style={{ display: 'block', height: 'auto', whiteSpace: 'normal', padding: 8, border: `1px solid ${token.colorBorder}`, borderRadius: 4 }}>
                    <Text strong>Из заявки:</Text><br/>
                    <Text style={{ wordBreak: 'break-all' }}>
                        {String(newValue === null || newValue === undefined ? ' (пусто) ' : newValue)}
                    </Text>
                </Radio>
            </Radio.Group>
        </>)
}

const DefaultFormComponent = ({ value, onChange, formParams, placeholder }) => {
    const { key } = formParams;

    return (
        <Input
            key={key}
            allowClear
            value={value}
            onChange={onChange}
            defaultValue=''
            placeholder={placeholder}
            type="textarea"
        />
    );
};

//  TODO:   доработать компонент
const DefaultFilterComponent = () => (
    <Text>В разработке</Text>
);

//  TODO:   доработать компонент
const DefaultModalComponent = () => (
    <Text>В разработке</Text>
);


const defaultComponentsByMode = {
    info: DefaultInfoComponent,
    editableInfo: DefaultEditableInfoComponent,
    form: DefaultFormComponent,
    filter: DefaultFilterComponent,
    edit: DefaultEditComponent,
    modal: DefaultModalComponent,
    conflictInfo:DefaultInfoComponent,
    conflict: DefaultEditComponent,
    conflictResolve: DefaultConflictResolveForm,
};

export default defaultComponentsByMode;