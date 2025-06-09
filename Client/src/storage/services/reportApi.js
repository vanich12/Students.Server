import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

// СОЗДАЕМ СВОЮ ФУНКЦИЮ ДЛЯ ЗАПРОСОВ. ОНА ПРОСТАЯ И НАДЕЖНАЯ.
const rawBaseQuery = async (args, api, extraOptions) => {
    const { url, method, body } = args;
    const baseUrl = `${apiUrl}/report`;

    try {
        const response = await fetch(`${baseUrl}${url}`, {
            method,
            body
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || `Сетевая ошибка: ${response.status}`);
        }

        // САМОЕ ГЛАВНОЕ ИЗМЕНЕНИЕ:
        // Вместо всего объекта Response, мы сразу получаем его содержимое как ArrayBuffer
        // и возвращаем только его. ArrayBuffer - это просто массив байтов.
        const buffer = await response.arrayBuffer();
        return { data: buffer };

    } catch (error) {
        return { error: { status: 'FETCH_ERROR', error: error.message } };
    }
};

export const reportApi = createApi({
    reducerPath: 'report',
    keepUnusedDataFor: 0,
    // ИСПОЛЬЗУЕМ НАШУ НОВУЮ ФУНКЦИЮ
    baseQuery: rawBaseQuery,
    tagTypes: ['report', 'reports'],
    endpoints: (builder) => ({
        addPFDOReport: builder.mutation({
            // Теперь query просто возвращает объект для нашего rawBaseQuery
            query: ({ startDate, endDate }) => ({
                url: `/GetPFDOReport?startDate=${startDate}&endDate=${endDate}`,
                method: 'POST'
                // formData: true БОЛЬШЕ НЕ НУЖНО И НЕ БУДЕТ РАБОТАТЬ
            }),

            // responseHandler БОЛЬШЕ НЕ НУЖЕН ЗДЕСЬ, мы все делаем в компоненте
            // УДАЛИ ЕГО!

            invalidatesTags: [{ type: 'reports', id: 'LIST' }]
        }),
        // ... другие твои эндпоинты
    }),
});

export const {
    useAddPFDOReportMutation,
    useAddRostatReportMutation,
} = reportApi;