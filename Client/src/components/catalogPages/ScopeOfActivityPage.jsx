import React from 'react';
import { Layout } from '../shared/layout/index.js';
import Catalog from '../shared/catalogProvider/Catalog.jsx';
import config from '../../storage/catalogConfigs/scopeOfActivity.js'

const ScopeOfActivityPage = () => {
    return (
        <Layout title="Сферы деятельности">
            <Catalog config={config} />
        </Layout>
    );
};

export default ScopeOfActivityPage;