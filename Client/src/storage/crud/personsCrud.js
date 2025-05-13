
import {
    useGetPersonQuery,
    useGetPersonsPagedQuery,
    useGetPersonByIdQuery,
    useAddPersonMutation,
    useEditPersonMutation,
    useRemovePersonMutation,
} from '../services/personApi';

const useGetAllAsync = () => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetPersonQuery();


    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};

const useGetAllPagedAsync = ({ pageNumber, pageSize, filterDataReq: queryString }) => {
    const { data, isError, isSuccess, error, isLoading, isFetching, refetch } =  useGetPersonsPagedQuery({ pageNumber, pageSize, filterDataReq: queryString});


    return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
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
    useEditPersonMutation as useEditOneAsync,
    useRemoveOneAsync,
}