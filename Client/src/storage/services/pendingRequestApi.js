import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const pendingRequestApi = createApi({
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
        getPersonRequestById: builder.query({
            query: (id) => id,
            providesTags: ['PRequestById'],
        }),
        addRequestFromPendingRequest: builder.mutation({
            query: ({pRequestId, personId}) => ({
                url: `/CreateRequestFromPendingRequest?pRequestId=${pRequestId}&personId=${personId}`,
                method: 'POST',
                invalidatesTags: ['PRequests'],
            }),
            invalidatesTags: [{type: 'PRequests', id: 'LIST'}]
        }),
        addPendingRequest: builder.mutation({
            query: (request) => ({
                url: '/NewRequest',
                method: 'POST',
                body: request,
                invalidatesTags: ['PRequests'],
            }),
            invalidatesTags: [{type: 'Students', id: 'LIST'}]
        }),
        editPendingRequest: builder.mutation({
            query: ({ id, item }) => ({
                url: `/EditRequest/${id}`,
                method: 'PUT',
                body: item,
            }),

            invalidatesTags: (result, error, { id }) => [{ type: 'Request', id }],
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
    // надо правильно называть хзуки, от этого зависят их свойства (мутации, или запрос)
    useGetPendingRequestByIdQuery,
    useAddRequestFromPendingRequestMutation,
    useAddPendingRequestMutation,
    useEditPendingRequestMutation,
    useRemovePendingRequestMutation,
} = pendingRequestApi;