import React, { useEffect, useState } from 'react'
import { Form, Radio, Tag, Typography, Button, Divider, theme, Flex } from "antd"
import { Loading } from '../shared/layout'
import ConflictFormItem from './ConflictFormItem'
import { useEditOneByPRequest } from '../../storage/crud/personsCrud'
import useNotifications from '../../notifications/useNotifications'
import { useNavigate } from 'react-router-dom'
const { Text, Title } = Typography;

// ToDo: нужно будет сделать единый конфиг, по которому будет происходить сравнение, чтобы из конфигов не брались лишние поля
const ConflictResolutionForm = ({ datasId, configs }) => {

    const [form] = Form.useForm();
    const { token } = theme.useToken();
    const { showSuccess, showError } = useNotifications();
    const navigate = useNavigate();
    const { personId, pendingRequestId } = datasId
    const {personConfig,pendingRequestConfig } = configs


    const { crud:personCrud, properties: personProperties} = personConfig
    const { crud: pRequestCrud, properties: pRequestProperties} = pendingRequestConfig
    const { useGetOneByIdAsync:useGetOnePersonByIdAsync,useEditOneByPRequest:useEditPersonByPRequest} = personCrud;
    const { useGetOneByIdAsync:useGetOnePRequestByIdAsync} = pRequestCrud

    const [resolvedFields, setResolvedFields] = useState({})
    // isLoading = true только если в кеше RTK Query отсутствуют данные об обьекте и запроса еще не было, а если
    const { data: personData, isLoading: personIsLoading, isFetching:personIsFetching, refetch: personRefetch } = useGetOnePersonByIdAsync(personId);
    const [editRequest, editingResult] = useEditPersonByPRequest();
    const { data: pRequestData, isLoading: pRequestIsLoading, isFetching:pRequestIsFetching, refetch: pRequestRefetch } = useGetOnePRequestByIdAsync(pendingRequestId);
    const isLoading = personIsLoading || pRequestIsLoading || !personData || !pRequestData;

    const onSubmit =  async (formValues) => {
        console.log("значения в форме", formValues);
        console.log({ pendingRequestId, personId, formValues });
        await editRequest({ pendingRequestId, personId, formValues });
        await new Promise(resolve => setTimeout(resolve, 1000));
        navigate(-1);
    };

    useEffect(() => {
        if (!personIsLoading &&
            !pRequestIsLoading &&
            !pRequestIsFetching &&
            !personIsFetching) {
            const initialValues = {};
            const newPersonData = { ...personData };
            console.log("данные персоны:")
            console.log(newPersonData);
            delete newPersonData.id;
            Object.keys(personProperties).forEach(key => {
                if (newPersonData.hasOwnProperty(key)) {
                   /* initialValues[key] = personData[key];*/
                }
            });
            const newPRequestData = { ...pRequestData };
            console.log("Данные заявки:")
            console.log(newPRequestData);
            Object.keys(pRequestProperties).forEach(key => {
                if (newPRequestData.hasOwnProperty(key) && !newPRequestData.hasOwnProperty(key)) {
                    initialValues[key] = pRequestData[key];
                }
            });
            form.setFieldsValue(initialValues);
            setResolvedFields(initialValues);
        }
    }, [personIsLoading,
        personIsFetching,
        pRequestIsFetching, personData, pRequestData]);
    const allPropertyKeys = new Set([...Object.keys(personProperties), ...Object.keys(pRequestProperties)]);

    return (isLoading) ? (<Loading/>):(
        <Form
            layout="horizontal"
            form={form}
            name="conflict_resolution_form"
            onFinish={(values) => onSubmit(values)}
            style={{

            }
        }
            scrollToFirstError
        >
            <Title level={4}>Разрешение данных</Title>
                <Text>Выберите, какие значения использовать для обновления информации о персоне на основе данных из заявки.</Text>
            <Divider style={{background:token.colorPrimary}} />

            {Array.from(allPropertyKeys).map((fieldName) => {
                const personPropConfig = personProperties[fieldName];
                const pRequestPropConfig = pRequestProperties[fieldName];

                const currentValue = personData.hasOwnProperty(fieldName) ? personData[fieldName] : undefined;
                const newValue = pRequestData.hasOwnProperty(fieldName) ? pRequestData[fieldName] : undefined;

                const hasPersonValue = personData.hasOwnProperty(fieldName);
                const hasPRequestValue = pRequestData.hasOwnProperty(fieldName);
                const fieldDisplayName = personPropConfig?.name || pRequestPropConfig?.name || fieldName;
                // Если поле есть в обоих, но значения совпадают или нет значения в заявке
                if (!hasPRequestValue || currentValue === newValue|| newValue === null) {
                    if (personPropConfig) {

                        const ItemComponent = personPropConfig.type
                        return (
                            <Form.Item
                                key={fieldName}
                                label={<>{fieldDisplayName} <Tag color="default">Без изменений</Tag></>}
                                name={fieldName}
                                initialValue={currentValue}
                                style={{margin:'0 auto',
                                width:'70%'
                                }}
                            >

                                 <ItemComponent
                                        key={fieldName}
                                        name={fieldName}
                                        params={personPropConfig.params}
                                        formParams={{ fieldName, name: fieldDisplayName, ...personPropConfig.formParams }}
                                        mode='conflictInfo'
                                    />
                            </Form.Item>
                        );
                    }
                    // Если нет конфига для personProperties, просто выводим текст
                    return (
                        <Form.Item key={fieldName} label={<>{fieldDisplayName} <Tag>Без изменений</Tag></>} name={fieldName} initialValue={currentValue}>
                            <Text>{String(currentValue === null || currentValue === undefined ? ' (пусто) ' : currentValue)}</Text>
                        </Form.Item>
                    );
                }
                // если поле есть в конфигурации персоны
                else if (hasPersonValue) {
                    // КОНФЛИКТ: поле есть в обоих и значения разные
                    return (
                        <Form.Item
                            key={fieldName}
                            label={
                                <div
                                    style={{display: 'flex', alignItems: 'center', fontWeight: token.fontWeightStrong}}>
                                    {fieldDisplayName}
                                    <Tag color="warning" style={{marginLeft: '8px'}}>
                                        Конфликт
                                    </Tag>
                                </div>
                            }
                            name={fieldName}
                            labelCol={{span: 6}}
                            wrapperCol={{span: 18}}
                            style={{
                                backgroundColor: token.colorWarningBg,
                                border: `1px solid ${token.colorWarningBorder}`,
                                borderLeft: `3px solid ${token.colorWarning}`,
                                padding: '16px',
                                borderRadius: token.borderRadiusLG,
                                marginBottom: '24px',
                                boxShadow: token.boxShadowSecondary,
                            }}
                            rules={[{required: true, message: 'Пожалуйста, сделайте выбор!'}]}
                        >
                            {personPropConfig && (<ConflictFormItem
                                config={personPropConfig}
                                currentValue={currentValue}
                                newValue={newValue}
                                identifier={fieldName}
                            />)}
                        </Form.Item>
                    );
                }
            })}
            <Divider />
            <Form.Item>
                <Button type="primary" htmlType="submit" >
                    Применить и сохранить
                </Button>
            </Form.Item>
        </Form>

    );
};

export default ConflictResolutionForm;
