﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace REA.Models
{
    public class Employee : IDataErrorInfo
    {
        #region Fields

        [NotMapped]
        private string _passport;

        [NotMapped]
        private string _address;

        [NotMapped]
        private string _phone;

        #endregion

        #region Properties

        [Key]
        public string Email { get; set; }

        [Required]
        public string HashPassword { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Patronymic { get; set; }

        public string Passport
        {
            get => _passport;
            set
            {
                if (value == "")
                    _passport = null;
                else
                    _passport = value;
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                if (value == "")
                    _address = null;
                else
                    _address = value;
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                if (value == "")
                    _phone = null;
                else
                    _phone = value;
            }
        }

        public DateTime? Birthday { get; set; }

        public EmployeesPhoto Photo { get; set; }

        public virtual ICollection<Deal> Deals { get; set; }

        #endregion

        #region IDataErrorInfo

        [NotMapped]
        public string Error { get => string.Empty; }

        [NotMapped]
        public string this[string propertyName]
        {
            get
            {
                string result = string.Empty;
                propertyName = propertyName ?? string.Empty;
                if (propertyName == "Email" && (Email == null || !new Regex(@"^.+@[^\.].*\.[a-z]{2,}$").IsMatch(Email)))
                    result = App.Current.Resources["_ErrorWrongEmail"].ToString();
                if (propertyName == "Surname" && (Surname == null || !new Regex(@"^[a-zA-Zа-яА-Я'][a-zA-Zа-яА-Я-' ]+[a-zA-Zа-яА-Я']?$").IsMatch(Surname)))
                    result = App.Current.Resources["_ErrorWrongSurname"].ToString();
                if (propertyName == "Name" && (Name == null || !new Regex(@"^[a-zA-Zа-яА-Я'][a-zA-Zа-яА-Я-' ]+[a-zA-Zа-яА-Я']?$", RegexOptions.IgnoreCase).IsMatch(Name)))
                    result = App.Current.Resources["_ErrorWrongName"].ToString();
                if (propertyName == "Patronymic" && (Patronymic == null || !new Regex(@"^[a-zA-Zа-яА-Я'][a-zA-Zа-яА-Я-' ]+[a-zA-Zа-яА-Я']?$").IsMatch(Patronymic)))
                    result = App.Current.Resources["_ErrorWrongPatronymic"].ToString();
                if (propertyName == "HashPassword" && (HashPassword == null || !new Regex(@"^(?=.*\d).{4,8}$").IsMatch(HashPassword)))
                    result = App.Current.Resources["_ErrorWrongPasswordFormat"].ToString();
                if (propertyName == "Passport" && Passport != null  && !new Regex(@"^((AB)|(BM)|(HB)|(KH)|(MP)|(MC)|(KB)|(PP))\d{7}$").IsMatch(Passport))
                    result = App.Current.Resources["_ErrorWrongPassport"].ToString();
                if (propertyName == "Phone" && Phone != null && !new Regex(@"^(\+375)(44|25|29)\d{7}$").IsMatch(Phone))
                    result = App.Current.Resources["_ErrorWrongPhone"].ToString();
                return result;
            }
        }

        #endregion

        public Employee()
        {
            Deals = new List<Deal>();
        }

        public override string ToString()
        {
            return $"[{Email}] {Surname} {Name}";
        }
    }
}
