import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const ordersApi = createApi({
  reducerPath: 'orders',
  keepUnusedDataFor: 30,
  baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/order` }),
  endpoints: (builder) => ({
    getOrders: builder.query({
      query: () => '',
    }),
    getOrdersPaged: builder.query({
      query: () => '',  //  TODO: Переделать
      providesTags: ['Orders'],
    }),
    getOrderById: builder.query({
      query: (id) => id,
      invalidatesTags: ['Orders'],
    }),
    addOrder: builder.mutation({
      query: (item) => ({
        method: 'POST',
        url:'PostDTO',
        body: item,
      }),
      invalidatesTags: ['Orders'],
    }),
    editOrder: builder.mutation({
      query: ({ id, item }) => ({
        url: id,
        method: 'PUT',
        body: item,
      }),
      invalidatesTags: ['Orders'],
    }),
    removeOrder: builder.mutation({
      query: (id) => ({
        url: id,
        method: 'DELETE',
      }),
      invalidatesTags: ['Orders'],
    }),
  }),
});

export const {
  useGetOrdersQuery,
  useGetOrdersPagedQuery,
  useGetOrderByIdQuery,
  useAddOrderMutation,
  useEditOrderMutation,
  useRemoveOrderMutation,
} = ordersApi;