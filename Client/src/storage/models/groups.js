

const model = {
    name: {
        name: 'Наименование группы',
        type: 'String', // Используем строку
    },
    educationProgramId: {
        name: 'Программа обучения',
        type: 'EducationProgramSelect', // Используем строку
    },
    startDate: {
        name: 'Дата начала',
        type: 'Date', // Используем строку
    },
    endDate: {
        name: 'Дата окончания',
        type: 'Date', // Используем строку
    },
};

export default model;