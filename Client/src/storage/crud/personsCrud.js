
import {
    useGetPersonQuery,
    useGetPersonsPagedQuery,
    useGetPersonByIdQuery,
    useAddPersonMutation,
    useCreatePersonBasedOnPRequestMutation,
    useEditPersonMutation,
    useUpdatePersonBasedOnPRequestMutation,
    useRemovePersonMutation,
} from '../services/personApi';
import { useEditPendingRequestMutation } from '../services/pendingRequestsApi'
import useNotifications from '../../notifications/useNotifications'
import { useEffect } from 'react'

const useGetAllAsync = () => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetPersonQuery();


    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};

const useGetAllPagedAsync = ({ pageNumber, pageSize, filterDataReq: queryString }) => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } =  useGetPersonsPagedQuery({ pageNumber, pageSize, filterDataReq: queryString});


    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};
const useAddOneByPRequest = () =>{
    const [addTrigger, addResult] = useCreatePersonBasedOnPRequestMutation();
    const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = addResult;

    return [addTrigger, addResult];
};

const useEditOneByPRequest = () =>{
    const [editItem, editingResult] = useUpdatePersonBasedOnPRequestMutation();
    const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = editingResult;

    const { showSuccess, showError } = useNotifications();

    useEffect(() => {
        if (isSuccess) {
            showSuccess('Персона успешно обновлена', 'описание уведомления');
        }
        if (isError) {
            showError('Ошибка! Редактирование персоны не удалось!', error);
        }
    }, [isSuccess, isError]);

    const editRequest = ({ pendingRequestId, personId, formValues  }) => {
        console.log('[useEditOneByPRequest] Received for editItem - pendingRequestId:', pendingRequestId);
        console.log('[useEditOneByPRequest] Received for editItem - personId:', personId);
        console.log('[useEditOneByPRequest] Received for editItem - form:', formValues);
        editItem({ pendingRequestId, personId, formValues  });
    };
    return [editRequest, editingResult];
};

const useRemoveOneAsync = () => {
    const [removeTrigger, removingResult] = useRemovePersonMutation();
    const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = removingResult;

    return [removeTrigger, removingResult];
};

export {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetPersonByIdQuery as useGetOneByIdAsync,
    useAddPersonMutation as useAddOneAsync,
    useAddOneByPRequest,
    useEditPersonMutation as useEditOneAsync,
     useEditOneByPRequest,
    useRemoveOneAsync,
}