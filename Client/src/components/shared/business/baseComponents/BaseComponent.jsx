import React, { useState, useCallback, useEffect } from 'react';
import renderByMode from './renderByMode.js';
import defaultComponentsByMode from './componentsByMode.js';
import _ from 'lodash';
import defaultFormParams from './formParams.js';
import defaultParams from './params.js';

const BaseComponent = ({ formParams, params, ...props }) => {
    // mode передается уже в самой форме представления
    const { components, mode, value, setValue } = props;
    const [currentMode, setCurrentMode] = useState(mode);
    const [changed, setChanged] = useState(false);

    const handleSetValue = useCallback((newValue) => {
        console.log(newValue);
        setChanged(newValue !== value);
        console.log(value)
        setValue(newValue);
        console.log(value);
    }, [value, setValue]);
    //выбирается в зависимости от режима, который был сюда передан, например editableInfo
    const ComponentByMode = { ...defaultComponentsByMode, ...components }[currentMode];
    //компонент обертка, которая указывает в чем будет ComponentByMode
    const MultimodeComponent = renderByMode[currentMode] ?? renderByMode.info;
// я так понял, что передача нужных пропсов по компонента, которые пришли из components происходит именно тут, то есть MultimodeComponent, отдает нужные пропсы ComponentByMode
    return (
        <MultimodeComponent
            Component={ComponentByMode}
            props={{
                ...props,
                setValue: handleSetValue,
                setMode: setCurrentMode,
                changed,
                params: _.merge({}, defaultParams, params),
                formParams: _.merge({}, defaultFormParams, formParams),
            }}
        />
    );
};

export default BaseComponent;
