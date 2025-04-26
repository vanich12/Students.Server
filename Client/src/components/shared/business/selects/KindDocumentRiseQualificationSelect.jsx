import React from 'react';
import _ from 'lodash';
import QueryableSelect from '../common/QueryableSelect.jsx';
import config from '../../../../storage/catalogConfigs/kindDocumentRiseQualification.js';    

const defaultRules = [
    {
        required: true,
        message: 'Необходимо заполнить это поле',
    },
];

const defaultFormParams = {
    labelKey: 'name',
    name: 'Вид документа повышения квалификации',
    normalize: (value) => value,
    rules: defaultRules,
};

const KindDocumentRiseQualificationSelect = ({ value, formParams, ...props }) => {
    const { crud } = config;
    const { useGetOneByIdAsync, useGetAllAsync } = crud;
    const { data: dataById } = useGetOneByIdAsync(value);
    const { data: allData } = useGetAllAsync();

    return (
        <QueryableSelect
            {
                ...{
                    ...props,
                    dataById: dataById || {},
                    allData: allData || [],
                    crud: config.crud,
                    formParams: _.merge({}, defaultFormParams, formParams),
                }
            }
        />
    );
};

export default KindDocumentRiseQualificationSelect;