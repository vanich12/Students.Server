
import {
    useGetPendingRequestsQuery,
    useGetPendingRequestsPagedQuery,
    useGetPendingRequestByIdQuery,
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

const useCreateOneValidRequest = () =>{
    const [mutationTrigger, mutationResult] = useAddRequestFromPendingRequestMutation();
    const {data, error, isUninitialized, isLoading, isSuccess, isError, reset} = mutationResult;
    return [mutationTrigger, mutationResult]
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
    useCreateOneValidRequest,
    useAddPendingRequestMutation as useAddOneAsync,
    useEditOneAsync,
    useRemoveOneAsync,
}