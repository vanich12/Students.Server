import {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetOneByIdAsync,
    useGetLearningHistoryOfStudent,
    useAddOneAsync,
    useEditOneAsync,
    useRemoveOneAsync,
} from '../crud/studentsCrud.js';
import { studentsModel } from '../models/index.js';
import BirthDate from '../../components/shared/business/BirthDate.jsx';
import EducationProgramSelect from '../../components/shared/business/selects/EducationProgramSelect'
import DateFilter from '../../components/shared/catalogProvider/filters/DateFilter.tsx'

export default {
    detailsLink: 'student',
    hasDetailsPage: true,
    serverPaged: true,
    properties: studentsModel,
    crud: {
        useGetAllAsync,
        useGetAllPagedAsync,
        useGetOneByIdAsync,
        useGetLearningHistoryOfStudent,
        useAddOneAsync,
        useEditOneAsync,
        useRemoveOneAsync,
    },
    columns: [
        {
            title: 'Ф.И.О. обучающегося',
            dataIndex: 'fullName',
            key: 'fullName',
        },
        {
            title: 'Дата рождения',
            dataIndex: 'birthDate1',
            key: 'birthDate',
        },
        {
            title: 'Место проживания',
            dataIndex: 'address',
            key: 'address',
        },
        {
            title: 'Программа обучения',
            dataIndex: 'programName',
            key: 'programName',
        },
        {
            title: 'Группа',
            dataIndex: 'groupName',
            key: 'groupName',
        },
        {
            title: 'Год обучения',
            dataIndex: 'groupEndDate',
            key: 'groupEndDate',
        },
        {
            title: 'Статус заявки',
            dataIndex: 'statusRequestName',
            key: 'statusRequestName',
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