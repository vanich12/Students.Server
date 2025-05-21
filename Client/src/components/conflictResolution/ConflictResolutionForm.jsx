import React, { useEffect, useState } from 'react'
import { Modal, Form, Radio, Tag, Typography, Spin, Alert, Button, Divider, theme } from "antd"
import { Loading } from '../shared/layout'
const { Text, Title } = Typography;

const ConflictResolutionForm = ({ datasId, configs }) => {

    const [form] = Form.useForm();
    const { personId, pendingRequestId } = datasId
    const {personConfig,pendingRequestConfig } = configs
    const { token } = theme.useToken();
    const { crud:personCrud, properties: personProperties} = personConfig
    const { crud: pRequestCrud, properties: pRequestProperties} = pendingRequestConfig
    const { useGetOneByIdAsync:useGetOnePersonByIdAsync} = personCrud;
    const { useGetOneByIdAsync:useGetOnePRequestByIdAsync} = pRequestCrud

    const [resolvedFields, setresolvedFields] = useState({})

    const { data: personData, isLoading: personIsLoading, isFetching:personIsFetching, refetch: personRefetch } = useGetOnePersonByIdAsync(personId);
    const { data: pRequestData, isLoading: pRequestIsLoading, isFetching:pRequestIsFetching, refetch: pRequestRefetch } = useGetOnePRequestByIdAsync(pendingRequestId);
    useEffect(() => {
        if (!personIsLoading &&
            !pRequestIsLoading &&
            !pRequestIsFetching &&
            !personIsFetching) {
            const initialValues = {};
            const newPersonData = { ...personData };
            console.log(newPersonData);
            delete newPersonData.id;
            Object.keys(personProperties).forEach(key => {
                if (newPersonData.hasOwnProperty(key)) {
                    initialValues[key] = personData[key];
                }
            });
            const newPRequestData = { ...pRequestData };
            console.log(newPRequestData);
            Object.keys(pRequestProperties).forEach(key => {
                if (newPRequestData.hasOwnProperty(key) && !newPRequestData.hasOwnProperty(key)) {
                    initialValues[key] = pRequestData[key];
                }
            });
            form.setFieldsValue(initialValues);
        }

    }, [personIsLoading,
        personIsLoading,
        personIsFetching,
        pRequestIsFetching])
    const allPropertyKeys = new Set([...Object.keys(personProperties), ...Object.keys(pRequestProperties)]);

    return (personIsLoading && pRequestIsLoading && pRequestIsFetching && personIsFetching) ? (<Loading/>):(
        <Form
            layout="horizontal"
            form={form}
            name="conflict_resolution_form"
            style={{

            }}
            scrollToFirstError
        >
            <Title level={4}>Разрешение данных</Title>
                <Text>Выберите, какие значения использовать для обновления информации о персоне на основе данных из заявки.</Text>
            <Divider style={{background:token.colorPrimary}} />

            {Array.from(allPropertyKeys).map((key) => {
                const personPropConfig = personProperties[key];
                const pRequestPropConfig = pRequestProperties[key];

                const currentValue = personData.hasOwnProperty(key) ? personData[key] : undefined;
                const newValue = pRequestData.hasOwnProperty(key) ? pRequestData[key] : undefined;

                const hasPersonValue = personData.hasOwnProperty(key);
                const hasPRequestValue = pRequestData.hasOwnProperty(key);

                const fieldDisplayName = personPropConfig?.name || pRequestPropConfig?.name || key;

                // Если поле есть в обоих, но значения совпадают или нет значения в заявке
                if (!hasPRequestValue || currentValue === newValue) {
                    if (personPropConfig) {
                        const ItemComponent = personPropConfig.type
                        return (
                            <Form.Item
                                key={key}
                                label={<>{fieldDisplayName} <Tag color="default">Без изменений</Tag></>}
                                name={key}
                                initialValue={currentValue}
                                style={{margin:'0 auto',
                                width:'70%'
                                }}
                            >
                                 <ItemComponent
                                        key={key}
                                        params={personPropConfig.params}
                                        formParams={{ key, name: fieldDisplayName, ...personPropConfig.formParams }}
                                        mode='form'
                                    />
                            </Form.Item>
                        );
                    }
                    // Если нет конфига для personProperties, просто выводим текст
                    return (
                        <Form.Item key={key} label={<>{fieldDisplayName} <Tag>Без изменений</Tag></>} name={key} initialValue={currentValue}>
                            <Text>{String(currentValue === null || currentValue === undefined ? ' (пусто) ' : currentValue)}</Text>
                        </Form.Item>
                    );
                }

                // КОНФЛИКТ: поле есть в обоих и значения разные
                return (
                    <Form.Item
                        key={key} // Убедитесь, что key - это строка, а не объект
                        label={
                            <div style={{ display: 'flex', alignItems: 'center', fontWeight: token.fontWeightStrong }}> {/* Сделаем лейбл чуть жирнее */}
                                {fieldDisplayName}
                                <Tag color="warning" style={{ marginLeft: '8px' }}> {/* Используем стандартный цвет "warning" для тега */}
                                    Конфликт
                                </Tag>
                            </div>
                        }
                        name={key}
                        initialValue={currentValue}
                        labelCol={{ span: 6 }}  // Или подберите соотношение, например, { flex: '0 0 200px' } для фиксированной ширины лейбла
                        wrapperCol={{ span: 18 }} // Или { flex: '1 1 auto' }
                        style={{
                            backgroundColor: token.colorWarningBg,      // Фон предупреждения (очень светлый желто-оранжевый)
                            border: `1px solid ${token.colorWarningBorder}`, // Рамка предупреждения (чуть темнее фона)
                            // Или можно использовать более тонкую/менее заметную рамку:
                            // border: `1px solid ${token.colorBorderSecondary}`,
                             borderLeft: `3px solid ${token.colorWarning}`,
                            padding: '16px',                            // Внутренние отступы
                            borderRadius: token.borderRadiusLG,         // Скругление углов
                            marginBottom: '24px',                       // Отступ снизу для разделения блоков
                             boxShadow: token.boxShadowSecondary,     // Опционально: легкая тень для "приподнятости"
                        }}
                        rules={[{ required: true, message: 'Пожалуйста, сделайте выбор!' }]}
                    >
                        <Radio.Group style={{ width: '100%' }}>
                            <Radio value={currentValue} style={{ display: 'block', height: 'auto', whiteSpace: 'normal', marginBottom: 8, padding: 8, border: `1px solid ${token.colorBorder}`, borderRadius: 4 }}>
                                <Text strong>Текущее у персоны:</Text><br/>
                                <Text style={{ wordBreak: 'break-all' }}>
                                    {String(currentValue === null || currentValue === undefined ? ' (пусто) ' : currentValue)}
                                </Text>
                            </Radio>
                            <Radio value={newValue} style={{ display: 'block', height: 'auto', whiteSpace: 'normal', padding: 8, border: `1px solid ${token.colorBorder}`, borderRadius: 4 }}>
                                <Text strong>Из заявки:</Text><br/>
                                <Text style={{ wordBreak: 'break-all' }}>
                                    {String(newValue === null || newValue === undefined ? ' (пусто) ' : newValue)}
                                </Text>
                            </Radio>
                        </Radio.Group>
                    </Form.Item>
                );
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