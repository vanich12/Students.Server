import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const personApi = createApi({
    reducerPath: 'persons',
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

        CreatePersonBasedOnPRequest: builder.mutation({
            query: ({ pendingRequestId, form }) => ({
                url: 'CreatePersonBasedOnPRequest',
                method: 'POST',
                params: { pendingRequestId },
                body: form,
            }),
            invalidatesTags: [{type: 'Persons', id: 'LIST'}],
        }),

        editPerson: builder.mutation({
            query: ({id,data}) => { // Добавляем фигурные скобки для тела функции
                console.log('Отправляемый item (внутри query):', data); // Выводим item в консоль
                return { // Явно возвращаем объект конфигурации запроса
                    url: id,
                    method: 'PUT',
                    body: data,
                };
            },
            invalidatesTags: (result, error, { id }) => [ // Обратите внимание: здесь {id} деструктурируется из третьего аргумента (arg)
                {type: 'Person', id: id},
                {type:'Persons', id: 'LIST' }
            ],
        }),
        UpdatePersonBasedOnPRequest: builder.mutation({
            query: ({ pendingRequestId, personId, formValues }) => { // Изменяем на блочную функцию
                // Логируем объект form
                console.log('[RTK Query] UpdatePersonBasedOnPRequest - Input form:', formValues);
                console.log('[RTK Query] UpdatePersonBasedOnPRequest - pendingRequestId:', pendingRequestId);
                console.log('[RTK Query] UpdatePersonBasedOnPRequest - personId:', personId);

                // Возвращаем конфигурацию запроса
                return {
                    url: 'UpdatePersonBasedOnPRequest',
                    method: 'PUT',
                    params: { pendingRequestId, personId },
                    body: formValues,
                };
            },
            invalidatesTags: (result, error, { personId }) => [ // Не забудьте поправить здесь на id: personId
                { type: 'Person', id: personId },              // Было: { type: 'Person', personId }
                { type: 'Persons', id: 'LIST' }
            ],
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
    useCreatePersonBasedOnPRequestMutation,
    useEditPersonMutation,
    useUpdatePersonBasedOnPRequestMutation,
    useRemovePersonMutation,
} = personApi;