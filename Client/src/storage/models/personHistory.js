
const model = {
    OldFamily: {
        name: 'Старая фамилия',
        type: 'String',
    },
    NewFamily: {
        name: 'Новая фамилия',
        type: 'String',
    },
    OldName: {
        name: 'Старое имя',
        type: 'String',
    },
    NewName: {
        name: 'Новая имя',
        type: 'String',
    },
    oldPatroon: {
        name: 'Старое отчество',
        type: 'String',
    },
    NewPatroon: {
        name: 'Новое отчество',
        type: 'String',
    },
    changeDate: {
        name: 'Дата изменения',
        type: 'Date',
    },
    ChangeType: {
        name: 'Тип изменений',
        type: 'String',
    },
    personId: {
        name: 'Заявка',
        type: 'RequestSelectWithSearch',
    },

};

export default model;