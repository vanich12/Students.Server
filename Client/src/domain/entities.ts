import {
    Gender, YesNo, BirthDate, Address, PhoneNumber, Email, Snils, Age,
    EducationType, ScopeOfActivitySelect
} from './types'
export interface IPerson {

    family: string;
    name: string;
    patron?: string;
    birthDate: BirthDate;
    sex: Gender;
    age?: Age;
    address: Address;
    phone: PhoneNumber;
    email: Email;
    snils: Snils;
    nationality: string;
    typeEducationId: EducationType;
    speciality: string;
    scopeOfActivityLevelOneId: ScopeOfActivitySelect;
    scopeOfActivityLevelTwoId: ScopeOfActivitySelect;
    fullNameDocument?: string;
    documentSeries?: string;
    documentNumber?: string;
    disability: YesNo;
     projects?: string[];
     iT_Experience?: string;
}