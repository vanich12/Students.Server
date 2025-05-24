import React, { useEffect, useState } from 'react'
import { Form, Button, Space, Flex } from 'antd'

const Conflict = ({ Component, props }) => {
    const { value, formParams, setValue, setMode } = props;
    console.log(props)
    const { key, name, rules, normalize, hasFeedback } = formParams;

    const [currentEditValue, setCurrentEditValue] = useState(value);
    useEffect(() => {
        console.log("Обертка конфликтного поля");
        console.log(value);
        setCurrentEditValue(value);
    },[value])
    const handleValueChange = (componentOnChangeValue) => {
        let newValue;
        if (componentOnChangeValue && typeof componentOnChangeValue === 'object' && componentOnChangeValue.target !== undefined) {
            newValue = componentOnChangeValue.target.value;
        } else {
            newValue = componentOnChangeValue;
        }
        setCurrentEditValue(newValue);
    };
    const handleSave = () => {
        console.log(currentEditValue);
        setMode('conflictInfo');
    };

    const handleCancel = () => {
        setMode('conflictInfo');
    };

    return (
        <Space>
                <Component
                    {...{
                        ...props, defaultValue: value,
                        value: currentEditValue,
                    }}
                />
            <>
                <Space>
                    <Button type="primary" onClick={handleSave}>
                        Сохранить
                    </Button>
                    <Button htmlType="button" onClick={handleCancel}>
                        Отмена
                    </Button>
                </Space>
            </>
        </Space>
    );
};

export default Conflict;