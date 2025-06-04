import Snils from '../../components/shared/business/Snils'
import String from '../../components/shared/business/common/String'
import YesNo from '../../components/shared/business/YesNo'

const model = {

    snils: {
        name: 'Снилс',
        type: Snils,
    },
    speciality: {
        name: 'Специальность',
        type: String,
    },
    fullNameDocument: {
        name: 'Фамилия в дипломе о ВО/СПО',
        type: String,
    },
    documentSeries: {
        name: 'Серия документа о ВО/СПО',
        type: String,
    },
    documentNumber: {
        name: 'Номер документа о ВО/СПО',
        type: String,
    },
    disability: {
        name: 'ОВЗ',
        type: YesNo,
    },
    projects: {
        name: 'Проекты',
        type: String,
    },
    // iT_Experience: {
    //     name: 'Опыт в IT',
    //     type: String,
    // },
};

export default model;