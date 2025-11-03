using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Collections.Generic;
using VelocipedeUtils.Shared.Models.Business;
using VelocipedeUtils.Shared.Models.Business.Languages;
using VelocipedeUtils.Ubp.Core.DbContexts;

namespace VelocipedeUtils.Ubp.Core.Dal
{
    /// <summary>
    /// A class that provides general functionality for working with databases in the context of languages.
    /// </summary>
    public static class LanguageDal
    {
        /// <summary>
        /// Get a collection of labels for visual components from the database to implement language support.
        /// </summary>
        public static Dictionary<string, string> GetLanguageKvpByFormName(
            UbpDbContext context,
            LanguageType languageType,
            string formName,
            string applicationUid)
        {
            List<LanguageKeyValuePair> languageKvpList = context.LanguageKeyValuePairs
                .FromSqlRaw("SELECT DISTINCT * FROM public.\"GetLanguageKvpByFormName\"(@LanguageType, @FormName, @ApplicationUid)", 
                    new NpgsqlParameter("LanguageType", (int)languageType),
                    new NpgsqlParameter("FormName", formName),
                    new NpgsqlParameter("ApplicationUid", applicationUid))
                .ToList();
            
            var result = new Dictionary<string, string>();
            foreach (var kvp in languageKvpList)
            {
                if (!result.ContainsKey(kvp.Key))
                {
                    result.Add(kvp.Key, kvp.Value);
                }
            }
            return result;
        }

        /// <summary>
        /// Get a collection of available languages.
        /// </summary>
        public static List<LanguageKey> GetAvailableLanguages(
            UbpDbContext context)
        {
            return context.LanguageKeys
                .Where(x => x.BusinessEntityStatus == BusinessEntityStatus.Active)
                .Distinct()
                .ToList();
        }
    }
}