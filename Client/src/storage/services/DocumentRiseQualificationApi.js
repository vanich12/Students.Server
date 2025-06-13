import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const DocumentRiseQualificationApi = createApi({
  reducerPath: 'documentRiseQualification',
  tagTypes:['DocumentRiseQualification','DocumentRiseQualifications'],
  baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/documentRiseQualification` }),
  endpoints: (builder) => ({
    getDocumentRiseQualification: builder.query({
      query: () => '',
      providesTags: (result)=>{
        var tags = result?.data ?
            result.data.map(({id})=>({type: 'DocumentRiseQualification', id })):[];
        const listTag = { type: 'DocumentRiseQualifications', id: 'LIST' };
        return [...tags, listTag];
      },
    }),
    getDocumentRiseQualificationPaged: builder.query({
      query: ({ pageNumber, pageSize, filterDataReq }) => {
        // Логируем входящие параметры
        console.log('Called getDocumentRiseQualificationPaged with params:', {
          pageNumber,
          pageSize,
          filterDataReq: filterDataReq || 'filterDataReq not provided'
        });

        return `paged?page=${pageNumber}&size=${pageSize}`;
      },
      providesTags: (result)=>{
        var tags = result?.data ?
            result.data.map(({id})=>({type: 'DocumentRiseQualification', id })):[];
        const listTag = { type: 'DocumentRiseQualifications', id: 'LIST' };
        return [...tags, listTag];
      },
    }),
    getDocumentRiseQualificationById: builder.query({
      query: (id) => id,
      providesTags: (result, error, id) => [{ type: 'DocumentRiseQualification', id }],
    }),
    addDocumentRiseQualification: builder.mutation({
      query: (item) => ({
        method: 'POST',
        body: item,
      }),
      invalidatesTags: [{type:'DocumentRiseQualifications', id:'LIST' }],
    }),
    editDocumentRiseQualification: builder.mutation({
      query: ({ id, item }) => ({
        url: id,
        method: 'PUT',
        body: item,
      }),
      invalidatesTags: (result, error, { id }) => [{type:'DocumentRiseQualification', id:'LIST' },{type:'DocumentRiseQualification', id: id }],
    }),
    removeDocumentRiseQualification: builder.mutation({
      query: (id) => ({
        url: id,
        method: 'DELETE',
      }),
      invalidatesTags: [{type:'DocumentRiseQualification', id:'LIST' }],
    }),
  }),
});

export const {
  useGetDocumentRiseQualificationQuery,
  useGetDocumentRiseQualificationPagedQuery,
  useGetDocumentRiseQualificationByIdQuery,
  useAddDocumentRiseQualificationMutation,
  useEditDocumentRiseQualificationMutation,
  useRemoveDocumentRiseQualificationMutation,
} = DocumentRiseQualificationApi;