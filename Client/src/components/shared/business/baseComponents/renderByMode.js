import Info from './Info.jsx';
import EditableInfo from './EditableInfo.jsx';
import Edit from './Edit.jsx';
import Form from './Form.jsx';
import Filter from './Filter.jsx';
import Modal from './Modal.jsx';
import Conflict from './Conflict.jsx'
import ConflictInfo from './ConflictInfo.jsx'

//Обертка, окружение, которое отображается в зависимости от режимов:
const renderByMode = {
    info: Info,
    editableInfo: EditableInfo,
    form: Form,
    filter: Filter,
    edit: Edit,
    conflict: Conflict,
    conflictInfo: ConflictInfo,
    modal: Modal,
};

export default renderByMode;