import React, { useState, useEffect, useCallback, useMemo } from 'react'
import { Layout, Loading, DetailsPageData, RoutingWarningModal } from '../shared/layout/index.js';
import { useParams, useBlocker, useNavigate } from 'react-router-dom'
import { Row, Col, Button } from 'antd'
import config from '../../storage/catalogConfigs/pendingRequests';
import personConfig from '../../storage/catalogConfigs/person';
import SelectModalItemsForm from '../shared/catalogProvider/forms/SelectModalItemsForm'

const PendingRequestDetailsPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [requestData, setRequestData] = useState({});
    const [isChanged, setIsChanged] = useState(false);
    const [initialData, setInitialData] = useState({});
    const [isSaveInProgress, setIsSaveInProgress] = useState(false);
    // ToDO: временное решение, надо будет сделать запрос на сервер, который будет проверят, нет ли подтвержденной заявки, с такими же атрибутами
    const [hasConflictResolveButton,setHasConflictResolveButton] = useState(false)
    const [currentPerson, setCurrentPerson] = useState(null);
    const [showModal, setShowModal] = useState(false);

    const { properties, crud } = config;

    const { useGetOneByIdAsync, useEditOneAsync, useCreateOneValidRequestByPerson,useCreateOneValidRequest} = crud;
    const { data, isLoading, isFetching, refetch } = useGetOneByIdAsync(id);

    const [editRequest] = useEditOneAsync();
    const [mutationTrigger,mutationResult] = useCreateOneValidRequestByPerson()
    const [createReqTrigger, triggerResult] = useCreateOneValidRequest()

    const filterString = useMemo(() => {
        if (!requestData) return '';

        const params = new URLSearchParams();

        if (requestData.name) params.append('name', requestData?.name);
        if (requestData.family) params.append('family', requestData?.family);
        if (requestData.patron) params.append('patron', requestData?.patron);
        if (requestData.birthDate) params.append('birthDate', requestData?.birthDate);
        if (requestData.phone) params.append('phone', requestData?.phone);
        if (requestData.email) params.append('email', requestData?.email);
        const queryString = params.toString();

        return `&${queryString}&matchAny=true`;
    }, [requestData]);


    // футер модалки
    const modalFooter =()=>(
        <>
            <Button key="confirm" type="primary" disabled={currentPerson === null}
                    onClick={() => {
                        mutationTrigger({pRequestId: id, personId: currentPerson.id })
                        setShowModal(false)
                        setHasConflictResolveButton(true)
                    }}>Подтвердить</Button>
            <Button key="close" onClick={() => setShowModal(false)}>Закрыть</Button>
        </>
    );

    useEffect(() => {
        if (!isLoading && !isFetching) {
            const newData = { ...data };
            delete newData.id;
            setRequestData(newData);
            console.log(newData);
            setInitialData(newData);
        }
    }, [isLoading, isFetching,showModal]);

    let blocker = useBlocker(
        ({ currentLocation, nextLocation }) =>
            isChanged &&
            currentLocation.pathname !== nextLocation.pathname
    );
    const handleSetPerson = (record) => {
        setCurrentPerson(record);
    }

    const openConflictResolutionPage = useCallback((personId, pendingRequestId) => {
        const pId = personId && personId.id ? personId.id : personId;
        navigate(`/conflictResolutions/${pId}/${pendingRequestId}`);
    }, [navigate]);

    const onSave = useCallback(() => {
        console.log(requestData)
        editRequest({ id, item: requestData });
        setIsChanged(false);
    },[id,requestData]);

    const onCancel = useCallback(() => {
        setRequestData(initialData);
        setIsChanged(false);
    }, [initialData]);

    return isLoading || isFetching
        ? (<Loading />)
        : (
            <>
                <Layout title={`Заявки - ${requestData.family} ${requestData?.name} ${requestData?.patron}`}>
                    <h2>{requestData.family} {requestData?.name} {requestData?.patron}</h2>
                    <DetailsPageData
                        items={properties}
                        data={requestData}
                        editData={setRequestData}
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
                            {(!requestData?.isArchive && <Button style={{ margin: '0 10px' }}  type="primary" onClick={() => {
                                setShowModal(true)
                            }}>
                                Привязать заявку к персоне
                            </Button>)}

                            <Button type="primary" onClick={() => createReqTrigger({pRequestId: id})}>
                                Подтвердить заявку
                            </Button>

                            {hasConflictResolveButton && (<Button style={{ margin: '0 10px'}} onClick={() => openConflictResolutionPage(currentPerson.id,id)}>
                                Разрешить конфликты привязки
                            </Button>)}

                        </Col>
                    </Row>
                    <RoutingWarningModal
                        show={blocker.state === "blocked"}
                        blocker={blocker}
                    />
                </Layout>
                <SelectModalItemsForm
                    config={personConfig}
                    modalTitle = {"Выберите человека, который, по вашему, подавал эту заявку"}
                    modalFooter = {modalFooter}
                    control={{
                        showForm: showModal,
                        setShowForm: setShowModal,
                        onRowHandleFunction: handleSetPerson
                    }}
                    filterString = {filterString}
                />
            </>
        );
};

export default PendingRequestDetailsPage;