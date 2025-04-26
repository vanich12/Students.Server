import React from 'react';
import { Layout, EntityTable } from '../shared/layout/index.js';
import config from '../../storage/catalogConfigs/students.js'
import DateFilter from '../shared/filters/DateFilter.tsx'
import { Flex } from 'antd'


const StudentsPage = () => {

    return (
        <Layout title="Студенты">
            <EntityTable config={config} />
        </Layout>
    );
};

export default StudentsPage;