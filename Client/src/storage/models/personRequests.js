

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
    educationProgramId : { 
        name: 'Программа', 
        type: 'EducationProgramSelect',
    },
    typeEducationId : { 
        name: 'Уровень образования', 
        type: 'EducationTypeSelect',
    },
    iT_Experience: { 
        name: 'Опыт в IT', 
        type: 'String',
    },
    speciality: { 
        name: 'Специальность', 
        type: 'String',
    },                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
    projects: { 
        name: 'Проекты', 
        type: 'String',
    },
    statusEntrancExams: { 
        name: 'Тестовое задание', 
        type: 'StatusEntrancExamsSelect',
    },
    birthDate: { 
        name: 'Дата рождения', 
        type: 'BirthDate',
    },
    address: { 
        name: 'Место проживания',  
        type: 'Address',
    },
    phone: { 
        name: 'Телефон', 
        type: 'PhoneNumber',
    },
    email: { 
        name: 'E-mail', 
        type: 'Email',
    },
    scopeOfActivityLevelOneId: { 
        name: 'Сфера деятельности уровень 1', 
        type: 'ScopeOfActivitySelect',
    },
    scopeOfActivityLevelTwoId: { 
        name: 'Сфера деятельности уровень 2', 
        type: 'ScopeOfActivitySelect',
    },
    agreement: { 
        name: 'Согласие на обработку перс. даннных', 
        type: 'CheckBox',
    },
};

export default model;