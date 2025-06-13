// SubmitForm.js - ИСПРАВЛЕННЫЙ И РЕКОМЕНДУЕМЫЙ ВАРИАНТ

import React, { useEffect } from 'react';
import { Modal, Form, Input } from "antd"

const SubmitForm = ({ control, properties, data = {}, onSubmit, title }) => {
    const [form] = Form.useForm();
    const { showAddOneForm, setShowAddOneForm } = control;

    // Хук для установки значений при открытии окна
    useEffect(() => {
        if (showAddOneForm) {
            const preparedData = {};
            for (const key in data) {

                if (data[key] && typeof data[key] === 'object' && !Array.isArray(data[key]) && !(data[key] instanceof Date)) {
                    preparedData[key] = data[key].id; // Предполагаем, что нам нужен ID
                } else {
                    preparedData[key] = data[key];
                }
            }
            form.setFieldsValue(preparedData);
        }
    }, [showAddOneForm, data, form]);


    const handleOk = () => {
        form.submit();
    };

    return (
        <Modal
            title={title}
            open={showAddOneForm}
            okText="Сохранить"
            cancelText="Отмена"
            width = {800}
            onOk={handleOk}
            onCancel={() => setShowAddOneForm(false)}
            destroyOnClose
        >

            <Form
                form={form}
                layout="vertical"
                name="form_in_modal"
                onFinish={onSubmit}
                autoComplete="off"
            >
                {Object.entries(properties).map(([key, { name, type, formParams, params }]) => {
                    const Item = type;
                    return (
                        <Form.Item
                            key={key}
                            name={key} // <-- Связывает поле с данными из формы
                            label={name}
                            {...formParams}
                        >
                            {/*
                              УБИРАЕМ ВСЕ ЛИШНИЕ ПРОПСЫ!
                              Form.Item сам передаст value и onChange в ItemComponent.
                              Вашему <ItemComponent> НЕ НУЖНЫ пропсы value, name, setValue.
                              Он должен быть стандартным "контролируемым компонентом".
                            */}
                            <Input />
                        </Form.Item>
                    );
                })}
            </Form>
        </Modal>
    );
};

export default SubmitForm;