import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const groupsApi = createApi({
  reducerPath: 'groups',
  keepUnusedDataFor: 30,
  baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/group` }),
  endpoints: (builder) => ({
    getGroups: builder.query({
      query: () => '',
    }),

    getGroupsPaged: builder.query({
      query: () => '',  //  TODO: Переделать
      providesTags: ['Groups'],
    }),

    getGroupById: builder.query({
      query: (id) => id,
      invalidatesTags: ['Groups'],
    }),

    addGroup: builder.mutation({
      query: (item) => ({
        method: 'POST',
        body: item,
      }),
      invalidatesTags: ['Groups'],
    }),

    addStudentInGroup: builder.mutation({
      query: ({objects, groupId}) => {
        const relativeUrlString = `AddStudentsToGroupByRequest?groupId=${groupId}`;
        const requestConfig = {
          url: relativeUrlString,
          method: 'POST',
          body: objects,
        };

        return requestConfig;
      }
    }),
      editGroup: builder.mutation({
        query: ({id, item}) => ({
          url: id,
          method: 'PUT',
          body: item,
        }),
        invalidatesTags: ['Groups'],
      }),
      removeGroup: builder.mutation({
        query: (id) => ({
          url: id,
          method: 'DELETE',
        }),
        invalidatesTags: ['Groups'],
      }),
  }),
});

export const {
  useGetGroupsQuery,
  useGetGroupsPagedQuery,
  useGetGroupByIdQuery,
  useAddGroupMutation,
  useAddStudentInGroupMutation,
  useEditGroupMutation,
  useRemoveGroupMutation,
} = groupsApi;