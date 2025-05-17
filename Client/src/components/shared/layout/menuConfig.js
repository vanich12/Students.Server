import { useNavigate, useLocation } from 'react-router-dom';
import {
  SendOutlined,
  TeamOutlined, 
  ReadOutlined,
  FileDoneOutlined,
  ContactsOutlined,
  FilePptOutlined,
} from '@ant-design/icons';

const openedKey = {
    '/pendingrequests' : 'catalogs',
    '/program': 'catalogs',
    '/group' : 'catalogs',
    '333' : 'catalogs',
    '/educationForm' : 'settings',
    '/statusRequest' : 'settings',
    '/typeEducation' : 'settings',
    '/studentStatus': 'settings',
    '/kindOrder' : 'settings',
    '/kindDocumentRiseQualification' : 'settings',
    '/financingType' : 'settings',
    '/fEAProgram': 'settings',
};

const useMenuConfig = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const menuItems = [
        {
            key: '/pendingrequests',
            label: 'Неподтвержденные заявки',
            onClick: () => {navigate('/pendingrequests')},
        },
        {
            key: '/requests',
            label: 'Заявки',
            onClick: () => {navigate('/requests')},
        },
        { type: 'divider' },
        {
            key: '/students',
            label: 'Обучающиеся',
            onClick: () => {navigate('/students')},
        },
        { type: 'divider' },
        {
            key: 'catalogs',
            label: 'Справочники',
            children: [
                {
                    key: '/program',
                    label: 'Программы',
                    onClick: () => {navigate('/program')},
                },
                {
                    key: '/group',
                    label: 'Группы',
                    onClick: () => {navigate('/group')},
                },
                {
                    key: '/order',
                    label: 'Приказы',
                    onClick: () => {navigate('/order')},
                },
            ],
        },
        { type: 'divider' },
        {
            key: '/report',
            label: 'Отчеты',
            onClick: () => {navigate('/report')},
        },
        { type: 'divider' },
        {
            key: 'settings',
            label: 'Настройка системы',
            children: [
                {
                    key: '/educationForm',
                    label: 'Форма образования',
                    onClick: () => {navigate('/educationForm')},
                },
                {
                    key: '/statusRequest',
                    label: 'Статусы заявки',
                    onClick: () => {navigate('/statusRequest')},
                },
                {
                    key: '/typeEducation',
                    label: 'Типы образования',
                    onClick: () => {navigate('/typeEducation')},
                },
                {
                    key: '/studentStatus',
                    label: 'Статусы студента',
                    onClick: () => {navigate('/studentStatus')},
                },
                {
                    key: '/kindOrder',
                    label: 'Типы приказов',
                    onClick: () => {navigate('/kindOrder')},
                },
                {
                    key: '/kindDocumentRiseQualification',
                    label: 'Виды документа повышения квалификации',
                    onClick: () => {navigate('/kindDocumentRiseQualification')},
                },
                {
                    key: '/financingType',
                    label: 'Типы финансирования',
                    onClick: () => {navigate('/financingType')},
                },
                {
                    key: '/fEAProgram',
                    label: 'ВЭД программы',
                    onClick: () => {navigate('/fEAProgram')},
                },
                {
                    key: '/scopeOfActivity',
                    label: 'Сферы деятельности',
                    onClick: () => {navigate('/scopeOfActivity')},
                },
            ],
        },
        { type: 'divider' },
    ];

    return { 
        selectedKey: location.pathname,
        openedKey: openedKey[location.pathname] ?? '',
        menuItems,
    };
};

export default useMenuConfig;