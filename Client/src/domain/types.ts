export enum Gender {
    Male = 'male',
    Female = 'female',
}

// src/types/YesNo.ts
export enum YesNo {
    Yes = 'yes',
    No = 'no',
}


export type BirthDate = Date; // Или class BirthDate { ... }


export interface Address {
    street: string;
    city: string;
    postalCode: string;
}

export type PhoneNumber = string;


export type Email = string;


export type Snils = string; // Или class Snils { ... }

export type EducationType = string | number;


export type ScopeOfActivitySelect = string | number;

export type Age = number;