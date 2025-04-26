import React, { useState, useEffect, useCallback } from 'react';
import { Layout, Loading, DetailsPageData, RoutingWarningModal } from '../shared/layout/index.js';
import { useParams, useBlocker } from 'react-router-dom';
import { Row, Col, Space, Button } from 'antd';
import config from '../../storage/catalogConfigs/order.js';

//  TODO:   переделать эту рыба (с програм на приказы)
const OrdersDetailsPage = () => {
    const { id } = useParams();
    const [orderData, setOrderData] = useState({});
    const [isChanged, setIsChanged] = useState(false);
    const [isSaveInProgress, setIsSaveInProgress] = useState(false);
    const [initialData, setInitialData] = useState({}); 
    const { properties, crud } = config;
    const { useGetOneByIdAsync, useEditOneAsync } = crud;
    const { data, isLoading, isFetching } = useGetOneByIdAsync(id);

    const [editProgram] = useEditOneAsync();

    useEffect(() => {
        if (!isLoading && !isFetching) {
            const newData = { ...data };
            delete newData.id;
            setOrderData(newData);
            setInitialData(newData); 
        }
    }, [isLoading, isFetching, data]);

    let blocker = useBlocker(
        ({ currentLocation, nextLocation }) =>
            isChanged &&
            currentLocation.pathname !== nextLocation.pathname
    );

    const onSave = useCallback(() => {
        editProgram({ id, item: orderData });
        setIsChanged(false);
    }, [id, orderData]); 
    
    const onCancel = useCallback(() => {
        setOrderData(initialData);
        setIsChanged(false);
    }, [initialData]);
    
    return isLoading || isFetching
    ? (<Loading />)
    : (
        <Layout title="Данные приказа">
            <h2>{orderData?.name}</h2>
            <DetailsPageData
                items={properties}
                data={orderData}
                editData={setOrderData}
                setIsChanged={setIsChanged}
            />
            <hr />
            <Row>
                <Col>
                    <Button onClick={onSave} style={{ marginRight: '10px' }}>Сохранить</Button>
                </Col>
                <Col>
                    <Button onClick={onCancel}disabled={isSaveInProgress}>Отмена</Button>
                </Col>
            </Row>
            <RoutingWarningModal
                show={blocker.state === "blocked"}
                blocker={blocker} 
            />
        </Layout>
    );
};

export default OrdersDetailsPage;