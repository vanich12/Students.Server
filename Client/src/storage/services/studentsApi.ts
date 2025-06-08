import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl';
import {IPagedPageData, StudentDTO} from "../../domain/interfaces";

interface IPagedParams {
  pageNumber: number;
  pageSize: number;
  filterDataReq: string;
}

interface IPagedParams {
  pageNumber: number;
  pageSize: number;
  filterDataReq: string;
}

interface PagedResponse<T> {
  data: T[];
  total: number;
  page: number;
}

export const studentsApi = createApi({
  reducerPath: 'students',
  keepUnusedDataFor: 5,
  baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/student` }),
  tagTypes: ['Students', 'Student', 'GroupStudentList','LearningHistory'], // 1. Определяем типы тегов
  endpoints: (builder) => ({
    getStudents: builder.query<StudentDTO[], void>({
      query: () => '',
    }),

    getStudentsPaged: builder.query<IPagedPageData<StudentDTO>, IPagedParams>({
      // query остается как был, принимая filterDataReq
      query: ({ pageNumber, pageSize, filterDataReq }) => {
        const relativeUrlString = `paged?page=${pageNumber}&size=${pageSize}${filterDataReq}`;
        console.log('[RTK Query Log] Endpoint: getStudentsPaged');
        console.log('[RTK Query Log] Input Params:', { pageNumber, pageSize, filterDataReq });
        console.log('[RTK Query Log] Generated Relative URL:', relativeUrlString);
        return relativeUrlString;
      },
      // 2. ОБНОВЛЯЕМ providesTags, АНАЛИЗИРУЯ filterDataReq
      providesTags: (result, error, { filterDataReq }) => {
        // Базовые теги для отдельных студентов и общего списка
        const studentTags = result?.data
            ? result.data.map(({ id }) => ({ type: 'Student' as const, id }))
            : [];
        const listTag = { type: 'Students' as const, id: 'LIST' };

        // Пытаемся извлечь groupId из строки filterDataReq
   const groupIdMatch = filterDataReq?.match(/&groupId=([^&]+)/);

        const groupId = groupIdMatch ? groupIdMatch[1] : null;

        // Если groupId найден в фильтре, добавляем специфичный тег
       if (groupId) {
          console.log(`[RTK Query Log - providesTags] Found groupId=${groupId} in filter. Providing GroupStudentList tag.`);
          return [
            ...studentTags,
            listTag,
            { type: 'GroupStudentList' as const, id: groupId } // <--- Наш специфичный тег
          ];
        }

        // Иначе возвращаем только базовые теги
        console.log("[RTK Query Log - providesTags] No groupId found in filter. Providing only base tags.");
        return [...studentTags, listTag];
      }
    }),

    getStudentById: builder.query<StudentDTO, string>({
      query: (id) => id,
      providesTags: (result, error, id) => [{ type: 'Student', id }],
    }),
      getLearningHistoryOfStudent: builder.query({
          query: ({studentId,hasGroup}) => {
              const groupRelation = hasGroup ? `&hasGroup=${hasGroup}` : "";
              const relativeUrlString = `GetLearningHistoryOfStudent?studentId=${studentId}${groupRelation}`;
              const requestConfig ={
                  url: relativeUrlString,
                  method: 'GET'
              };
              return requestConfig;
          },
          providesTags:[{type: 'LearningHistory', id:'LIST'}],
      }),
    addStudent: builder.mutation<StudentDTO, Partial<StudentDTO>>({
      query: (student) => {

        console.log('[addStudent Query Arg]:', student);

        const requestConfig = {
          url: '',
          method: 'POST',
          body: student,
        };
        // Логируем конфигурацию запроса, которая будет передана в baseQuery
        console.log('[addStudent Request Config]:', requestConfig);

        return requestConfig;
      },
      invalidatesTags: ['Students'],
    }),

    editStudent: builder.mutation<StudentDTO, { id: string; data: Partial<StudentDTO> }>({
      query: ({id, data}) =>{
        console.log('ID перед отправкой:', id);
        console.log('DATA перед отправкой:', data);

        const config = {
          url: `EditStudent/${id}`, // Убедитесь, что URL правильный
          method: 'PUT',
          body: data, // Проблема здесь: почему 'data' не уходит в тело?
        };
        return config;
      },
      invalidatesTags: (result, error, { id }) => [
        { type: 'Student', id },
        'PersonHistories',
        'Students'
      ],
    }),


    removeStudent: builder.mutation<void, string>({
      query: (id) => ({
        url: id,
        method: 'DELETE',
      }),
      invalidatesTags: (result, error, id) => [
        { type: 'Student', id },
        'Students'
      ],
    }),
  }),
});

// Экспорт хуков
export const {
  useGetStudentsQuery,
  useGetStudentsPagedQuery,
  useGetStudentByIdQuery,
    useGetLearningHistoryOfStudentQuery,
  useAddStudentMutation,
  useEditStudentMutation,
  useRemoveStudentMutation,
} = studentsApi;