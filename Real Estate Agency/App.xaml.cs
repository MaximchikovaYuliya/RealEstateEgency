using REA.Models.UnitOfWork;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace REA
{
    public partial class App : Application
    {
        public static UnitOfWork UnitOfWork { get; set; } = new UnitOfWork();

        public static void SelectCulture(string culture)
        {
            if (string.IsNullOrEmpty(culture))
                return;

            var dictionaryList = Current.Resources.MergedDictionaries.ToList();
  
            string requestedCulture = string.Format("Localization/StringResources.{0}.xaml", culture);
            var resourceDictionary = dictionaryList.
                FirstOrDefault(d => d.Source.OriginalString == requestedCulture);
 
            if (resourceDictionary != null)
            {
                Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                Current.Resources.MergedDictionaries.Add(resourceDictionary);
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }
    }
}
