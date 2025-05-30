import React from 'react';
import { Modal, Form } from "antd";

const StandartForm = ({ control, properties, crud, mutationHook, hookArgs, title}) => {
    const { useAddOneAsync } = crud;
    const { showAddOneForm, setShowAddOneForm } = control;
    const [ addOne, { error, isLoading } ] = useAddOneAsync();
    const [form] = Form.useForm();

    const currentHook = (hook == null || undefined) ? addOne : hook;
    const onSubmit = (formValues) => {
        hookArgs = {id: hookArgs.id, formValues};
        console.log(formValues);
        currentHook(formValues);
        setShowAddOneForm(false);
        form.resetFields();
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

export default StandartForm;