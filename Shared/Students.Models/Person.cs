using Students.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
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


    }
}
