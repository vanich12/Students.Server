import { useState, useEffect } from 'react';
import useNotifications from '../../notifications/useNotifications.js';
import { 
    useGetPersonRequestsQuery,
    useGetRequestToAddInGroupQuery,
    useGetPersonRequestsPagedQuery,
    useLazyGetPersonRequestsOfStudentQuery,
    useEditBindRequestToPersonMutation,
    useGetPersonRequestByIdQuery,
    useAddPersonRequestMutation,
    useEditPersonRequestMutation,
    useRemovePersonRequestMutation,
} from '../services/requestsApi.js';
import { useAddStudentInGroupMutation } from '../services/groupsApi'

const useGetAllAsync = () => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetPersonRequestsQuery();
  
  
    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};  

const useGetAllPagedAsync = ({ pageNumber, pageSize, filterDataReq: queryString }) => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetPersonRequestsPagedQuery({ pageNumber, pageSize, filterDataReq: queryString });
  
  
    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};
const useGetRequestToAddInGroupAsync = ({ pageNumber, pageSize, filterDataReq: queryString }) => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetRequestToAddInGroupQuery({ pageNumber, pageSize, filterDataReq: queryString });

    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};

/*const useGetReqByStudentId = ({studentId}) => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetPersonRequestsOfStudentQuery({ studentId });
    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};*/


const useGetReqByStudentId = (studentId, options) => { // (1) Принимаем studentId и options
    const [trigger, result] = useLazyGetPersonRequestsOfStudentQuery();

    useEffect(() => {
        if (studentId !== undefined && studentId !== null && (!options || !options.skip)) {
            trigger(studentId);
        }
    }, [studentId, trigger, options]);

    return [trigger,result];
};

const useEditBinding = () => {
    const [editTrigger, editingResult] = useEditBindRequestToPersonMutation();
    const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = editingResult;

    const { showSuccess, showError } = useNotifications();

    useEffect(() => {
        if (isSuccess) {
            showSuccess('Заявка успешно обновлена', 'описание уведомления');
        }
        if (isError) {
            showError('Ошибка! Редактирование заявки не удалось!', error);
        }
    }, [isSuccess, isError]);

    const editRequest = ({ id, item }) => {
        editTrigger({ id, item });
    };
    return [editRequest, editingResult];
}
  
const useRemoveOneAsync = () => {
    const [removeItem, removingResult] = useRemovePersonRequestMutation();
    const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = removingResult;
  
  
  
    return [removeItem, removingResult];
};

const useEditOneAsync = () => {
    const [editItem, editingResult] = useEditPersonRequestMutation();
    const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = editingResult;

    const { showSuccess, showError } = useNotifications();

    useEffect(() => {
        if (isSuccess) {
            showSuccess('Заявка успешно обновлена', 'описание уведомления');
        }
        if (isError) {
          showError('Ошибка! Редактирование заявки не удалось!', error);
        }
    }, [isSuccess, isError]);

    const editRequest = ({ id, item }) => {
      editItem({ id, item });
    };
    return [editRequest, editingResult];
};

export {
    useGetPersonRequestsQuery as useGetAllAsync,
    useGetRequestToAddInGroupAsync,
    useGetPersonRequestsPagedQuery as useGetAllPagedAsync,
    useGetReqByStudentId,
    useEditBinding,
    useGetPersonRequestByIdQuery as useGetOneByIdAsync,
    useAddPersonRequestMutation as useAddOneAsync,
    useEditOneAsync,
    useRemovePersonRequestMutation as useRemoveOneAsync,
}