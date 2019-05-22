using REA.Models;
using System.Collections.Generic;

namespace REA.ViewModels
{
    static class CurrentState
    {
        #region Properties

        public static RealtyPhoto Photo { get; set; } = new RealtyPhoto();
        public static string UserEmail { get; set; } = "";
        public static List<string> ErrorList { get; set; } = new List<string>();
        public static string CurrentLanguage { get; set; } = "РУС";

        #endregion
    }
}
