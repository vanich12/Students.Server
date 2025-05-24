import { Layout } from 'antd'
import React from 'react';
import ConflictResolutionForm from './ConflictResolutionForm'
import personConfig from '../../storage/catalogConfigs/person.js'
import pRequestConfig from '../../storage/catalogConfigs/pendingRequests.js'
import { useParams } from 'react-router-dom'

const ConflictResolutionPage = () =>{
    const { personId ,pendingRequestId} = useParams();
    console.log('Person ID из URL:', personId);
    console.log('Pending Request ID из URL:', pendingRequestId);
    return (
            <Layout title="Конфликты">
                <ConflictResolutionForm datasId={{personId, pendingRequestId}} configs={{personConfig:personConfig,
                    pendingRequestConfig:pRequestConfig}}/>
            </Layout>
    );
}

export default ConflictResolutionPage;