﻿import String from '../../components/shared/business/common/String.jsx';
import Gender from '../../components/shared/business/Gender.jsx';
import Email from '../../components/shared/business/Email.jsx';
import Age from '../../components/shared/business/Age.jsx';
import Address from '../../components/shared/business/Address.jsx';
import EducationType from '../../components/shared/business/selects/EducationTypeSelect.jsx'
import ScopeOfActivitySelect from '../../components/shared/business/selects/ScopeOfActivitySelect.jsx'
import BirthDate from '../../components/shared/business/BirthDate.jsx';
import PhoneNumber from '../../components/shared/business/PhoneNumber.jsx';

const model = {
    family: {
        name: 'Фамилия',
        type: 'String',
    },
    name: {
        name: 'Имя',
        type: 'String',
    },
    patron: {
        name: 'Отчество',
        type: 'String',
    },
    birthDate: {
        name: 'Дата рождения',
        type: 'BirthDate',
    },
    sex: {
        name: 'Пол',
        type: 'Gender',
    },
    age: {
        name: 'Возраст',
        type: 'Age',
        formParams: {
            rules: [
                {
                    required: false,
                },
            ],
        },
        params: {
            show: {
                form: false,
            }
        },
    },
    address: {
        name: 'Место проживания',
        type: 'Address',
    },
    phone: {
        name: 'Номер телефона',
        type: 'PhoneNumber',
    },
    email: {
        name: 'E-mail',
        type: 'Email',
    },
    nationality: {
        name: 'Гражданство',
        type: 'String',
    },
    typeEducationId: {
        name: 'Уровень образования',
        type: 'EducationTypeSelect',
    },
    speciality: {
        name: 'Специальность',
        type: 'String',
    },
    iT_Experience:{
        name:'Опыт в IT',
        type: 'String',
    },
    scopeOfActivityLevelOneId: {
        name: 'Сфера деятельности ур.1',
        type: 'ScopeOfActivitySelect',
    },
    scopeOfActivityLevelTwoId: {
        name: 'Сфера деятельности ур.2',
        type: 'ScopeOfActivitySelect',
    },
};

export default model;