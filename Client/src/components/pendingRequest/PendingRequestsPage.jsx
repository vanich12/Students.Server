import React from 'react';
import { Layout, EntityTable } from '../shared/layout/index.js';
import config from '../../storage/catalogConfigs/pendingRequest'

const PendingRequestsPage = () => {
    return (
        <Layout title="Неподтвержденные Заявки">
            <EntityTable config={config} />
        </Layout>
    );
};

export default PendingRequestsPage;