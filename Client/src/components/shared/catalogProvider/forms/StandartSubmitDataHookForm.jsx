import React from 'react';
import { Modal, Form } from "antd";
// что-то типо формы под разные хуки мутаций
const StandartSubmitForm = ({ control, properties, crud, mutationHook, initialHookArgs, initialValues,formDataKey,title}) => {
    const { useAddOneAsync } = crud;
    const { showAddOneForm, setShowAddOneForm } = control;
    const [form] = Form.useForm();

    const activeMutationHook = mutationHook || crud?.useAddOneAsync;
    const [triggerMutation, { error: mutationError, isLoading, isSuccess, reset }] = activeMutationHook();
    const onSubmit = async (formValues) => {
        let payload;
        if (formDataKey) {
            payload = {
                ...(initialHookArgs || {}),
                [formDataKey]: formValues
            };
        } else {
            payload = { ...(initialHookArgs || {}), ...formValues };
        }

        console.log("Submitting with :", payload);
        try {
            await triggerMutation(payload).unwrap();
            setShowAddOneForm(false);
        } catch (err) {
            console.error("Mutation failed:", err);
        }
    };

    return (
        <Modal
            title={title}
            open={showAddOneForm}
            okText="Добавить"
            cancelText="Отмена"
            okButtonProps={{
                autoFocus: true,
                htmlType: 'submit',
            }}
            onCancel={() => setShowAddOneForm(false)}
            modalRender={(dom) => (
                <Form
                    layout="horizontal"
                    form={form}
                    name="form_in_modal"
                    scrollToFirstError
                    initialValues={initialValues}
                    onFinish={(values) => onSubmit(values)}
                >
                    {dom}
                </Form>
            )}
        >
            {Object.entries(properties).map(([key, { name, type, formParams, params }]) => {
                const Item = type;

                return (
                    <Item
                        key={key}
                        params={params}
                        formParams={{ key, name, ...formParams }}
                        mode='form'
                        setValue={(value) => {

                            form.setFieldsValue({
                                [key]: value,
                            });
                        }}
                    />
                );
            })}
        </Modal>
    );
};

export default StandartSubmitForm;