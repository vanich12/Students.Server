import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const personHistoryApi = createApi({
    reducerPath: 'personHistory',
    keepUnusedDataFor:0,
    baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/Request` }), //  TODO: уточнить url
    tagTypes: ['PersonHistory','PersonHistoryById','PersonHistories'],
    endpoints: (builder) => ({
        getPersonHistories: builder.query({
            query: () => '',
        }),

        getPersonHistoryPaged: builder.query({
            query: ({ pageNumber, pageSize, filterDataReq }) => `paged?page=${pageNumber}&size=${pageSize}`,
            providesTags: (result)=>{
                var requestTags = result?.data ?
                    result.data.map(({id})=>({type: 'PersonHistory', id })):[];
                const listTag = { type: 'PersonHistories', id: 'LIST' };

                return [...requestTags, listTag];
            },

        }),
        getPersonHistoryById: builder.query({
            query: (id) => id,
            providesTags: ['PersonHistoryById'],
        }),
        editPersonHistory: builder.mutation({
            query: ({ id, item }) => ({
                url: `/EditPersonHistory/${id}`,
                method: 'PUT',
                body: item,
            }),

            invalidatesTags: (result, error, { id }) => [{ type: 'Request', id }],
        }),
        removePersonHistory: builder.mutation({
            query: (id) => ({
                url: id,
                method: 'DELETE',
            }),
            invalidatesTags: ['PersonHistories'],
        }),
    }),
});

export const {
    useGetPersonHistoriesQuery,
    useGetPersonHistoryPagedQuery,
    useGetPersonHistoryByIdQuery,
    useAddPersonHistoryMutation,
    useEditPersonHistoryMutation,
    useRemovePersonHistoryMutation,
} = personHistoryApi;