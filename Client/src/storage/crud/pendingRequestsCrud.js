
import {
    useGetPendingRequestsQuery,
    useGetPendingRequestsPagedQuery,
    useGetPendingRequestByIdQuery,
    useAddRequestFromPendingRequestMutation,
    useAddPendingRequestMutation,
    useEditPendingRequestMutation,
    useRemovePendingRequestMutation,
} from '../services/pendingRequestApi';

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

export {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetPendingRequestByIdQuery as useGetOneByIdAsync,
    useCreateOneValidRequest,
    useAddPendingRequestMutation as useAddOneAsync,
    useEditPendingRequestMutation as useEditOneAsync,
    useRemoveOneAsync,
}