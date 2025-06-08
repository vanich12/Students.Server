import { configureStore } from '@reduxjs/toolkit';
import userReducer  from './slices/userSlice.js';
import { authApi }  from './services/authApi.js';
import { studentsApi }  from './services/studentsApi.ts';

import { personApi }  from './services/personApi';
import { educationFormApi }  from './services/educationFormApi.js';
import { requestStatusApi }  from './services/requestStatusApi.js';
import { typeEducationApi }  from './services/typeEducationApi.js';
import { studentStatusApi }  from './services/studentStatusApi.js';
import { kindOrderApi }  from './services/kindOrderApi.js';
import { kindDocumentRiseQualificationApi }  from './services/kindDocumentRiseQualificationApi.js';
import { financingTypeApi }  from './services/financingTypeApi.js';
import { feaProgramApi }  from './services/feaProgramApi.js';
import { educationProgramApi }  from './services/educationProgramApi.js';
import { groupsApi }  from './services/groupsApi.js';
import { requestsApi }  from './services/requestsApi.js';
import { pendingRequestsApi }  from './services/pendingRequestsApi.js';
import { scopeOfActivityApi }  from './services/scopeOfActivityApi.js';
import { ordersApi } from './services/orderApi.js';
import { reportApi } from './services/reportApi.js';
import { personHistoryApi } from './services/personHistoryApi.js';

export default configureStore({
  reducer: {
    user: userReducer,
    [authApi.reducerPath]: authApi.reducer,
    [studentsApi.reducerPath]: studentsApi.reducer,
    [personApi.reducerPath]:personApi.reducer,
    [reportApi.reducerPath]:reportApi.reducer,
    [personHistoryApi.reducerPath]:personHistoryApi.reducer,
    [educationFormApi.reducerPath]: educationFormApi.reducer,
    [requestStatusApi.reducerPath]: requestStatusApi.reducer,
    [typeEducationApi.reducerPath]: typeEducationApi.reducer,
    [studentStatusApi.reducerPath]: studentStatusApi.reducer,
    [ordersApi.reducerPath]: ordersApi.reducer,
    [kindOrderApi.reducerPath]: kindOrderApi.reducer,
    [kindDocumentRiseQualificationApi.reducerPath]: kindDocumentRiseQualificationApi.reducer,
    [financingTypeApi.reducerPath]: financingTypeApi.reducer,
    [feaProgramApi.reducerPath]: feaProgramApi.reducer,
    [educationProgramApi.reducerPath]: educationProgramApi.reducer,
    [groupsApi.reducerPath]: groupsApi.reducer,
    [requestsApi.reducerPath]: requestsApi.reducer,
    [pendingRequestsApi.reducerPath]: pendingRequestsApi.reducer,
    [scopeOfActivityApi.reducerPath]: scopeOfActivityApi.reducer
  },
  middleware: (
    (getDefaultMiddleware) => getDefaultMiddleware()
    .concat(authApi.middleware)
    .concat(studentsApi.middleware)
        .concat(personApi.middleware)
        .concat(personHistoryApi.middleware)
    .concat(educationFormApi.middleware)
    .concat(reportApi.middleware)
    .concat(requestStatusApi.middleware)
    .concat(typeEducationApi.middleware)
    .concat(studentStatusApi.middleware)
    .concat(kindOrderApi.middleware)
    .concat(kindDocumentRiseQualificationApi.middleware)
    .concat(financingTypeApi.middleware)
    .concat(feaProgramApi.middleware)
    .concat(educationProgramApi.middleware)
    .concat(groupsApi.middleware)
    .concat(ordersApi.middleware)
    .concat(requestsApi.middleware)
        .concat(pendingRequestsApi.middleware)
    .concat(scopeOfActivityApi.middleware)
  ),
});