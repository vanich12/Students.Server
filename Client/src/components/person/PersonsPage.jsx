import React from 'react';
import { Layout, EntityTable } from '../shared/layout/index.js';
import config from '../../storage/catalogConfigs/person.js'
import DateFilter from '../shared/filters/DateFilter.tsx'
import { Flex } from 'antd'


const PersonsPage = () => {

    return (
        <Layout title="Подававшие заявки">
            <EntityTable config={config} />
        </Layout>
    );
};

export default PersonsPage;