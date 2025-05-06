import React from 'react';
import { Dropdown } from 'antd';


const DropdownMenu = ({items, open, children,menuPosition,menuClose,...restProps}) => {

    const menuItems = items || [];

    console.log(open);
    return (
        <Dropdown
            menu={{ items: menuItems }} // Используем переданные items
            open={open}
            onOpenChange={menuClose}
            trigger={['contextMenu']}  // Триггер по правому клику
            {...restProps}
        >
            {/* Невидимый элемент-якорь для позиционирования меню */}
            <div
                style={{
                    position: 'fixed',
                    left: menuPosition.x,
                    top: menuPosition.y,
                    width: 1,
                    height: 1,
                    pointerEvents: 'none', // Не перехватывает клики
                    opacity: 0,          // Невидимый
                }}
            />
        </Dropdown>
    );
};

export default DropdownMenu;