import React, { useState, useEffect, useCallback } from 'react';
import { Row, Col, Space } from 'antd';
import { getComponentFromRegistry } from '../../../storage/componentRegistry'

const rowStyle = { alignItems: 'center' };

const DetailsPageData = ({ items, data, editData, setIsChanged }) => {
    return (
        <Space direction="vertical" size={0} style={{ display: 'flex' }}>
            {Object.entries(items).map(([key, { name, type, formParams, params }]) => {
                const Item = getComponentFromRegistry(type);
                return (
                    <Row style={rowStyle} key={key}>
                        <Col span={3}>{name}</Col>
                        <Col span={8}>
                          {/*  во все элементы управления пропcы передаются (почти все) именно отсюда*/}
                            <Item
                                key={key}
                                name={key}
                                value={data[key]}
                                mode='editableInfo'
                                params={params}
                                formParams={{ key, name, ...formParams }}
                                setValue={(value) => {
                                    editData({
                                        ...data,
                                        [key]: value
                                    });
                                    setIsChanged(true);
                                }}
                            />
                        </Col>
                    </Row>
                );
            })}
        </Space>
    );
};

export default DetailsPageData;