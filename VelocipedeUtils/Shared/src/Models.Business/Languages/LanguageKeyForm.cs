using VelocipedeUtils.Shared.Models.Business.WfAppInfo;

namespace VelocipedeUtils.Shared.Models.Business.Languages
{
    /// <summary>
    /// Association between key used for translation, and the name of the form or view.
    /// </summary>
    public class LanguageKeyForm
    {
        /// <summary>
        /// ID of the business entity.
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Key used for translation.
        /// </summary>
        public required string Key { get; set; }
        
        /// <summary>
        /// Name of the form or view.
        /// </summary>
        public string? FormName { get; set; }
        
        /// <summary>
        /// UID of the application.
        /// </summary>
        public string? ApplicationUid { get; set; }
        
        /// <summary>
        /// Information about the application form.
        /// </summary>
        public WfApplicationForm? WfApplicationForm { get; set; }
        
        /// <summary>
        /// The date when the business entity was created.
        /// </summary>
        public DateTime? DateCreated { get; set; }

        /// <summary>
        /// The date when the business entity was changed.
        /// </summary>
        public DateTime? DateChanged { get; set; }

        /// <summary>
        /// Business entity status.
        /// </summary>
        public BusinessEntityStatus? BusinessEntityStatus { get; set; }
    }
}