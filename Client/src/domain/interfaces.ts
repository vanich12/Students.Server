
export interface IFieldMetadata {
    name: string; // Отображаемое имя
    // Можно добавить опциональное поле для связи с типом, если нужно для генерации UI/форм
    // typeHint?: string; // например 'string', 'Gender', 'BirthDate'
    formParams?: {
        rules?: Array<{
            required?: boolean;
            // другие правила валидации: minLength, pattern и т.д.
            [key: string]: any; // Для расширяемости
        }>;
        // другие параметры формы: placeholder, helperText и т.д.
    };
    params?: {
        show?: {
            form?: boolean; // Показывать ли в форме
            table?: boolean; // Показывать ли в таблице
            // ... другие контексты отображения
        };
        // другие кастомные параметры
        [key: string]: any;
    };
}

export interface IPagedPageData<T>  {
    currentPage: number;
    totalPages: number;
    pageSize: number;
    totalCount: number;
    hasPrevious: boolean;
    hasNext: boolean;
    data: T [];
}

export interface StudentDTO{
    id: string;
    studentFamily: string;
    studentName: string;
    studentPatron: string;
    studentFullName: string;
    birthDate: Date;
    address: string;
}
export interface RequestDTO{
    id: string;
    statusId: string;
    statusName: string;
    educationProgramId: string;
    programName: string;
    groupId: string;
    groupName: string;
    groupStartDate: Date;
    groupEndDate: Date;
}