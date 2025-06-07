import String from '../../components/shared/business/common/String.jsx';
import Date from '../../components/shared/business/common/Date.jsx';
import KindOrderSelect from '../../components/shared/business/selects/KindOrderSelect.jsx'
import GroupSelect from '../../components/shared/business/selects/GroupSelector.jsx'
import RequestSelectWithSearch from '../../components/shared/business/selects/RequestSelectWithSearch'
const model = {
    number: {
        name: 'Номер приказа',
        type: String,
    },
    kindOrderId: {
        name: 'Вид приказа', 
        type: KindOrderSelect,
    },
    date: { 
        name: 'Дата начала', 
        type: Date, 
    },
    groupId: {
        name: 'Группа', 
        type: GroupSelect,
    },
    requestId: {
        name: 'Заявка',
        type: RequestSelectWithSearch,
    },
  
};

export default model;