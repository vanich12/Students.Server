
import {
    useGetPersonHistoriesQuery,
    useGetPersonHistoryPagedQuery,
    useGetPersonHistoryByIdQuery,
    useAddPersonHistoryMutation,
    useEditPersonHistoryMutation,
    useRemovePersonHistoryMutation,
} from '../services/personHistoryApi';

const useGetAllAsync = () => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetPersonHistoriesQuery();


    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};

const useGetAllPagedAsync = ({ pageNumber, pageSize, filterDataReq: queryString }) => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetPersonHistoryPagedQuery({ pageNumber, pageSize, filterDataReq: queryString });


    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};


const useRemoveOneAsync = () => {
    const [removeTrigger, removingResult] = useRemovePersonHistoryMutation();
    const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = removingResult;

    return [removeTrigger, removingResult];
};

export {
    useGetAllAsync,
    useGetAllPagedAsync,
    useGetPersonHistoryByIdQuery as useGetOneByIdAsync,
    useAddPersonHistoryMutation as useAddOneAsync,
    useEditPersonHistoryMutation as useEditOneAsync,
    useRemoveOneAsync,
}