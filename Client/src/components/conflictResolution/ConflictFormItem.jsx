import { Button, Flex, Radio, Tag, theme, Typography } from 'antd'
import React, { useEffect, useState } from 'react'
const { Text, Title } = Typography;

const ConflictFormItem =({   identifier,
                             config,
                             currentValue,
                             newValue,
                             value: valueFromForm,
                             onChange: onChangeFormItem, }) =>{
    const { token } = theme.useToken();
    const {type, params, formParams, name} = config;
    const ItemComponent = type;
    const fieldDisplayName = name || identifier;
    const [currentSolve, setCurrentSolve] = useState('');
    const [isResultApproved, setIsResultApproved] = useState(false);

/*  useEffect(() => {
        // Если значение в форме уже есть (например, из initialValues или предыдущего выбора),
        // и оно совпадает с одним из вариантов конфликта, считаем конфликт "разрешенным" в UI.
        // Это важно, чтобы при повторном рендере с уже установленным значением не показывать снова выбор.
        if (valueFromForm === currentValue || valueFromForm === newValue) {
            setCurrentSolve(valueFromForm);
            setIsResultApproved(true);
        } else {
            // Если значение из формы не соответствует ни одному из вариантов,
            // сбрасываем в состояние выбора (по умолчанию currentValue)
            setCurrentSolve(currentValue);
            setIsResultApproved(false);
        }
    }, [valueFromForm, currentValue, newValue]);*/

    const handleCancelOrEdit = () => {
        setIsResultApproved(false);
    };
    const handleApprove = () => {
        if (onChangeFormItem) {
            onChangeFormItem(currentSolve); // Передаем выбранное значение в Form.Item
        }
        setIsResultApproved(true);
    };
    const handleRadioChange = (e) => {
        setCurrentSolve(e.target.value);
    };


    if (isResultApproved) {
        console.log(identifier)
        return (
            <Flex align="center" style={{ width: '100%' }}>
                <div style={{ flexGrow: 1 }}>
                    <ItemComponent
                        value={valueFromForm} // ItemComponent получит value от Form.Item автоматически
                        onChange={onChangeFormItem} // ItemComponent получит onChange от Form.Item автоматически
                        key={identifier}
                        params={params}
                        formParams={{identifier, name: fieldDisplayName, ...formParams }}
                        mode='editableInfo'
                    />
                </div>
                <Button onClick={handleCancelOrEdit} type="link" style={{ marginLeft: 8 }}>
                    Изменить выбор
                </Button>
            </Flex>
        );
    }

    return (
        <>
                <>
                <Radio.Group style={{ width: '100%' }} onChange={handleRadioChange}>
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
                    <Flex>
                        <Button type='primary' onClick={handleApprove}>Применить</Button>
                        <Button>Отмена</Button>
                    </Flex>
                </>
        </>
    );
}

export default ConflictFormItem;