import String from '../../components/shared/business/common/String.jsx';
import Date from '../../components/shared/business/common/Date.jsx';
import KindOrderSelect from '../../components/shared/business/selects/KindOrderSelect.jsx'
import StudentsSelect from '../../components/shared/business/selects/StudentsSelect.jsx'
import GroupSelect from '../../components/shared/business/selects/GroupSelector.jsx'
import CumulativelistSelector from '../../components/shared/business/selects/CumulativelistSelector'
const model = {
    name: { 
        name: 'Номер приказа',
        type: String,
    },
    kindOrder: { 
        name: 'Вид приказа', 
        type: KindOrderSelect,
    },
    date: { 
        name: 'Дата начала', 
        type: Date, 
    },
    group: { 
        name: 'Группа', 
        type: GroupSelect,
    },
    student: { 
        name: 'Обучающийся', 
        type: CumulativelistSelector,
    },
  
};

export default model;