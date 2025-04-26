import React from 'react';
import dayjs, { Dayjs } from 'dayjs'; // Используем Dayjs для работы с датами
import moment from 'moment';
import { DatePicker, Space } from 'antd';

const dateFormat = 'DD.MM.YYYY';

// Типизация customParser
const customParser = (value: string): moment.Moment | string | undefined => {
    if (!value) return value;

    // Если введено ровно 8 символов
    if (value.length === 8) {
        // Преобразуем строку в формат "DD-MM-YYYY"
        const day = value.slice(0, 2);
        const month = value.slice(2, 4);
        const year = value.slice(4, 8);
        return moment(`${day}-${month}-${year}`, "DD-MM-YYYY");
    }

    return value;
};


interface DateFilterProps {
    placeholder?: string; // Опциональный placeholder
    onChange: (date: Dayjs | null, dateString: string) => void; // Функция, которая будет вызвана при изменении даты
}

const DateFilter = (props: DateFilterProps) => {
    const { placeholder, onChange } = props;
    return (
        <div className="col-2">
            <Space>
                <DatePicker
                    placeholder={placeholder}
                    format={dateFormat}
                    //parser={customParser}
                    defaultPickerValue={dayjs('05.03.1990', dateFormat)} // Пример значения по умолчанию
                    onChange={(date, dateString) => onChange(date, dateString)} // Передаем дату и строку
                />
            </Space>
        </div>
    );
};

export default DateFilter;
