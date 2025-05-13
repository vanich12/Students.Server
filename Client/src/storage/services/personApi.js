import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const personApi = createApi({
    reducerPath: 'person',
    keepUnusedDataFor: 30,
    baseQuery: fetchBaseQuery(
        { baseUrl: `${apiUrl}/person` }),
    tagTypes: ['Persons', 'Person'],
    endpoints: (builder) => ({
        getPersons: builder.query({
            query: () => '',
            providesTags: (result) => result ? [
                ...result.map(({ id }) => ({ type: 'Persons', id })),
                { type: 'Persons', id: 'LIST' },
            ]: { type: 'Persons', id: 'LIST' },
        }),

        getPersonsPaged: builder.query({
            query: ({ pageNumber, pageSize, filterDataReq }) => {
                const relativeUrlString = `paged?page=${pageNumber}&size=${pageSize}${filterDataReq}`;
                console.log('[RTK Query Log] Endpoint: getPersonPaged');
                console.log('[RTK Query Log] Input Params:', { pageNumber, pageSize, filterDataReq });
                console.log('[RTK Query Log] Generated Relative URL:', relativeUrlString);
                return relativeUrlString;
            },

            // то есть помечаем тэгом каждый элемент в коллекции, и саму коллекцию тэгом, а если
            // коллекция пустая, то просто помечаем коллекцию тэгом
            providesTags: (result) => result ? [
                ...result.data.map(({ id }) => ({ type: 'Person', id })),
                { type: 'Persons', id: 'LIST' },
            ]: { type: 'Persons', id: 'LIST' },
        }),

        getPersonById: builder.query({
            query: (id) => id,
            providesTags: (result, error, id) => [
                { type: 'Person', id: id},
            ],
        }),

        addPerson: builder.mutation({
            query: (item) => ({
                method: 'POST',
                body: item,
            }),
            invalidatesTags: { type: 'Persons', id: 'LIST' },
        }),
        editPerson: builder.mutation({
            query: ({id, item}) => ({
                url: id,
                method: 'PUT',
                body: item,
            }),

            invalidatesTags: (result,error, id) =>[
                {type: 'Person', id: id},
                {type:'Persons', id: 'LIST' },],
        }),
        removePerson: builder.mutation({
            query: (id) => ({
                url: id,
                method: 'DELETE',
            }),
            invalidatesTags: { type: 'Persons', id: 'LIST' },
        }),
    }),
});

export const {
    useGetPersonQuery,
    useGetPersonsPagedQuery,
    useGetPersonByIdQuery,
    useAddPersonMutation,
    useEditPersonMutation,
    useRemovePersonMutation,
} = personApi;