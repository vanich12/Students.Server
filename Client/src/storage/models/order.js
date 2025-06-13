
const model = {
    number: {
        name: 'Номер приказа',
        type: 'String',
    },
    kindOrderId: {
        name: 'Вид приказа', 
        type: 'KindOrderSelect',
    },
    date: { 
        name: 'Дата начала', 
        type: 'Date',
    },
    groupId: {
        name: 'Группа', 
        type: 'GroupSelector'
    },
    requestId: {
        name: 'Заявка',
        type: 'RequestSelectWithSearch',
    },
  
};

export default model;