

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
    snils: { 
        name: 'Снилс', 
        type: 'Snils',
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
    fullNameDocument: {
        name: 'Фамилия в дипломе о ВО/СПО', 
        type: 'String',
    },
    documentSeries: {
        name: 'Серия документа о ВО/СПО', 
        type: 'String',
    },
    documentNumber: {
        name: 'Номер документа о ВО/СПО', 
        type: 'String',
    },
    disability: {
        name: 'ОВЗ', 
        type: 'YesNo',
    },
    // projects: { 
    //     name: 'Проекты',
    //     type: String,
    // },
    // iT_Experience: {
    //     name: 'Опыт в IT', 
    //     type: String, 
    // },
};

export default model;