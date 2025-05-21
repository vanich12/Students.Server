import React, { useEffect } from 'react'
import personConfig from '../../storage/catalogConfigs/personRequests.js';
import pendingRequestConfig from '../../storage/catalogConfigs/pendingRequests';
import { Modal, Form, Radio, Tag, Typography, Spin, Alert, Button, Divider } from "antd";
const { Text, Title } = Typography;

const AddOneForm = ({ control, datasId }) => {
    const [form] = Form.useForm();
    const {personId, pRequestId} = datasId
    const {crud:personCrud, properties: personProperties} = personConfig
    const { crud: pRequestCrud, properties: pRequestProperties} = pendingRequestConfig
    const { useGetOneByIdAsync:useGetOnePersonByIdAsync} = personCrud;
    const { useGetOneByIdAsync:useGetOnePRequestByIdAsync} = pRequestCrud

    const [resolvedFields, setresolvedFields] = useState({})

    const { data: personData, isLoading: personIsLoading, isFetching:personIsFetching, refetch: personRefetch } = useGetOnePersonByIdAsync(personId);
    const { data: pRequestData, isLoading: pRequestIsLoading, isFetching:pRequestIsFetching, refetch: pRequestRefetch } = useGetOnePRequestByIdAsync(pRequestId);
    useEffect(() => {
        if (!personIsLoading &&
            !pRequestIsLoading &&
            !pRequestIsFetching &&
            !personIsFetching) {
            const initialValues = {};
            const newPersonData = { ...personData };
            delete newPersonData.id;
            Object.keys(personProperties).forEach(key => {
                if (newPersonData.hasOwnProperty(key)) {
                    initialValues[key] = personData[key];
                }
            });
            const newPRequestData = { ...pRequestData };
            // Добавляем поля, которые есть только в pRequestData
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
    // Собираем все ключи из обоих объектов свойств, чтобы не пропустить уникальные
    const allPropertyKeys = new Set([...Object.keys(personProperties), ...Object.keys(pRequestProperties)]);

    return (
        <Form
            layout="vertical" // vertical обычно лучше для такого UI
            form={form}
            name="conflict_resolution_form"

            scrollToFirstError
        >
            <Title level={4}>Разрешение данных</Title>
          Выберите, какие значения использовать для обновления информации о персоне на основе данных из заявки.
            <Divider />

            {Array.from(allPropertyKeys).map((key) => {
                const personPropConfig = personProperties[key];
                const pRequestPropConfig = pRequestProperties[key]; // Предполагаем, что структура конфига похожа

                const currentValue = personData.hasOwnProperty(key) ? personData[key] : undefined;
                const newValue = pRequestData.hasOwnProperty(key) ? pRequestData[key] : undefined;

                const hasPersonValue = personData.hasOwnProperty(key);
                const hasPRequestValue = pRequestData.hasOwnProperty(key);

                // Определяем имя поля для отображения
                // Приоритет у personProperties, если нет, то у pRequestProperties
                const fieldDisplayName = personPropConfig?.name || pRequestPropConfig?.name || key;

           /*     // Если поле есть только в данных заявки (новое поле для персоны)
                if (hasPRequestValue && !hasPersonValue) {
                    return (
                        <Form.Item
                            key={key}
                            label={<>{fieldDisplayName} <Tag color="cyan">Новое из заявки</Tag></>}
                            name={key}
                            initialValue={newValue} // Новое значение будет установлено
                        >
                            {/!* Можно отобразить как текст или использовать ваш компонент Item, если нужно редактирование *!/}
                           {String(newValue === null || newValue === undefined ? ' (пусто) ' : newValue)}
                            {/!* Или если ваш Item может работать в режиме "только чтение" или вы хотите дать возможность редактировать *!/}
                            {/!* <ItemComponent key={key} ... initialValue={newValue} ... /> *!/}
                        </Form.Item>
                    );
                }*/

         /*       // Если поле есть только в данных персоны (нет в заявке, оставляем как есть)
                if (hasPersonValue && !hasPRequestValue) {
                    return (
                        <Form.Item
                            key={key}
                            label={<>{fieldDisplayName} <Tag>Только у персоны</Tag></>}
                            name={key}
                            initialValue={currentValue}
                        >
                            {String(currentValue === null || currentValue === undefined ? ' (пусто) ' : currentValue)}
                        </Form.Item>
                    );
                }*/

                // Если поле есть в обоих, но значения совпадают или нет значения в заявке
                if (!hasPRequestValue || currentValue === newValue) {
                    // Используем ваш кастомный компонент Item для отображения/редактирования
                    // если он предусмотрен в personProperties
                    if (personPropConfig) {
                        const ItemComponent = personPropConfig.type; // Ваш тип компонента из конфига
                        return (
                            <Form.Item
                                key={key}
                                label={<>{fieldDisplayName} <Tag color="default">Без изменений</Tag></>}
                                name={key}
                                initialValue={currentValue}
                            >
                                {/* Здесь вы можете решить, показывать просто текст или ваш кастомный компонент */}
                                {/* Если это поле не должно меняться через этот интерфейс, то просто текст */}
                                {String(currentValue === null || currentValue === undefined ? ' (пусто) ' : currentValue)}

                                {/* Если вы хотите использовать ваш динамический компонент: */}
                                {/* <ItemComponent
                                        key={key} // Убедитесь, что это уникально
                                        params={personPropConfig.params}
                                        formParams={{ key, name: fieldDisplayName, ...personPropConfig.formParams }}
                                        mode='form' // или другой режим, если есть
                                        // Важно: как ItemComponent будет получать и устанавливать значение в Form
                                        // Стандартные Form.Item компоненты AntD делают это автоматически
                                        // Для кастомных, вам нужно убедиться, что они совместимы с Form (принимают value, onChange)
                                        // или использовать form.setFieldsValue внутри их onChange
                                    /> */}
                            </Form.Item>
                        );
                    }
                    // Если нет конфига для personProperties, просто выводим текст
                    return (
                        <Form.Item key={key} label={<>{fieldDisplayName} <Tag>Без изменений</Tag></>} name={key} initialValue={currentValue}>
                            {String(currentValue === null || currentValue === undefined ? ' (пусто) ' : currentValue)}
                        </Form.Item>
                    );
                }

                // КОНФЛИКТ: поле есть в обоих и значения разные
                // Отображаем Radio.Group для выбора
                return (
                    <Form.Item
                        key={key}
                        label={<>{ } <Tag color="orange">Конфликт</Tag></>}
                        name={key} // Имя поля в форме, будет хранить выбранное значение
                        initialValue={currentValue} // По умолчанию выбираем текущее значение персоны
                        rules={[{ required: true, message: 'Пожалуйста, сделайте выбор!' }]}
                    >
                        <Radio.Group style={{ width: '100%' }}>
                            <Radio value={currentValue} style={{ display: 'block', height: 'auto', whiteSpace: 'normal', marginBottom: 8, padding: 8, border: '1px solid #d9d9d9', borderRadius: 4 }}>
                                <Text strong>Текущее у персоны:</Text><br/>
                                <Text style={{ wordBreak: 'break-all' }}>
                                    {String(currentValue === null || currentValue === undefined ? ' (пусто) ' : currentValue)}
                                </Text>
                            </Radio>
                            <Radio value={newValue} style={{ display: 'block', height: 'auto', whiteSpace: 'normal', padding: 8, border: '1px solid #d9d9d9', borderRadius: 4 }}>
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

export default AddOneForm;