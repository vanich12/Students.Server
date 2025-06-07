import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import apiUrl from './apiUrl.js';

export const ordersApi = createApi({
  reducerPath: 'orders',
  keepUnusedDataFor: 30,
  tagTypes: ['Orders', 'Order', 'OrderById'],
  baseQuery: fetchBaseQuery({ baseUrl: `${apiUrl}/order` }),
  endpoints: (builder) => ({
    getOrders: builder.query({
      query: () => '',
    }),
    getOrdersPaged: builder.query({
      query: ({ pageNumber, pageSize, filterDataReq }) => `paged?page=${pageNumber}&size=${pageSize}${filterDataReq}`, //  TODO: Переделать
      providesTags: (result)=>{
        var requestTags = result?.data ?
            result.data.map(({id})=>({type: 'Order', id })):[];
        const listTag = { type: 'Orders', id: 'LIST' };
        return [...requestTags, listTag];
      },
    }),
    getOrderById: builder.query({
      query: (id) => id,
      providesTags: (result, error, id) => [{ type: 'Order', id }],
    }),
    addOrder: builder.mutation({
      query: (item) => ({
        method: 'POST',
        url:'PostDTO',
        body: item,
      }),

      invalidatesTags: [{ type: 'Orders', id: 'LIST'}],
    }),
    editOrder: builder.mutation({
      query: ({ id, item }) => ({
        url: id,
        method: 'PUT',
        body: item,
      }),
      invalidatesTags: (result, error, id) => [{ type: 'Order', id }],
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