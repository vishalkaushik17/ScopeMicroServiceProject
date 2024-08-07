namespace ModelTemplates.Persistence.Models.AppLevel
{
    /// <summary>
    /// For getting values into session.
    /// </summary>
    public class SystemPreferenceForSession
    {
        /// <summary>
        /// Name of the module where this key get applied.
        /// </summary>
        public string ModuleName { get; set; } = string.Empty;
        /// <summary>
        /// Name of the preference key
        /// </summary>
        public string PreferenceName { get; set; } = string.Empty;
        /// <summary>
        /// Value of the preference key
        /// </summary>
        public string Value { get; set; } = string.Empty;
        /// <summary>
        /// Data type of the preference key
        /// </summary>
        public GenericFunction.Enums.ValueType ValueType { get; set; }


    }
}
