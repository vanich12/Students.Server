import React, {useState, useCallback, useEffect} from 'react';
import { Flex, Button, Space, DatePicker, Select, Input, Form } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import AddOneForm from './forms/AddOneForm';
import moment from 'moment';


const style = {
    height: '10vh',
    minHeight: '50px',
};

interface FilterFieldConfig {
    key: string;
    label: string;
    filterType: 'text' | 'select' | 'date' | 'number' | 'daterange';
    isFilterable?: boolean;
    placeholder?: string;
    options?: { value: any; label: string }[];
    selectMode?: 'multiple' | 'tags';
    picker?: 'date' | 'week' | 'month' | 'quarter' | 'year';
    format?: string;
    component?: React.ComponentType<any>;
}

interface FilterPanelProps {
    query: any;
    config: {
        properties?: any;
        filters?: FilterFieldConfig[];
        crud?: any;
        entityName?: string; // Добавили для заголовка (если нужно)
    };
    setQuery: (query: any) => void;
}

const FilterPanel = (props: FilterPanelProps) => {
    const { query, config, setQuery } = props;
    const { properties, crud, filters } = config;
    const [showAddOneForm, setShowAddOneForm] = useState(false);

    // --- 1. Локальное состояние объекта для непримененных фильтров ---
    // Инициализируем из ВНЕШНЕГО query
    const [localQuery, setLocalQuery] = useState(() => query || {});

    // --- 2. Синхронизация локального с внешним, если внешнее изменится ---
    useEffect(() => {
        setLocalQuery(query || {});
    }, [query]);

    const filterableFields = filters?.filter(field => field.isFilterable !== false && (field.component));

    // --- 3. handleFilterChange обновляет ЛОКАЛЬНЫЙ ОБЪЕКТ, агрегирует фильтры ---
    const handleFilterChange = useCallback((key: string, value: any) => {
        let actualValue = value;
        const isDayjsLike = actualValue && typeof actualValue === 'object' && '$y' in actualValue && typeof actualValue.format === 'function';

        // Обработка событий от стандартных input/textarea
        if (value && typeof value === 'object' && value.target !== undefined && 'value' in value.target) {
            actualValue = value.target.value;
        }

        if ( typeof value === 'boolean') {
            actualValue = value;
        }

        // Форматирование значения ПЕРЕД записью в локальное состояние
        if (moment.isMoment(actualValue)) {
            actualValue = actualValue.format('YYYY-MM-DD');
        } else if (isDayjsLike) {
            try { actualValue = actualValue.format('YYYY-MM-DD'); }
            catch (e) { console.error(`[Error] Failed to format dayjs-like object:`, e); }
        } else if (Array.isArray(actualValue) && actualValue.length === 2) {

        }

        // Обновляем локальное состояние для обычных полей
        setLocalQuery(prevLocalQuery => {
            // теперь newLocalQuery - это обьект со всем свойствами у prevLocalQuery
            // создаем обьект, который будет хранить в себе ключ - значение для запроса фильтров
            const newLocalQuery = { ...prevLocalQuery };
            if (actualValue === null || actualValue === undefined || actualValue === '') {
                delete newLocalQuery[key];
            } else {
                newLocalQuery[key] = actualValue;
            }
            console.log("[Local Query Updated]:", newLocalQuery);
            return newLocalQuery;
        });

    }, []); // Зависимостей нет при использовании функционального обновления

    // --- 4. Функция применения фильтров ---
    const applyFilters = useCallback(() => {
        setQuery(localQuery);
    }, [localQuery, setQuery]);

    // --- 5. Функция сброса фильтров ---
    const resetFilters = useCallback(() => {
        setLocalQuery({});
        setQuery({});
    }, [setQuery]);

    // --- 6. renderFilterInput читает из ЛОКАЛЬНОГО состояния ---
    const renderFilterInput = (field: FilterFieldConfig) => {
        if (field.component) {
            const CustomComponent = field.component;
            const currentValue = localQuery[field.key]; // Читаем из localQuery
            // ... (нужно адаптировать для RangePicker, если используете _gte/_lte)
            const customProps = {
                value: currentValue,
                onChange: (valueOrEvent: any) => handleFilterChange(field.key, valueOrEvent),
                allowClear: true,
                placeholder: field.placeholder || field.label,
                fieldConfig: field
            };
            return <CustomComponent mode={'filter'} {...customProps} />;
        }
        return null;
    };

    return (
        <>
            <Flex style={{ background: '#f9f9f9', flexDirection: 'column' }}>
                {/* Секция с полями фильтров */}
                <Flex style={{ padding: '10px 15px', gap: '15px', background: '#f9f9f9' }}>
                    {filterableFields?.length > 0 ? (
                        <Form layout="inline" style={{ flexGrow: 1 }}>
                            <Space wrap size="middle">
                                {filterableFields.map(field => (
                                    <Form.Item label={field.label} key={field.key} style={{ marginBottom: '0' }}>
                                        {renderFilterInput(field)}
                                    </Form.Item>
                                ))}
                            </Space>
                        </Form>
                    ) : (
                        <span style={{ flexGrow: 1, color: '#888' }}>Нет доступных фильтров</span>
                    )}
                    {crud?.useAddOneAsync && (
                        <Button type="primary" onClick={() => setShowAddOneForm(true)}>
                            <PlusOutlined />
                            Добавить
                        </Button>
                    )}
                </Flex>
                {filters &&
                    <Flex style={{ gap: '10px', padding: '0px 15px 10px 15px' }}>
                        <Button onClick={resetFilters}>
                            Сбросить фильтры
                        </Button>
                        <Button type="primary" onClick={applyFilters}>
                            Применить фильтры
                        </Button>
                    </Flex>
                }
            </Flex>

            {crud?.useAddOneAsync && (
                <AddOneForm
                    control={{ showAddOneForm, setShowAddOneForm }}
                    properties={properties}
                    crud={crud}
                />
            )}
        </>
    );
};

export default FilterPanel;