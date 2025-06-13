import { Button, Flex, Radio, Tag, theme, Typography } from 'antd'
import React, { useEffect, useState } from 'react'
import { CheckOutlined } from '@ant-design/icons'
import { CloseOutlined } from '@mui/icons-material'
import styled from 'styled-components'
import { getComponentFromRegistry } from '../../storage/componentRegistry'
const { Text, Title } = Typography;

const StyledApplyButton = styled(Button)`
    background-color: #006400;
    border-color: #006400;    
    color: white;              
    transition: background-color 0.3s ease, border-color 0.3s ease, color 0.3s ease;

    &:hover,
    &:focus { 
        background-color: white;             
        color: #006400;                    
        border-color: #006400;            
    }
`;

const StyledCancelButton = styled(Button)`
  background-color: ${props => props.token.colorError};
  border-color: ${props => props.token.colorError};
  transition: background-color 0.3s, border-color 0.3s;

  &:hover {
    background-color: ${props => props.token.colorErrorHover};
    border-color: ${props => props.token.colorErrorHover};
    color: white; 
  }
`;


const ConflictFormItem =({   identifier,
                             config,
                             currentValue,
                             newValue,
                             value: valueFromForm,
                             onChange: onChangeFormItem, }) =>{
    const { token } = theme.useToken();
    const {type, params, formParams, name} = config;

    const ItemComponent = getComponentFromRegistry(type);
    const fieldDisplayName = name || identifier;
    const [currentSolve, setCurrentSolve] = useState('');
    const [isResultApproved, setIsResultApproved] = useState(false);
// ToDo: надо сделать Debounce
/* useEffect(() => {
        console.log("обновление current Resolve")
        setCurrentSolve(valueFromForm)
    }, [valueFromForm]);*/

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
            <Flex  style={{marginTop: '10px'}} gap="small">
                <StyledApplyButton
                    icon={<CheckOutlined />}
                    onClick={handleApprove}
                    style={{
                        backgroundColor: '#006400',
                        borderColor: '#006400',
                        color: 'white',
                    }}
                >
                    Применить
                </StyledApplyButton>
                <StyledCancelButton danger icon={<CloseOutlined />} token = {token} onClick={handleApprove}>
                    Отмена
                </StyledCancelButton>
            </Flex>
        </>
    );
}

export default ConflictFormItem;