import React, { useState, useEffect, useCallback } from 'react';
import { Layout, Loading, DetailsPageData, RoutingWarningModal } from '../shared/layout/index.js';
import { useParams, useBlocker } from 'react-router-dom';
import { Row, Col, Button } from 'antd';
import config from '../../storage/catalogConfigs/documentRiseQualification.js';

const DocumentRiseQualificationDetailsPage = () => {
    const { id } = useParams();
    const [documentData, setDocumentData] = useState({});
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
            setDocumentData(newData);
            setInitialData(newData);
        }
    }, [isLoading, isFetching, data]);

    let blocker = useBlocker(
        ({ currentLocation, nextLocation }) =>
            isChanged &&
            currentLocation.pathname !== nextLocation.pathname
    );

    const onSave = useCallback(() => {
        editProgram({ id, item: documentData });
        setIsChanged(false);
    }, [id, documentData]);

    const onCancel = useCallback(() => {
        setDocumentData(initialData);
        setIsChanged(false);
    }, [initialData]);

    return isLoading || isFetching
        ? (<Loading />)
        : (
            <Layout title="Данные Документа">
                <h2>{documentData?.name}</h2>
                <DetailsPageData
                    items={properties}
                    data={documentData}
                    editData={setDocumentData}
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

export default DocumentRiseQualificationDetailsPage;