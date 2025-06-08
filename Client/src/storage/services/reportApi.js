import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const reportApi = createApi({
    reducerPath: 'report',
    keepUnusedDataFor:0,
    baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/report` }), //  TODO: уточнить url
    tagTypes: ['report','reports'],
    endpoints: (builder) => ({
     /*   getPersonHistories: builder.query({
            query: () => '',
        }),

        getPersonHistoryPaged: builder.query({
            query: ({ pageNumber, pageSize, filterDataReq }) => {
                console.log('>>> [RTK Query] Вызван эндпоинт getPersonHistoryPaged');
                console.log('>>> Полученные аргументы:', { pageNumber, pageSize, filterDataReq });

                const url = `paged?page=${pageNumber}&size=${pageSize}${filterDataReq}`;
                console.log('>>> Сгенерирован URL:', url);

                return url;
            },
            providesTags: (result) => {
                var requestTags = result?.data ?
                    result.data.map(({id}) => ({type: 'PersonHistory', id })) : [];
                const listTag = { type: 'PersonHistories', id: 'LIST' };

                return [...requestTags, listTag];
            },
        }),*/
        addPFDOReport: builder.mutation({
            query: ({startDate,endDate}) => ({
                url: `/GetPFDOReport?startDate=${startDate}&endDate=${endDate}`,
                method: 'POST'
            }),
            invalidatesTags: [{type: 'reports', id: 'LIST'}]
        }),
        addRostatReport: builder.mutation({
            query: () => ({
                url: '/GetRostatReport',
                method: 'POST'
            }),
            invalidatesTags: [{type: 'reports', id: 'LIST'}]
        }),
    }),
});

export const {
    useAddPFDOReportMutation,
    useAddRostatReportMutation,
} = reportApi;