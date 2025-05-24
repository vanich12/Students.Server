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
// ToDo: надо сделать Debounce
 useEffect(() => {
        setCurrentSolve(valueFromForm)
    }, [valueFromForm]);

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
        console.log(e.target.value)
        setCurrentSolve(e.target.value);
    };


    if (isResultApproved) {
        return (
            <Flex align="center" style={{ width: '100%' }}>
                <div style={{ flexGrow: 1 }}>
                    <ItemComponent
                        value={valueFromForm} // ItemComponent получит value от Form.Item автоматически
                        onChange={onChangeFormItem} // ItemComponent получит onChange от Form.Item автоматически
                        key={identifier}
                        name = {identifier}
                        params={params}
                        formParams={{identifier, name: fieldDisplayName, ...formParams }}
                        mode='conflictInfo'
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
         {/*       <>
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
                </>*/}
            <ItemComponent
                currentValue = {currentValue}
                value={currentValue}
                newValue = {newValue}
                onChange={onChangeFormItem} // ItemComponent получит onChange от Form.Item автоматически
                handleRadioChange = {handleRadioChange}
                key={identifier}
                name = {identifier}
                params={params}
                formParams={{identifier, name: fieldDisplayName, ...formParams }}
                mode='conflictResolve'
            />
            <Flex>
                <Button type='primary' onClick={handleApprove}>Применить</Button>
                <Button>Отмена</Button>
            </Flex>
        </>
    );
}

export default ConflictFormItem;