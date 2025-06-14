import {
  useGetGroupsQuery,
  useGetGroupsPagedQuery,
  useGetGroupByIdQuery,
  useAddGroupMutation,
  useAddStudentInGroupMutation,
  useRemoveStudentFromGroupMutation,
  useEditGroupMutation,
  useRemoveGroupMutation,
} from '../services/groupsApi.js';

const useAddSubjectRangeAsync = () => {
  // 1. Вызываем оригинальный хук БЕЗ аргументов данных
  const [trigger, result] = useAddStudentInGroupMutation();

  // 2. Возвращаем полученный массив [функцияЗапуска, объектРезультата]
  // Теперь компонент, использующий useAddSubjectRangeAsync, получит то же самое,
  // как если бы он использовал useAddStudentInGroupMutation напрямую.
  return [trigger, result];
};

const useRemoveSubjectRangeAsync = () => {
  // 1. Вызываем оригинальный хук БЕЗ аргументов данных
  const [trigger, result] = useRemoveStudentFromGroupMutation();

  // 2. Возвращаем полученный массив [функцияЗапуска, объектРезультата]
  // Теперь компонент, использующий useAddSubjectRangeAsync, получит то же самое,
  // как если бы он использовал useAddStudentInGroupMutation напрямую.
  return [trigger, result];
};


export {
  useGetGroupsQuery as useGetAllAsync,
  useGetGroupsPagedQuery as useGetAllPagedAsync,
  useGetGroupByIdQuery as useGetOneByIdAsync,
  useAddGroupMutation as useAddOneAsync,
  useAddSubjectRangeAsync,
  useRemoveSubjectRangeAsync,
  useEditGroupMutation as useEditOneAsync,
  useRemoveGroupMutation as useRemoveOneAsync,
}