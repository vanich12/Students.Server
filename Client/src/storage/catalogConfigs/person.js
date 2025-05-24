import {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetOneByIdAsync,
    useAddOneAsync,
    useAddOneByPRequest,
    useEditOneAsync,
    useEditOneByPRequest,
    useRemoveOneAsync,
} from '../crud/personsCrud.js';
import { personModel } from '../models/index.js';
import BirthDate from '../../components/shared/business/BirthDate.jsx';
import DateFilter from '../../components/shared/catalogProvider/filters/DateFilter.tsx'

export default {
    detailsLink: 'person',
    hasDetailsPage: true,
    serverPaged: true,
    properties: personModel,
    crud: {
        useGetAllAsync,
        useGetAllPagedAsync,
        useGetOneByIdAsync,
        useAddOneAsync,
        useAddOneByPRequest,
        useEditOneAsync,
        useEditOneByPRequest,
        useRemoveOneAsync,
    },
    columns: [
        {
            title: 'Ф.И.О. Личности',
            dataIndex: 'personFullName',
            key: 'fullName',
        },
        {
            title: 'Дата рождения',
            dataIndex: 'birthDate1',
            key: 'birthDate',
        },
        {
            title: 'Номер телефона',
            dataIndex: 'phoneNumber',
            key: 'phone',
        },
        {
            title: 'Email',
            dataIndex: 'email',
            key: 'email',
        },

    ],
    filters: [
        {
            key: 'birthDate',
            label: 'Дата рождения',
            format: 'yyyy-MM-dd',
            placeholder: 'Дата Рождения',
            component:DateFilter
        }
    ],
    dataConverter: (data) => {
        return data?.map(({ birthDate, ...props }) => {
            const birthDate1 = (
                <BirthDate value={birthDate} mode='info' />
            );
            return { ...props, birthDate1 };
        });
    },
};