﻿using GestionPersonnel.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPersonnel.Models.Pointage
{

    public class Pointage
    {
        public int PointageID { get; set; }
        public int EmployeID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan HeureEntree { get; set; }
        public TimeSpan HeureSortie { get; set; }
        public decimal HeuresTravaillees { get; set; }
        public string Remarque { get; set; }
        public string Stat
        {
            get => HeuresTravaillees > 0 ? "Présent" : "Absent";
            set { }
        }

        public int Persontage
        {
            get => (int)Math.Floor((HeuresTravaillees / 8m) * 100);
            set { }
        }

        public string NomEmploye { get; set; }
        public string PrenomEmploye { get; set; }
        public string NomFonction { get; set; }
        private decimal _journeeCoefficient;
        private decimal _heuresSupplementairesCoefficient;

        public decimal JourneeCoefficient
        {
            get =>(int)Math.Floor( _journeeCoefficient * 100);
            set => _journeeCoefficient = value;
        }

        public decimal HeuresSupplementairesCoefficient
        {
            get =>(int)Math.Floor(_heuresSupplementairesCoefficient * 100) ;
            set => _heuresSupplementairesCoefficient = value;
        }
    }
}
