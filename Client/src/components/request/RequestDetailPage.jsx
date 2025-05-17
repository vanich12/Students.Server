import React, { useState, useEffect, useCallback, useMemo } from 'react'
import { Layout, Loading, DetailsPageData, RoutingWarningModal } from '../shared/layout/index.js';
import { useParams, useBlocker } from 'react-router-dom';
import { Row, Col, Space, Button, Flex } from 'antd'
import config from '../../storage/catalogConfigs/personRequests.js';
import personConfig from '../../storage/catalogConfigs/person';
import SelectModalItemsForm from '../shared/catalogProvider/forms/SelectModalItemsForm'


const RequestDetailsPage = () => {
    const { id } = useParams();
    const [requestData, setRequestData] = useState({});
    const [isChanged, setIsChanged] = useState(false);
    const [initialData, setInitialData] = useState({}); 
    const [isSaveInProgress, setIsSaveInProgress] = useState(false);
    const [currentPerson, setCurrentPerson] = useState(null);
    const [showModal, setShowModal] = useState(false);
    
    const { properties, crud } = config;
    const { useGetOneByIdAsync, useEditOneAsync, useEditBinding } = crud;
    const { data, isLoading, isFetching, refetch } = useGetOneByIdAsync(id);

    const [editRequest] = useEditOneAsync();

   const [bindTrigger, bindingResult] = useEditBinding();

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

        return `&${queryString}` ;
    }, [requestData]);

   // футер модалки
    const modalFooter =()=>(
          <>
              <Button key="confirm" type="primary" disabled={currentPerson === null}
              onClick={() => bindTrigger({requestId: id, person: currentPerson.id })}>Подтвердить</Button>
              <Button key="close" onClick={() => setShowModal(false)}>Закрыть</Button>
          </>
    );

    useEffect(() => {
        if (!isLoading && !isFetching) {
            const newData = { ...data };
            delete newData.id;
            setRequestData(newData);
            setInitialData(newData);
        }
    }, [isLoading, isFetching]);

    let blocker = useBlocker(
        ({ currentLocation, nextLocation }) =>
            isChanged &&
            currentLocation.pathname !== nextLocation.pathname
    );
    const handleSetPerson = (record) => {
        setCurrentPerson(record);
        console.log("выбранная персона")
        console.log(currentPerson)
    }

    const onSave = useCallback(() => {
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
                    {requestData.statusRequest === "Ожидает подтверждения" && (<Button style={{ margin: '0 10px' }}  type="primary" onClick={() => {
                        setShowModal(true)
                    }}>
                        Привязать заявку к персоне

                    </Button>)}
                </Col>
            </Row>
            <RoutingWarningModal
                show={blocker.state === "blocked"}
                blocker={blocker} 
            />
        </Layout>
            {requestData.statusRequest === "Ожидает подтверждения" &&(<SelectModalItemsForm
                config={personConfig}
                modalTitle = {"Выберите человека, который, по вашему, подавал эту заявку"}
                modalFooter = {modalFooter}
                control={{
                    showForm: showModal,
                    setShowForm: setShowModal,
                    onRowHandleFunction: handleSetPerson
                }}
                filterString = {filterString}
            />)}
      </>
  );
};

export default RequestDetailsPage;