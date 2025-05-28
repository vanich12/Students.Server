﻿using Students.Models.Enums;
using Students.Models.ReferenceModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Students.Models
{
    public class Person
    {
        /// <summary>
        /// Идентификатор персоны
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Фамилия
        /// экспорт из заявки
        /// </summary>
        public required string Family { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Отчество
        /// экспорт из заявки
        /// </summary>
        public string? Patron { get; set; }

        /// <summary>
        /// ФИО
        /// экспорт из заявки
        /// </summary>
        //Возможно нужна стратегия отображения ФИО, но тогда через конструктор
        public string FullName => $"{this.Family} {this.Name} {this.Patron}";

        /// <summary>
        /// Дата рождения
        /// </summary>
        public required DateOnly BirthDate { get; set; }

        /// <summary>
        /// Возраст
        /// </summary>
        public int Age
        {
            get
            {
                var age = DateTime.Now.Year - this.BirthDate.Year;
                //День рождения еще не наступил
                if (DateTime.Now.DayOfYear < this.BirthDate.DayOfYear)
                {
                    age--;
                }

                return age;
            }
        }

        /// <summary>
        /// Пол
        /// Справочник
        /// </summary>
        public required SexHuman Sex { get; set; }

        /// <summary>
        /// Гражданство
        /// </summary>
        public string? Nationality { get; set; }

        /// <summary>
        /// Адрес, по-хорошему нужен либо справочник, либо формат стандарта ГОСТа Р 6.30-2003
        /// экспорт из заявки
        /// </summary>
        public required string Address { get; set; }

        /// <summary>
        /// Валидированный телефон.
        /// </summary>
        private string _phone;

        //список полей для связи, вероятно нужно в отдельную таблицу
        /// <summary>
        /// Телефон
        /// экспорт из заявки
        /// </summary>
        public required string Phone
        {
            get => this._phone;
            set
            {
                if (Regex.IsMatch(value, @"^\+7\s\(\d{3}\)\s\d{3}-\d{2}-\d{2}$"))
                    this._phone = value;
                else
                    throw new ValidationException("Not a valid phone number.");
            }
        }

        /// <summary>
        /// Валидированный электронный адрес.
        /// </summary>
        private string _email;

        //public string PhonePrepared { get { return Phone.Length > 10 ? Phone.Substring(Phone.Length - 10) : Phone; } }
        /// <summary>
        /// Электронный адрес
        /// экспорт из заявки
        /// </summary>
        public required string Email
        {
            get => this._email;
            set
            {
                value = value.ToLower();
                if (Regex.IsMatch(value,
                        @"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$") &&
                    MailAddress.TryCreate(value, out var address))
                    this._email = address.Address;
                else
                    throw new ValidationException("Not a valid Email address.");
            }
        }

        /// <summary>
        /// Опыт в ИТ
        /// экспорт из заявки
        /// </summary>
        public required string IT_Experience { get; set; }

        /// <summary>
        /// Ид Уровень образования
        /// экспорт из заявки, хотя по факту тут тоже некий справочник Высшее образование / Среднее профессиональное образование / Студент ВО / Студент СПО
        /// </summary>
        public Guid? TypeEducationId { get; set; }

        /// <summary>
        /// Id сферы деятельности(1 уровень).
        /// </summary>
        public required Guid ScopeOfActivityLevelOneId { get; set; }

        /// <summary>
        /// Id сферы деятельности(2 уровень).
        /// </summary>
        public Guid? ScopeOfActivityLevelTwoId { get; set; }

        /// <summary>
        /// ссылка на студента (связь 1 к 1)
        /// </summary>
        [JsonIgnore]
        public virtual Student? Student { get; set; }

        /// <summary>
        /// Идентификатор пользователя, кто последний вносил изменения в данные студента
        /// необходимо для отслеживания изменений данных студента
        /// </summary>
        [JsonIgnore] public Guid? LastChangedByUserId { get; set; }

        /// <summary>
        /// поданные студентом заявки
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<Request>? Requests { get; set; }


        /// <summary>
        /// Сфера деятельности, уже есть как бы класс сфера деятельности с уровнями
        /// Хоть и список, но по факту должен содержать только 2 значения (1 уровень и второй???)
        /// </summary>
        [JsonIgnore]
        public virtual ScopeOfActivity? ScopeOfActivityLevelOne { get; set; }

        /// <summary>
        /// Сфера деятельности, уже есть как бы класс сфера деятельности с уровнями
        /// Хоть и список, но по факту должен содержать только 2 значения (1 уровень и второй???)
        /// </summary>
        [JsonIgnore]
        public virtual ScopeOfActivity? ScopeOfActivityLevelTwo { get; set; }
        /// <summary>
        /// Уровень образования
        /// экспорт из заявки, хотя по факту тут тоже некий справочник Высшее образование / Среднее профессиональное образование / Студент ВО / Студент СПО
        /// </summary>
        [JsonIgnore]
        public virtual TypeEducation? TypeEducation { get; set; }
    }
}