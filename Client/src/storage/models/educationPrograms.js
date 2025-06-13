
const model = {
    name: { name: 'Программа обучения', type: 'String', show: true, required: true },
    cost: { name: 'Стоимость', type: 'Cost', show: true, required: true },
    hoursCount: { name: 'Кол-во часов', type: 'HoursCount', show: true, required: true },
    educationFormId: { name: 'Форма образования', type: 'EducationFormSelect', show: true, required: true },
    kindDocumentRiseQualificationId: { name: 'Вид программы', type: 'KindDocumentRiseQualificationSelect', show: true, required: true },
    isModularProgram: { name: 'Модульная программа', type: 'YesNo', show: true, required: true },
    feaProgramId: { name: 'ВЭД программы', type: 'FEAProgramSelect', show: true, required: true },
    financingTypeId: { name: 'Источник финансирования', type: 'FinancingTypeSelect', show: true, required: true },
    isCollegeProgram: { name: 'Обязательно наличие ВО', type: 'YesNo', show: true, required: true },
    isNetworkProgram: { name: 'Сетевая форма', type: 'YesNo', show: true, required: true },
    isDOTProgram: { name: 'Применение ДОТ', type: 'YesNo', show: true, required: true },
    isFullDOTProgram: { name: 'Применение ДОТ полностью', type: 'YesNo', show: true, required: true },
    qualificationName: { name: 'Наименование квалификации', type: 'String', show: true, required: true },
    isArchive: { name: 'В архиве', type: 'YesNo', show: true, required: true },
};

export default model;
