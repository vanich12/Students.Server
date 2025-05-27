import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const pendingRequestsApi = createApi({
    reducerPath: 'pendingrequests',
    keepUnusedDataFor:0,
    baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/PendingRequest` }), //  TODO: уточнить url
    tagTypes: ['PRequest','PRequestById','PRequests'],
    endpoints: (builder) => ({
        getPendingRequests: builder.query({
            query: () => '',
        }),
        getPendingRequestsPaged: builder.query({
            query: ({ pageNumber, pageSize, filterDataReq }) => {const relativeUrlString = `paged?page=${pageNumber}&size=${pageSize}${filterDataReq}`
            console.log("Запрос идет на неподтвержденные заявки")
                console.log('[RTK Query Log] Generated Relative URL:', relativeUrlString);
                return relativeUrlString;
            },
            providesTags: (result)=>{
                var requestTags = result?.data ?
                    result.data.map(({id})=>({type: 'PRequest', id })):[];
                const listTag = { type: 'PRequests', id: 'LIST' };

                return [...requestTags, listTag];
            },
        }),
        getPendingRequestById: builder.query({
            query: (id) => id,
            providesTags: ['PRequestById'],
        }),
        addRequestFromPendingRequest: builder.mutation({
            query: ({pRequestId, personId}) => ({
                url: `/CreateRequestFromPendingRequest?pRequestId=${pRequestId}&personId=${personId}`,
                method: 'POST',
            }),
            invalidatesTags: (result, error, { pRequestId }) => [{ type: 'PRequestById', pRequestId }],
        }),
        addPendingRequest: builder.mutation({
            query: (request) => ({
                method: 'POST',
                body: request,
            }),
            invalidatesTags: [{type: 'PRequests', id: 'LIST'}]
        }),
        editPendingRequest: builder.mutation({
            query: ({ id, item }) => {

                console.log('Вызван editPendingRequest с аргументами:');
                console.log('id:', id);
                console.log('item:', item);

                return {
                    url: `/EditPendingRequest/${id}`,
                    method: 'PUT',
                    body: item,
                };
            },
            invalidatesTags: (result, error, { id }) => [{ type: 'PRequest', id }],
        }),
        removePendingRequest: builder.mutation({
            query: (id) => ({
                url: id,
                method: 'DELETE',
            }),
            invalidatesTags: [{type: 'PRequests', id: 'LIST'}],
        }),
    }),
});

export const {
    useGetPendingRequestsQuery,
    useGetPendingRequestsPagedQuery,
    // надо правильно называть хуки, от этого зависят их свойства (мутации, или запрос)
    useGetPendingRequestByIdQuery,
    useAddRequestFromPendingRequestMutation,
    useAddPendingRequestMutation,
    useEditPendingRequestMutation,
    useRemovePendingRequestMutation,
} = pendingRequestsApi;