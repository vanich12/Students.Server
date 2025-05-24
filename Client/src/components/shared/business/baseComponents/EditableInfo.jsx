import React from 'react';
import { Flex, Typography, Button, Space } from 'antd';
import { EditOutlined } from '@ant-design/icons';
const { Text } = Typography;

const ChangeSymbol = () => (<Text>* </Text>);
//Обертка, окружение, которое отображается в режиме
const EditableInfo = ({ Component,props }) => {
    const { changed, setMode } = props;
    console.log(props);
    return (
        <Space>
            {changed && (<ChangeSymbol />)}
            <Component {...props} />
            {//карандашик редактирвоания
            }
            <Button
                color="default"
                variant="link"
                icon={<EditOutlined />}
                onClick={() => setMode('edit')}
            />
        </Space>

    );
};

export default EditableInfo;