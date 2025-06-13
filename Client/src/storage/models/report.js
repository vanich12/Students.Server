
const model = {
    // --- Общая информация о документе ---
    typeDocument: {
        name: 'Тип документа',
        type: 'String',
    },
    statusDocument: {
        name: 'Статус документа',
        type: 'String',
    },
    confirmationLoss: {
        name: 'Подтверждение утраты',
        type: 'String',
    },
    confirmationExchange: {
        name: 'Подтверждение обмена',
        type: 'String',
    },
    confirmationDestruction: {
        name: 'Подтверждение уничтожения',
        type: 'String',
    },
    seriesDocuments: {
        name: 'Серия документа',
        type: 'String',
    },
    documentNumber: {
        name: 'Номер документа',
        type: 'String',
    },
    dateOfIssueDocument: {
        name: 'Дата выдачи документа',
        type: 'String', // Можно использовать DateOnlyType, если формат всегда 'YYYY-MM-DD'
    },
    registrationNumber: {
        name: 'Регистрационный номер',
        type: 'String',
    },

    // --- Информация о программе обучения ---
    additionalProfessionalProgram: {
        name: 'Дополнительная профессиональная программа',
        type: 'String',
    },
    nameAdditionalProfessionalProgram: {
        name: 'Наименование дополнительной профессиональной программы',
        type: 'String',
    },
    nameFieldProfessionalActivity: {
        name: 'Наименование области профессиональной деятельности',
        type: 'String',
    },
    enlargedGroupsSpecialties: {
        name: 'Укрупненные группы специальностей',
        type: 'String',
    },
    nameQualification: {
        name: 'Наименование квалификации, профессии, специальности',
        type: 'String',
    },
    yearBeginningTraining: {
        name: 'Год начала обучения',
        type: 'String',
    },
    yearGraduation: {
        name: 'Год окончания обучения',
        type: 'String',
    },
    durationTraining: {
        name: 'Срок обучения, часов',
        type: 'String', // Или Number, если это всегда число
    },

    // --- Информация о получателе ---
    recipientLastName: {
        name: 'Фамилия получателя',
        type: 'String',
    },
    recipientName: {
        name: 'Имя получателя',
        type: 'String',
    },
    recipientPatronymic: {
        name: 'Отчество получателя',
        type: 'String',
    },
    recipientDateBirth: {
        name: 'Дата рождения получателя',
        type: 'Date',
    },
    recipientGender: {
        name: 'Пол получателя',
        type: 'String',
    },
    recipientSNILS: {
        name: 'СНИЛС',
        type: 'String',
    },
    recipientCitizenship: {
        name: 'Гражданство получателя (код страны по ОКСМ)',
        type: 'String',
    },

    // --- Информация об условиях обучения ---
    formEducation: {
        name: 'Форма обучения',
        type: 'String',
    },
    sourceFundingForTraining: {
        name: 'Источник финансирования обучения',
        type: 'String',
    },
    formEducationAtTimeTerminationEducation: {
        name: 'Форма получения образования на момент прекращения образовательных отношений',
        type: 'String',
    },

    // --- Информация о предыдущем образовании (ВО/СПО) ---
    levelEducationHE: {
        name: 'Уровень образования ВО/СПО',
        type: 'String',
    },
    nameDocumentEducationOriginalDocument: {
        name: 'Наименование документа об образовании (оригинала)',
        type: 'String',
    },
    surnameIndicatedHE: {
        name: 'Фамилия указанная в дипломе о ВО или СПО',
        type: 'String',
    },
    seriesHE: { // Возможно, дублирует seriesOriginalDocument
        name: 'Серия документа о ВО/СПО',
        type: 'String',
    },
    numberHE: { // Возможно, дублирует numberOriginalDocument
        name: 'Номер документа о ВО/СПО',
        type: 'String',
    },

    // --- Информация об оригинальном документе (в случае дубликата) ---
    seriesOriginalDocument: {
        name: 'Серия (оригинала)',
        type: 'String',
    },
    numberOriginalDocument: {
        name: 'Номер (оригинала)',
        type: 'String',
    },
    registrationNumberOriginalDocument: {
        name: 'Регистрационный N (оригинала)',
        type: 'String',
    },
    dateIssueOriginalDocument: {
        name: 'Дата выдачи (оригинала)',
        type: 'String', // Или DateOnlyType
    },
    recipientLastNameOriginalDocument: {
        name: 'Фамилия получателя (оригинала)',
        type: 'String',
    },
    recipientNameOriginalDocument: {
        name: 'Имя получателя (оригинала)',
        type: 'String',
    },
    recipientPatronymicOriginalDocument: {
        name: 'Отчество получателя (оригинала)',
        type: 'String',
    },

    // --- Служебное поле ---
    numberDocumentToChange: {
        name: 'Номер документа для изменения',
        type: 'String',
    },
};
export default model;