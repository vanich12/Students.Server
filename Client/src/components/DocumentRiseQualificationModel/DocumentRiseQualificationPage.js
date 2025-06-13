import React from 'react';
import { Layout, EntityTable } from '../shared/layout/index.js';
import config from '../../storage/catalogConfigs/documentRiseQualification.js'

const DocumentRiseQualificationPage = () => {
    return (
        <Layout title="Документы о провышении квалификации">
            <EntityTable config={config} />
        </Layout>
    );
};

export default DocumentRiseQualificationPage;