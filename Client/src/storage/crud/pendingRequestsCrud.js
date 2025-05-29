
import {
    useGetPendingRequestsQuery,
    useGetPendingRequestsPagedQuery,
    useGetPendingRequestByIdQuery,
    useAddRequestFromPendingRequestAndPersonMutation,
    useAddRequestFromPendingRequestMutation,
    useAddPendingRequestMutation,
    useEditPendingRequestMutation,
    useRemovePendingRequestMutation,
} from '../services/pendingRequestsApi';
import { useEditPersonRequestMutation } from '../services/requestsApi'
import useNotifications from '../../notifications/useNotifications'
import { useEffect } from 'react'

const useGetAllAsync = () => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetPendingRequestsQuery();


    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};

const useGetAllPagedAsync = ({ pageNumber, pageSize, filterDataReq: queryString }) => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } =  useGetPendingRequestsPagedQuery({ pageNumber, pageSize, filterDataReq: queryString});


    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};

const useCreateOneValidRequestByPerson = () =>{
    const [mutationTrigger, mutationResult] = useAddRequestFromPendingRequestAndPersonMutation();
    const {data, error, isUninitialized, isLoading, isSuccess, isError, reset} = mutationResult;
    return [mutationTrigger, mutationResult]
};
const useCreateOneValidRequest = () =>{
    const [createReqTrigger, triggerResult] = useAddRequestFromPendingRequestMutation();
    const {data, error, isUninitialized, isLoading, isSuccess, isError, reset} = triggerResult;
    return [createReqTrigger, triggerResult]
};

const useRemoveOneAsync = () => {
    const [removeTrigger, removingResult] = useRemovePendingRequestMutation();
    const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = removingResult;

    return [removeTrigger, removingResult];
};
const useEditOneAsync = () => {
    const [editItem, editingResult] = useEditPendingRequestMutation();
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
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetPendingRequestByIdQuery as useGetOneByIdAsync,
    useCreateOneValidRequestByPerson,
    useCreateOneValidRequest,
    useAddPendingRequestMutation as useAddOneAsync,
    useEditOneAsync,
    useRemoveOneAsync,
}