import React, { useEffect, useState, useMemo } from 'react'
import { Select, Tag, Space, Button, Input } from 'antd'
import { useDebounceValue } from '../../../../Hooks/useDebounce'
import config from '../../../../storage/catalogConfigs/students.js';

const { Option } = Select;
const { Search } = Input;

const CumulativelistSelector = ({ value, formParams, ...props }) => {
    const {crud, properties} = config;
    // сюда запрос, который будет отфильтрованных персон доставать
    const {useGetAllAsync} = crud;
    const { data: allData } = useGetAllAsync();

    const [searchTerm, setSearchTerm] = useState('');
    const [availableOptions, setAvailableOptions] = useState(allData);
    const [selectedItems, setSelectedItems] = useState([]);
    const [currentSelection, setCurrentSelection] = useState(null); // Для сброса Select

    const debounceValue =  useDebounceValue(searchTerm);
    const handleSelectChange = (value) => {
        if (value && allData) {
            // Ищем в полном списке, а не в отфильтрованном для дропдауна
            const selectedOption = allData.find(opt => opt.id === value);
            if (selectedOption && !selectedItems.find(item => item.id === selectedOption.id)) {
                setSelectedItems(prevItems => [...prevItems, selectedOption]);
            }
            setCurrentSelection(null); // Сбрасываем Select
            setSearchTerm('');
        }
    };
    const handleRemoveItem = (itemIdToRemove) => {
        const itemToRemove = selectedItems.find(item => item.id === itemIdToRemove);
        setSelectedItems(prevItems => prevItems.filter(item => item.id !== itemIdToRemove));
        //  возвращаем элемент обратно в доступные опции, если он был оттуда убран
         if (itemToRemove && !selectedItems.find(opt => opt.id === itemToRemove.id)) {
          setAvailableOptions(prevOptions => [...prevOptions, itemToRemove]);
         }
         setSearchTerm('');
    };

    const handleSearchChange = (e) => {
        setSearchTerm(e.target.value);
    };

    const optionsForSelect = useMemo(() => {
        if (!allData) {
            return [];
        }
        let filtered = allData;
        if (debounceValue) {
            filtered = filtered.filter(option => {
                const nameMatches = option.name?.toLowerCase().includes(debounceValue.toLowerCase());
                const fullNameMatches = option.personFullName?.toLowerCase().includes(debounceValue.toLowerCase());
                return nameMatches || fullNameMatches;
            });
        }

        filtered = filtered.filter(
            option => !selectedItems.some(selected => selected.id === option.id)
        );

        return filtered;
        // когда меняется стейт, происходит ререндер копонента
    }, [allData, debounceValue, selectedItems]);

    return (
        <div>
            <Space direction="vertical" style={{ width: '100%' }}>
                <Search placeholder = 'поиск' onSearch={handleSearchChange}/>
                <Select
                    style={{ width: 300 }}
                    placeholder="Выберите элемент для добавления"
                    onChange={handleSelectChange}
                    value={currentSelection} // Контролируем значение для возможности сброса
                >
                    {optionsForSelect.map(option => (
                        <Option key={option.id} value={option.id}>
                            {option.personFullName}
                        </Option>
                    ))}
                </Select>

                <div>
                    <h4>Выбранные элементы:</h4>
                    {selectedItems.length > 0 ? (
                        <Space wrap>
                            {selectedItems.map(item => (
                                <Tag
                                    key={item.id}
                                    closable
                                    onClose={() => handleRemoveItem(item.id)}
                                >
                                    {item.personFullName}
                                </Tag>
                            ))}
                        </Space>
                    ) : (
                        <p>Пока ничего не выбрано.</p>
                    )}
                </div>
            </Space>
        </div>
    );

};

export default CumulativelistSelector;