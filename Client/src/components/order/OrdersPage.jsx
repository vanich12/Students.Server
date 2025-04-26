import React from 'react';
import { Layout, EntityTable } from '../shared/layout/index.js';
import config from '../../storage/catalogConfigs/order.js'
const OrdersPage = () => {
    return (
        <Layout title="Приказы">
                <EntityTable config={config} />
        </Layout>
    );
};

export default OrdersPage;