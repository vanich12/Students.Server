import String from '../../components/shared/business/common/String.jsx';
import EducationProgramSelect from '../../components/shared/business/selects/EducationProgramSelect.jsx';
import EducationTypeSelect from '../../components/shared/business/selects/EducationTypeSelect.jsx';
import ScopeOfActivitySelect from '../../components/shared/business/selects/ScopeOfActivitySelect.jsx';
import CheckBox from '../../components/shared/business/common/CheckBox.jsx';
import BirthDate from '../../components/shared/business/BirthDate.jsx';
import Address from '../../components/shared/business/Address.jsx';
import Email from '../../components/shared/business/Email.jsx';
import PhoneNumber from '../../components/shared/business/PhoneNumber.jsx';
import YesNo from '../../components/shared/business/YesNo'

const model = {
    family: {
        name: 'Фамилия',
        type: String,
    },
    name: {
        name: 'Имя',
        type: String,
    },
    patron: {
        name: 'Отчество',
        type: String,
    },
    educationProgram : {
        name: 'Программа',
        type: String,
    },
    educationLevel : {
        name: 'Уровень образования',
        type: String,
    },
    iT_Experience: {
        name: 'Опыт в IT',
        type: String,
    },
    birthDate: {
        name: 'Дата рождения',
        type: BirthDate,
    },
    address: {
        name: 'Место проживания',
        type: Address,
    },
    phone: {
        name: 'Телефон',
        type: PhoneNumber,
    },
    email: {
        name: 'E-mail',
        type: Email,
    },
    scopeOfActivityLevelOneId: {
        name: 'Сфера деятельности уровень 1',
        type: ScopeOfActivitySelect,
    },
    scopeOfActivityLevelTwoId: {
        name: 'Сфера деятельности уровень 2',
        type: ScopeOfActivitySelect,
    },
    agreement: {
        name: 'Согласие на обработку перс. даннных',
        type: CheckBox,
    },
    isArchive: {
        name: 'В архиве',
        type: YesNo,
        show: true,
        required: true
    },
};

export default model;