import React, { useState, useEffect, useCallback } from 'react';
import { Layout, Loading, DetailsPageData, RoutingWarningModal } from '../shared/layout/index.js';
import { useParams, useBlocker } from 'react-router-dom';
import { Row, Col, Space, Button } from 'antd';
import config from '../../storage/catalogConfigs/groups.js';
import requestConfig from '../../storage/catalogConfigs/personRequests';
import { PlusOutlined } from '@ant-design/icons'
import ObjectsChoiceModalForm from '../shared/business/ObjectChoiceModalForm'

const GroupDetailsPage = () => {
    const { id } = useParams();
    const [groupData, setGroupData] = useState({});
    const [initialData, setInitialData] = useState({});
    const [isChanged, setIsChanged] = useState(false);
    const { properties, crud } = config;
    const [showRangeForm, setShowRangeForm] = useState(false);
    const { useGetOneByIdAsync, useEditOneAsync } = crud;
    const { data, isLoading, isFetching, refetch } = useGetOneByIdAsync(id);

    const [editGroup] = useEditOneAsync();

    useEffect(() => {
        if (!isLoading && !isFetching) {
            const newData = { ...data };
            delete newData.id;
            setGroupData(newData);
            setInitialData(newData);
        }
    }, [isLoading, isFetching, data]);

    let blocker = useBlocker(
        ({ currentLocation, nextLocation }) =>
            isChanged &&
            currentLocation.pathname !== nextLocation.pathname
    );

    const onSave = useCallback(() => {
        editGroup({ id, item: groupData });
        setIsChanged(false);
    },[id, groupData]);

    const onCancel = useCallback(() => {
        setGroupData(initialData);
        setIsChanged(false);
    }, [initialData]);

    return isLoading || isFetching
    ? (<Loading />)
    : (
        <>
        <Layout title={'Данные группы'}>
            <h2>{groupData.name}</h2>
            <DetailsPageData
                items={properties}
                data={groupData}
                editData={setGroupData}
                setIsChanged={setIsChanged}
            />
            <hr />
            <Row>
                <Col>
                    <Button onClick={onSave}>Сохранить</Button>
                </Col>
                <Col>
                    <Button  style={{ margin: '0 10px' }} onClick={onCancel}>Отмена</Button>
                </Col>
                {crud?.useAddSubjectRangeAsync && (
                    <Button type="primary" onClick={() => setShowRangeForm(true)}>
                        <PlusOutlined />
                        Добавить студентов в группу
                    </Button>
                )}
            </Row>
            <RoutingWarningModal
                show={blocker.state === "blocked"}
                blocker={blocker} 
            />
        </Layout>
            {crud?.useAddSubjectRangeAsync && (
                <ObjectsChoiceModalForm
                    requestConfig={requestConfig}
                    groupConfig={config}
                    control={{ showRangeForm, setShowRangeForm }}
                    id={id}
                />
            )}
        </>
    );
};

export default GroupDetailsPage;
