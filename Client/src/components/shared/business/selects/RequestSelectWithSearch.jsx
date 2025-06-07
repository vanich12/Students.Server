import React from 'react';
import _ from 'lodash';
import QueryableSelect from '../common/QueryableSelect.jsx';
import config from '../../../../storage/catalogConfigs/personRequests.js';

const defaultRules = [
    {
        required: true,
        message: 'Необходимо заполнить это поле',
    },
];

const defaultFormParams = {
    labelKey: 'email',
    name: 'ФИО заявителя',
    normalize: (value) => value,
    rules: defaultRules,
};

// кастомное отображение списка выбора
const RequestSelectWithSearch = ({ value, formParams, ...props }) => {
    const { crud } = config;
    const { useGetOneByIdAsync, useGetAllAsync } = crud;
    const { data: dataById } = useGetOneByIdAsync(value);
    // Todo: сделать фильтры, чтобы выводились заявки без обучающихся, то есть не внесенные в приказ о зачислении
    const { data: allData } = useGetAllAsync();
    console.log( allData);

    const customPersonOptions = (allData || []).map(request => ({
        value: request.id,
        // label - это не строка, а JSX-элемент!
        label: (
            <div>
                <strong style={{ display: 'block' }}>
                    {request.studentFullName}
                </strong>
                <span style={{ fontSize: '12px', color: '#888' }}>
                Программа обучения: {request.educationProgram}
            </span>
            </div>
        ),
        // Чтобы поиск работал корректно, можно добавить дополнительное поле
        searchLabel: `${request.studentFullName} ${request.educationProgram}`
    }));

    return (
        <QueryableSelect
            {
                ...{
                    ...props,
                    dataById: dataById || {},
                    allData: allData || [],
                    options: customPersonOptions,
                    crud: config.crud,
                    formParams: _.merge({}, defaultFormParams, formParams),
                }
            }
        />
    );
};

export default RequestSelectWithSearch;