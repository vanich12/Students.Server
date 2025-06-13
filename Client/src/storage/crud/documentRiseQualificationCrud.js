import { 
  useGetDocumentRiseQualificationQuery,
  useGetDocumentRiseQualificationPagedQuery,
  useGetDocumentRiseQualificationByIdQuery,
  useAddDocumentRiseQualificationMutation,
  useEditDocumentRiseQualificationMutation,
  useRemoveDocumentRiseQualificationMutation,
} from '../services/DocumentRiseQualificationApi';

export {
  useGetDocumentRiseQualificationQuery as useGetAllAsync,
  useGetDocumentRiseQualificationPagedQuery as useGetAllPagedAsync,
  useGetDocumentRiseQualificationByIdQuery as useGetOneByIdAsync,
  useAddDocumentRiseQualificationMutation as useAddOneAsync,
  useEditDocumentRiseQualificationMutation as useEditOneAsync,
  useRemoveDocumentRiseQualificationMutation as useRemoveOneAsync,
}