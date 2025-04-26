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
  tagTypes: ['Students', 'Student'], // 1. Определяем типы тегов
  endpoints: (builder) => ({
    getStudents: builder.query<StudentDTO[], void>({
      query: () => '',
    }),

    getStudentsPaged: builder.query<IPagedPageData<StudentDTO>, IPagedParams>({
          query: ({ pageNumber, pageSize, filterDataReq }) => {
            // 1. Формируем строку запроса (относительную часть URL)
            const relativeUrlString = `paged?page=${pageNumber}&size=${pageSize}${filterDataReq}`;

            // 2. ВЫВОДИМ В КОНСОЛЬ сформированную строку и входные параметры
            console.log('[RTK Query Log] Endpoint: getStudentsPaged');
            console.log('[RTK Query Log] Input Params:', { pageNumber, pageSize, filterDataReq });
            console.log('[RTK Query Log] Generated Relative URL:', relativeUrlString);

            // 3. Возвращаем строку, как и раньше
            return relativeUrlString;
          },
      // используется для кеширования, нужно для того чтобы каждый обьект был за кэширован по id, и в случае мутации одного из студентов,
      // изменялся только этот студент из списка всех студентов
      // по средствам сравнения значений в кэше

      providesTags: (result) => {
        if (!result?.data) return ['Students'];
        return [
          ...result.data.map(({ id }) => ({ type: 'Student' as const, id })),
          { type: 'Students', id: 'LIST' }
        ];
      }
    }),

    getStudentById: builder.query<StudentDTO, string>({
      query: (id) => id,
      providesTags: (result, error, id) => [{ type: 'Student', id }],
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
      query: ({ id, data }) => ({
        url: id,
        method: 'PUT',
        body: data,
      }),
      // тут мы помечаем что обьект студента должен быть обновлен
      invalidatesTags: (result, error, { id }) => [
        { type: 'Student', id },
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
  useAddStudentMutation,
  useEditStudentMutation,
  useRemoveStudentMutation,
} = studentsApi;