import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const kindDocumentRiseQualificationApi = createApi({
  reducerPath: 'kindDocumentRiseQualification',
  tagTypes:['KindDocumentRiseQualification','KindDocumentRiseQualifications'],
  baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/kindDocumentRiseQualification` }),
  endpoints: (builder) => ({
    getKindDocumentRiseQualification: builder.query({
      query: () => '',
      providesTags: (result)=>{
        var tags = result?.data ?
            result.data.map(({id})=>({type: 'KindDocumentRiseQualification', id })):[];
        const listTag = { type: 'KindDocumentRiseQualifications', id: 'LIST' };
        return [...tags, listTag];
      },
    }),
    getKindDocumentRiseQualificationPaged: builder.query({
      query: ({ pageNumber, pageSize, filterDataReq }) => `paged?page=${pageNumber}&size=${pageSize}${filterDataReq}`,
      providesTags: (result)=>{
        var tags = result?.data ?
            result.data.map(({id})=>({type: 'KindDocumentRiseQualification', id })):[];
        const listTag = { type: 'KindDocumentRiseQualifications', id: 'LIST' };
        return [...tags, listTag];
      },
    }),
    getKindDocumentRiseQualificationById: builder.query({
      query: (id) => id,
      providesTags: (result, error, id) => [{ type: 'KindDocumentRiseQualification', id }],
    }),
    addKindDocumentRiseQualification: builder.mutation({
      query: (item) => ({
        method: 'POST',
        body: item,
      }),
      invalidatesTags: [{type:'KindDocumentRiseQualifications', id:'LIST' }],
    }),
    editKindDocumentRiseQualification: builder.mutation({
      query: ({ id, item }) => ({
        url: id,
        method: 'PUT',
        body: item,
      }),
      invalidatesTags: (result, error, { id }) => [{type:'KindDocumentRiseQualifications', id:'LIST' },{type:'KindDocumentRiseQualification', id: id }],
    }),
    removeKindDocumentRiseQualification: builder.mutation({
      query: (id) => ({
        url: id,
        method: 'DELETE',
      }),
      invalidatesTags: [{type:'KindDocumentRiseQualifications', id:'LIST' }],
    }),
  }),
});

export const {
  useGetKindDocumentRiseQualificationQuery,
  useGetKindDocumentRiseQualificationPagedQuery,
  useGetKindDocumentRiseQualificationByIdQuery,
  useAddKindDocumentRiseQualificationMutation,
  useEditKindDocumentRiseQualificationMutation,
  useRemoveKindDocumentRiseQualificationMutation,
} = kindDocumentRiseQualificationApi;