import { 
    useGetOrdersQuery,
    useGetOrdersPagedQuery,
    useGetOrderByIdQuery,
    useAddOrderMutation,
    useEditOrderMutation,
    useRemoveOrderMutation,
  } from '../services/orderApi.js';
  
  export {
    useGetOrdersQuery as useGetAllAsync,
    useGetOrdersPagedQuery as useGetAllPagedAsync,
    useGetOrderByIdQuery as useGetOneByIdAsync,
    useAddOrderMutation as useAddOneAsync,
    useEditOrderMutation as useEditOneAsync,
    useRemoveOrderMutation as useRemoveOneAsync,
  }