import React, { useState, useEffect, useCallback } from 'react';
import { Layout, Loading, DetailsPageData, RoutingWarningModal } from '../shared/layout/index.js';
import { useParams, useBlocker } from 'react-router-dom';
import { Row, Col, Space, Button } from 'antd';
import config from '../../storage/catalogConfigs/students.js'
import SelectModalItemsForm from '../shared/catalogProvider/forms/SelectModalItemsForm'
import studentHistoryConfig from '../../storage/catalogConfigs/studentHistory.js';
import personHistoryConfig from '../../storage/catalogConfigs/personHistory.js';


const StudentDetailsPage = () => {
    const { id } = useParams();
    const [studentData, setStudentData] = useState({});
    const [initialData, setInitialData] = useState({});

    const [isChanged, setIsChanged] = useState(false);
    const [isSaveInProgress, setIsSaveInProgress] = useState(false);
    // для модалки с  истории обучений
    const [showModalLearningHistory, setShowModalLearningHistory] = useState(false);
    // для модалки с истории изменения ФИО
    const [showModalDataHistory, setShowModalDataHistory] = useState(false);


    const { properties, crud } = config;
    const { useGetOneByIdAsync, useEditOneAsync } = crud;
    const { data, isLoading, isFetching, refetch } = useGetOneByIdAsync(id);

    const [editStudent] = useEditOneAsync();

    useEffect(() => {
      if (!isLoading && !isFetching) {
        const newData = { ...data };
        delete newData.id;
        setStudentData(newData);
        setInitialData(newData);
        console.log(newData)
      }
    }, [isLoading, isFetching,data]);

    let blocker = useBlocker(
        ({ currentLocation, nextLocation }) =>
            isChanged &&
            currentLocation.pathname !== nextLocation.pathname
    );

    const onSave = useCallback(() => {
        editStudent({ id, data: studentData });
        setIsChanged(false);
    },[id,studentData]);

       
    const onCancel = useCallback(() => {
        setStudentData(initialData);
        setIsChanged(false);
    }, [initialData]);

    const modalFooter =()=>(
        <>
            <Button key="confirm" type="primary"
                    onClick={() => {
                        setShowModalLearningHistory(false)
                    }}>Подтвердить</Button>
            <Button key="close" onClick={() => setShowModalLearningHistory(false)}>Закрыть</Button>
        </>
    );


    return isLoading || isFetching
    ? (<Loading />)
    : (
        <>
        <Layout title="Персональные данные студента">
            <h2>{studentData.family} {studentData?.name} {studentData?.patron}</h2>
            <DetailsPageData
                items={properties}
                data={studentData}
                editData={setStudentData}
                setIsChanged={setIsChanged}
            />
            <hr />
            <Row>
                <Col>
                    <Button onClick={onSave}
                            style={{ marginRight: '10px' }}>
                        Сохранить</Button>
                </Col>
                <Col>
                    <Button onClick={onCancel}
                            disabled={isSaveInProgress}>
                        Отмена</Button>
                </Col>
                <Col>
                    <Button disabled={isSaveInProgress}
                            onClick={()=>setShowModalLearningHistory(true)}
                            style={{ marginLeft: '10px' }}
                            type="primary">
                        История обучений</Button>
                </Col>
                <Col>
                    <Button disabled={isSaveInProgress}
                            onClick={()=>setShowModalDataHistory(true)}
                            style={{ marginLeft: '10px' }}
                            type="primary">
                        История ФИО обучающегося
                    </Button>
                </Col>
            </Row>
            <RoutingWarningModal
                show={blocker.state === "blocked"}
                blocker={blocker} 
            />
        </Layout>
            <SelectModalItemsForm
                useDataHook={config.crud.useGetLearningHistoryOfStudent}
                dataHookArgs={{ studentId: id, hasGroup: true }}
                config={studentHistoryConfig}
                modalTitle = {"История обучений обучающегося"}
                modalFooter={modalFooter}
                control={{
                    showForm: showModalLearningHistory,
                    setShowForm: setShowModalLearningHistory,
                }}
                filterString={""}
            />
            <SelectModalItemsForm
                config={personHistoryConfig}
                filterString={`&personId=${studentData.personId}`}
                modalTitle = {"История изменений ФИО обучающегося"}
                control={{
                    showForm: showModalDataHistory,
                    setShowForm: setShowModalDataHistory,
                }}
            />
        </>
  );
};

export default StudentDetailsPage;