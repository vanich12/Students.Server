// ========================================================================
//                           РЕЕСТР КОМПОНЕНТОВ
// ========================================================================

// КРИТИЧЕСКИ ВАЖНО: Переименовываем импорты, чтобы не конфликтовать
// с нативными объектами JavaScript (String, Date).

// Остальные импорты можно оставить как есть, их имена уникальны
import YesNo from '../components/shared/business/YesNo';
import Address from '../components/shared/business/Address';
import Age from '../components/shared/business/Age';
import BirthDate from '../components/shared/business/BirthDate';
import Cost from '../components/shared/business/Cost';
import Email from '../components/shared/business/Email';
import Gender from '../components/shared/business/Gender';
import HoursCount from '../components/shared/business/HoursCount';
import OrderDate from '../components/shared/business/OrderDate';
import PhoneNumber from '../components/shared/business/PhoneNumber';
import ScopeOfActivityLevel from '../components/shared/business/ScopeOfActivityLevel';
import Snils from '../components/shared/business/Snils';
import Date from '../components/shared/business/common/Date';
import YesNoButtons from '../components/shared/business/common/YesNoButtons';
import CheckBox from '../components/shared/business/common/CheckBox';
import String from '../components/shared/business/common/String';
import EducationFormSelect from '../components/shared/business/selects/EducationFormSelect';
import EducationProgramSelect from '../components/shared/business/selects/EducationProgramSelect';
import EducationTypeSelect from '../components/shared/business/selects/EducationTypeSelect';
import FEAProgramSelect from '../components/shared/business/selects/FEAProgramSelect';
import FinancingTypeSelect from '../components/shared/business/selects/FinancingTypeSelect';
import GroupSelector from '../components/shared/business/selects/GroupSelector';
import KindDocumentRiseQualificationSelect from '../components/shared/business/selects/KindDocumentRiseQualificationSelect';
import KindOrderSelect from '../components/shared/business/selects/KindOrderSelect';
import RequestSelectWithSearch from '../components/shared/business/selects/RequestSelectWithSearch';
import RequestStatusSelect from '../components/shared/business/selects/RequestStatusSelect';
import ScopeOfActivitySelect from '../components/shared/business/selects/ScopeOfActivitySelect';
import StatusEntrancExamsSelect from '../components/shared/business/selects/StatusEntrancExamsSelect';
import StudentsSelect from '../components/shared/business/selects/StudentsSelect';
import StudentStatusSelect from '../components/shared/business/selects/StudentStatusSelect';


export const componentRegistry = {
    // --- Общие типы ---
    'String': String, // Ключ - строка, значение - переименованный компонент
    'Date': Date,     // Ключ - строка, значение - переименованный компонент
    'YesNo': YesNo,
    'Address': Address,
    'Age': Age,
    'BirthDate': BirthDate,
    'Cost': Cost,
    'Email': Email,
    'Gender': Gender,
    'HoursCount': HoursCount,
    'OrderDate': OrderDate,
    'PhoneNumber': PhoneNumber,
    'ScopeOfActivityLevel': ScopeOfActivityLevel,
    'Snils': Snils,
    'CheckBox': CheckBox, // Аналогично

    // --- Селекты ---
    'EducationFormSelect': EducationFormSelect,
    'EducationProgramSelect': EducationProgramSelect,
    'EducationTypeSelect': EducationTypeSelect,
    'FEAProgramSelect': FEAProgramSelect,
    'FinancingTypeSelect': FinancingTypeSelect,
    'GroupSelector': GroupSelector,
    'kindDocumentRiseQualificationSelect': KindDocumentRiseQualificationSelect,
    'KindOrderSelect': KindOrderSelect,
    'RequestSelectWithSearch': RequestSelectWithSearch,
    'RequestStatusSelect': RequestStatusSelect,
    'ScopeOfActivitySelect': ScopeOfActivitySelect,
    'StatusEntrancExamsSelect': StatusEntrancExamsSelect,
    'StudentsSelect': StudentsSelect,
    'StudentStatusSelect': StudentStatusSelect,
};

// Экспортируем функцию, которая получает компонент по его строковому имени.
export const getComponentFromRegistry = (type) => {
    const Component = componentRegistry[type];

    if (!Component) {
        // Лог для разработчика, чтобы быстро найти проблему
        console.error(`Компонент с типом "${type}" не найден в componentRegistry.`);
        // Возвращаем компонент-заглушку, чтобы приложение не "упало" целиком,
        // а наглядно показало, где именно проблема.
        return () => <div style={{ color: 'red', border: '1px solid red', padding: '8px' }}>Компонент "{type}" не найден!</div>;
    }

    return Component;
};