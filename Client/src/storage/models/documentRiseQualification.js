import String from '../../components/shared/business/common/String.jsx';


const model = {
    name: {
        name: 'Вид документа повышения квалификации',
        type: String,
        show: true,
        required: true
    },
    kindDocumentRiseQualificationId:{
        name:'Вид документа повышения квалификации',
        type: String,
    },
    date:{
        name:'Дата выдачи удостоверения',
        type: Date,
    },
    number:{
        name:'Номер выдачи удостоверения',
        type: String,
    },
    requestId:{
        name:'Заявка, на которую выдается документ',
        type: String,
    }

};

export default model;