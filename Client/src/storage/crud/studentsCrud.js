import React, { useState, useEffect } from 'react';
import { 
  useGetStudentsQuery,
  useGetStudentsPagedQuery,
  useGetStudentByIdQuery,
  useGetLearningHistoryOfStudentQuery,
  useAddStudentMutation,
  useEditStudentMutation,
  useRemoveStudentMutation,
} from '../services/studentsApi.ts';

const useGetAllAsync = () => {
  const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetStudentsQuery();


  return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};

const useGetAllPagedAsync = ({ pageNumber, pageSize, filterDataReq: queryString }) => {
  const { data, isError, isSuccess, error, isLoading, isFetching, refetch } = useGetStudentsPagedQuery({ pageNumber, pageSize, filterDataReq: queryString });


  return { data, isError, isSuccess, error, isLoading, isFetching, refetch };
};


const useRemoveOneAsync = () => {
  const [removeTrigger, removingResult] = useRemoveStudentMutation();
  const { data, error, isUninitialized, isLoading, isSuccess, isError, reset } = removingResult;

  return [removeTrigger, removingResult];
};

export {
  useGetAllAsync,
  useGetAllPagedAsync,
  useGetStudentByIdQuery as useGetOneByIdAsync,
  useGetLearningHistoryOfStudentQuery as useGetLearningHistoryOfStudent,
  useAddStudentMutation as useAddOneAsync,
  useEditStudentMutation as useEditOneAsync,
  useRemoveOneAsync,
}