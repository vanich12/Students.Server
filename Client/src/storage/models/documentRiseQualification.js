


const model = {

    kindDocumentRiseQualificationId:{
        name:'Вид документа повышения квалификации',
        type: 'kindDocumentRiseQualificationSelect',
    },
    date:{
        name:'Дата выдачи удостоверения',
        type: 'Date',
    },
    number:{
        name:'Номер выдачи удостоверения',
        type: 'String',
    },
    requestId:{
        name:'Заявка, на которую выдается документ',
        type: 'RequestSelectWithSearch',
    }

};

export default model;