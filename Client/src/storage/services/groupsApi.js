import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const groupsApi = createApi({
  reducerPath: 'groups',
  keepUnusedDataFor: 30,
  baseQuery: fetchBaseQuery(
      { baseUrl: `${apiUrl}/group` }),
  tagTypes: ['Groups', 'GroupStudents', 'GroupStudentList'],
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
      providesTags: (result, error, id) => [
        { type: 'Groups', id: 'LIST' },
      ],
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
      },
      // ИНВАЛИДИРУЕМ тег для студентов КОНКРЕТНОЙ группы
      invalidatesTags: (result, error, { groupId }) => {
        console.log(`[RTK Query Log - invalidatesTags] Invalidating GroupStudentList tag for groupId=${groupId}`);
        return [{ type: 'GroupStudentList', id: groupId }];
      },
    }),
    removeStudentFromGroup: builder.mutation({
      query: ({studentId, groupId}) => {
        const relativeUrlString = `RemoveStudentsFromGroupByRequest?studentId=${studentId}&groupId=${groupId}`;
        console.log(relativeUrlString);
        const requestConfig = {
          url: relativeUrlString,
          method: 'DELETE',
        };
        return requestConfig;
      },
      invalidatesTags: (result, error, { groupId }) => {
        console.log(`[RTK Query Log - invalidatesTags] Invalidating GroupStudentList tag for groupId=${groupId}`);
        return [{ type: 'GroupStudentList', id: groupId },{type: 'Requests', id: 'LIST' }];
      },
    }),
      editGroup: builder.mutation({
        query: ({id, item}) => ({
          url: id,
          method: 'PUT',
          body: item,
        }),
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
    useRemoveStudentFromGroupMutation,
  useEditGroupMutation,
  useRemoveGroupMutation,
} = groupsApi;