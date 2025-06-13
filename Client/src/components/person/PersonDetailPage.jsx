import React, { useState, useEffect, useCallback } from 'react';
import { Layout, Loading, DetailsPageData, RoutingWarningModal } from '../shared/layout/index.js';
import { useParams, useBlocker } from 'react-router-dom';
import { Row, Col, Space, Button } from 'antd';
import config from '../../storage/catalogConfigs/person.js'
import studentConfig from '../../storage/catalogConfigs/students'
import enrollConfig from '../../storage/catalogConfigs/enrollingPerson'
import StandartSubmitDataHookForm from '../shared/catalogProvider/forms/StandartSubmitDataHookForm'

const PersonDetailsPage = () => {
    const { id } = useParams();
    const [personData, setPersonData] = useState({});
    const [initialData, setInitialData] = useState({});
    const [isChanged, setIsChanged] = useState(false);
    const [isSaveInProgress, setIsSaveInProgress] = useState(false);
    const [showAddOneForm,setShowAddOneForm] = useState(false);

    const { properties, crud } = config;
    const { useGetOneByIdAsync, useEditOneAsync ,useAddOneStudentByPerson} = crud;
    const { data, isLoading, isFetching, refetch } = useGetOneByIdAsync(id);

    const [editPerson] = useEditOneAsync();
    const [addTrigger] = useAddOneStudentByPerson();

    useEffect(() => {
        if (!isLoading && !isFetching) {
            const newData = { ...data };
            delete newData.id;
            setPersonData(newData);
            setInitialData(newData);
        }
    }, [isLoading, isFetching,data]);

    let blocker = useBlocker(
        ({ currentLocation, nextLocation }) =>
            isChanged &&
            currentLocation.pathname !== nextLocation.pathname
    );

    const onSave = useCallback(() => {
        console.log(personData)
        editPerson({ id, data: personData });
        setIsChanged(false);
    },[id,personData]);


    const onCancel = useCallback(() => {
        setPersonData(initialData);
        setIsChanged(false);
    }, [initialData]);

    return isLoading || isFetching
        ? (<Loading />)
        : (
            <>
            <Layout title="Данные кандидата">
                <h2>{personData.family} {personData?.name} {personData?.patron}</h2>
                <DetailsPageData
                    items={properties}
                    data={personData}
                    editData={setPersonData}
                    setIsChanged={setIsChanged}
                />
                <hr />
                <Row>
                    <Col>
                        <Button onClick={onSave} style={{ marginRight: '10px' }}>Сохранить</Button>
                    </Col>
                    <Col>
                        <Button onClick={onCancel} disabled={isSaveInProgress}>Отмена</Button>
                    </Col>
                    <Col>
                        <Button onClick={() => setShowAddOneForm(true)} type='primary' style={{ marginLeft: '10px' }}>Дополнить данные</Button>
                    </Col>
                </Row>
                <RoutingWarningModal
                    show={blocker.state === "blocked"}
                    blocker={blocker}
                />

            </Layout>
                <StandartSubmitDataHookForm
                    config={studentConfig}
                    crud={studentConfig.crud}
                    control={{ showAddOneForm, setShowAddOneForm }}
                    properties={enrollConfig.properties}
                    title={"Создание студента"}
                    mutationHook={crud.useAddOneStudentByPerson}
                    initialHookArgs={{personId: id}}
                />
            </>
        );
};

export default PersonDetailsPage;