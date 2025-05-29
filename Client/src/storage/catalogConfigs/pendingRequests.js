import React, { useState, useEffect, useRef } from 'react';
import { Checkbox, Select } from 'antd'
import { CheckCircleFilled } from '@ant-design/icons';
import {
    useGetAllAsync,
    useGetAllPagedAsync,
  useGetOneByIdAsync,
    useCreateOneValidRequestByPerson,
    useCreateOneValidRequest,
   useAddOneAsync,
   useEditOneAsync,
    useRemoveOneAsync,
} from '../crud/pendingRequestsCrud.js';
import { useGetRequestStatusQuery } from '../services/requestStatusApi.js'
import { pendingRequestsModel } from '../models/index.js';
import BirthDate from '../../components/shared/business/BirthDate.jsx';
import EducationProgramSelect from '../../components/shared/business/selects/EducationProgramSelect'
import YesNo from '../../components/shared/business/YesNo'

//  TODO    лучше перенести эту реализацию в компонент в новый режим
const IsArchive = ({ record }) => {
    const { id, isArchive } = record;
    const [editProgram, { isSuccess, isError }] = useEditOneAsync();
    const [status, setStatus] = useState('');
    const checkboxRef = useRef(null);

    useEffect(() => {
        if (isError) {
            setStatus('error');
        }
    }, [isSuccess, isError]);

    const onChange = ({ target }) => {
        const editetProgram = { ...record };
        // удаляем лишние поля дял отправки
        delete editetProgram.id;
        delete editetProgram.educationForm;
        delete editetProgram.kindDocumentRiseQualification;
        editProgram({ id, item: { ...editetProgram, isArchive: target.checked }});
        checkboxRef.current.blur();
    };

    return (
        <Checkbox
            ref={checkboxRef}
            defaultChecked={isArchive}
            onChange={onChange}
        />
    );
};
//  TODO    лучше перенести эту реализацию в компонент RequestStatusSelect в новый режим
const StatusRequestForm = ({ record }) => {
    const { id, statusRequest, statusRequestId } = record;
    const { data, isLoading, isFetching, refetch } = useGetRequestStatusQuery();
    const [editRequest, { isSuccess, isError }] = useEditOneAsync();
    const [status, setStatus] = useState('');
    const selectRef = useRef(null);

    useEffect(() => {
        if (isError) {
            setStatus('error');
        }
    }, [isSuccess, isError]);

    const onChange = (statusRequestId) => {
        const editetRequest = { ...record };
        delete editetRequest.id;
        editRequest({ id, item: { ...editetRequest, statusRequestId }});
        selectRef.current.blur();
    };

    return (
        <Select
            showSearch
            ref={selectRef}
            defaultValue={statusRequest}
            style={{ minWidth: '150px' }}
            placeholder='Статус заявки'
            filterOption={(input, option) =>
                (option?.label ?? '').toLowerCase().includes(input.toLowerCase())
            }
            onChange={onChange}
            variant='borderless'
            loading={isLoading || isFetching}
            status={status}
            options={(data || []).map((d) => ({
                value: d.id,
                label: d.name,
            }))}
        />
    );
};

export default {
    detailsLink: 'pendingrequests',
    hasDetailsPage: true,
    serverPaged: true,
    properties: pendingRequestsModel,
    crud: {
        useGetAllAsync,
        useGetAllPagedAsync,
        useGetOneByIdAsync,
        useCreateOneValidRequestByPerson,
        useCreateOneValidRequest,
        useAddOneAsync,
        useEditOneAsync,
        useRemoveOneAsync,
    },
    columns: [
        {
            title: 'Ф.И.О. заявителя',
            dataIndex: 'studentFullName',
            key: 'studentFullName',
        },
        {
            title: 'Дата рождения',
            dataIndex: 'birthDate',
            key: 'birthDate',
            render: (_, record) => (<BirthDate value={record.birthDate} mode='info' />),
        },
        {
            title: 'Место проживания',
            dataIndex: 'address',
            key: 'address',
        },
        {
            title: 'Уровень образования',
            dataIndex: 'typeEducation',
            key: 'typeEducation',
        },
        {
            title: 'Программа обучения',
            dataIndex: 'educationProgram',
            key: 'educationProgram',
        },
        {
            title: 'E-mail',
            dataIndex: 'email',
            key: 'email',
        },/*
        {
            title: 'Статус',
            dataIndex: 'statusRequest',
            key: 'statusRequest',
            render: (_, record) => {
                return (<StatusRequestForm record={record} />);
            },
        },*/
 /*       {
            title: 'Обучающийся',
            key: 'trined',
            render: (_, { trained }) => {
                return trained && (<CheckCircleFilled style={{ color: "#52c41a" }}/>);
            },
        },*/
        {
            title: 'В архив',
            dataIndex: 'isArchive',
            key: 'archive',
            render: (_, record) => (<IsArchive record={record} />),
        },
    ],
    filters: [
        {
            key: 'educationProgram', // Используем оригинальный ключ данных, а не из dataConverter ('birthDate1')
            label: 'Программа обучения',
            /*          filterType: 'date',*/ // Тип фильтра - выбор даты
            /*        format: 'YYYY-MM-DD',  */// Указываем формат даты (важно для DatePicker и API)
            component:EducationProgramSelect
        },
        {
            key: 'IsArchive',
            label: 'Архив',
            component: YesNo
        }
    ],
    dataConverter: (data) => data,
};