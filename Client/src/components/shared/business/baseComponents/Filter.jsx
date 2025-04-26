import React from 'react';

const Filter = ({ Component, props }) => {
    return (
        <Component {...props} />
    );
};

export default Filter;