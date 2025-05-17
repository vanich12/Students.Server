import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const requestsApi = createApi({
  reducerPath: 'personrequests',
  keepUnusedDataFor:0,
  baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/Request` }), //  TODO: уточнить url
  tagTypes: ['Request','RequestById','Requests'],
  endpoints: (builder) => ({
    getPersonRequests: builder.query({
      query: () => '',
    }),
    getPersonRequestsPaged: builder.query({
      query: ({ pageNumber, pageSize, filterDataReq }) => `paged?page=${pageNumber}&size=${pageSize}${filterDataReq}`,
      providesTags: (result)=>{
        var requestTags = result?.data ?
          result.data.map(({id})=>({type: 'Request', id })):[];
        const listTag = { type: 'Requests', id: 'LIST' };

        return [...requestTags, listTag];
      },
    }),
    getPersonRequestsOfStudent: builder.query({
      query: (studentId) => {
       const relativeUrlString = `GetListRequestsOfStudentExists?studentId=${studentId}`;
       const requestConfig ={
         url: relativeUrlString,
         method: 'GET'
       };
       return requestConfig;
      }
    }),
    editBindRequestToPerson: builder.mutation({
      query: ({requestId,personId}) => {
        const relativeUrlString = `BindRequestToPerson?requestId=${requestId}&personId=${personId}`;
        const requestConfig ={
          url: relativeUrlString,
          method: 'PUT',
        };
        return requestConfig;
      },
      invalidatesTags: (result, error, { requestId }) => [{ type: 'Request', id: requestId }],
    }),
    getPersonRequestById: builder.query({
      query: (id) => id,
      providesTags: ['RequestById'],
    }),
    addPersonRequest: builder.mutation({
      query: (request) => ({
        url: '/NewRequest',
        method: 'POST',
        body: request,
        invalidatesTags: ['Requests'],
      }),
      invalidatesTags: [{type: 'Students', id: 'LIST'}]
    }),
    editPersonRequest: builder.mutation({
      query: ({ id, item }) => ({
        url: `/EditRequest/${id}`,
        method: 'PUT',
        body: item,
      }),

      invalidatesTags: (result, error, { id }) => [{ type: 'Request', id }],
    }),
    removePersonRequest: builder.mutation({
      query: (id) => ({
        url: id,
        method: 'DELETE',
      }),
      invalidatesTags: ['Requests'],
    }),
  }),
});

export const {
  useGetPersonRequestsQuery,
  useGetPersonRequestsPagedQuery,
  useLazyGetPersonRequestsOfStudentQuery,
    // надо правильно называть хзуки, от этого зависят их свойства (мутации, или запрос)
  useEditBindRequestToPersonMutation,
  useGetPersonRequestByIdQuery,
  useAddPersonRequestMutation,
  useEditPersonRequestMutation,
  useRemovePersonRequestMutation,
} = requestsApi;