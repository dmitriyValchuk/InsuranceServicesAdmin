﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyDox;

namespace InsuranceServicesAdminLight.Business.TemplateGeneration
{
    public class GenerationOSCPV
    {
        public static void GenerateDOCX()
        {
            //Temp test value
            var fieldValues = new Dictionary<string, string> {
                {"Код страховика", "078"},
                {"ГП", "00"},
                {"ХП", "00"},
                {"ДП", "31"},
                {"МП", "05"},
                {"РП", "19"},
                {"ДЗ", "30"},
                {"МЗ", "05"},
                {"РЗ", "20"},
                {"Розмір франшизи", "Дев'ятсот п'ятдесят грн."},
                {"Прізвище", "Ніколаєнко"},
                {"Ім'я", "Ігор"},
                {"По батькові", "Віталійович"},
                {"Код ЄДРПОУ", "2790810299"},
                {"Адреса", "Києво-Святошинський р-н., с. Петропавлівська Борщагівка, вул. Шкільна, буд. 19, кв. 29"},
                {"ДН", "29"},
                {"МН", "05"},
                {"РН", "1976"},
                {"Код ІНПП", "2790810299"},
                {"Тип документу", "Посвідчення водія"},
                {"Серія документу", "ВАН"},
                {"Номер документу", "186828"},
                {"ДВ", "24"},
                {"МВ", "10"},
                {"РВ", "2000"},
                {"Орган, що видав документ", "Києво-Святошинське РЕВ ДАІ"},
                {"Тип ТЗ", "В 3"},
                {"Номер ТЗ", "АІ0057СМ"},
                {"Марка ТЗ", "Volkswagen"},
                {"Модель ТЗ", "T5"},
                {"Рік випуску", "2006"},
                {"ВІН код", "WV1ZZZ7HZ7H010460"},
                {"Місце реєстрації ТЗ", "Києво-Святошинське РЕВ ДАІ"},
                {"Таксі?", "Ні"},
                {"ОТК?", "Ні"},
                {"Будь-хто?", "Так"},
                {"Місяці використання", ""},
                {"Дата наступного ОТК", ""},
                {"Базовий", "180"},
                {"К1", "1,18"},
                {"К2", "1,22"},
                {"К3", "1"},
                {"К4", "1,55"},
                {"К5", "1"},
                {"К6", "1"},
                {"К7", "1"},
                {"БМ", "1,40"},
                {"КПарк", "..."},
                {"КПільговик", "..."},
                {"Сума грн.", "562"},
                {"Сума коп.", "00"},
                {"Сума прописом", "П'ятсот шістдесят дві грн. 00 коп."},
                {"ДС", "24"},
                {"МС", "05"},
                {"РС", "19"},
                {"ДУ", "24"},
                {"МУ", "05"},
                {"РУ", "19"},
                {"ПІБ", "Ніколаєнко І.В."},
            };

            var engine = new Engine();

            engine.Merge("E:\\Диплом\\template.docx", fieldValues, "E:\\Диплом\\templateOut.docx");
        }
    }
}