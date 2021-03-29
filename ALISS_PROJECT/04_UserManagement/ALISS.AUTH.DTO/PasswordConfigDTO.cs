using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ALISS.AUTH.DTO
{
    public class PasswordConfigDTO
    {
        public int pwc_id { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        //[DynamicRangeValidator("pwc_user_max_char", ErrorMessage = "Value must be between 0 and {1}")]
        public int pwc_user_min_char { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        public int pwc_user_max_char { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        //[DynamicRangeValidator("pwc_max_char", ErrorMessage = "Value must be between 0 and {1}")]
        public int pwc_min_char { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        public int pwc_max_char { get; set; }
        public bool pwc_lowwer_letter { get; set; }
        public bool pwc_upper_letter { get; set; }
        public bool pwc_number { get; set; }
        public bool pwc_special_char { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        public int pwc_max_invalid { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        public int pwc_force_reset { get; set; }
        [Range(0, 99, ErrorMessage = "* Please enter number between {1} to {2}")]
        public int pwc_session_timeout { get; set; }
        public string pwc_default_password { get; set; }
        public string pwc_createuser { get; set; }
        public DateTime? pwc_createdate { get; set; }
        public string pwc_updateuser { get; set; }
        public DateTime? pwc_updatedate { get; set; }
    }
    public class DynamicRangeValidator : ValidationAttribute
    {
        private readonly string _maxPropertyName;
        public DynamicRangeValidator(string maxPropertyName)
        {
            _maxPropertyName = maxPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var maxProperty = validationContext.ObjectType.GetProperty(_maxPropertyName);
            if (maxProperty == null)
            {
                return new ValidationResult(string.Format("Unknown property {0}", _maxPropertyName));
            }

            int maxValue = (int)maxProperty.GetValue(validationContext.ObjectInstance, null);
            int currentValue = (int)value;
            if (currentValue >= maxValue)
            {
                return new ValidationResult(
                    string.Format(
                        ErrorMessage,
                        0,
                        maxValue
                    )
                );
            }

            return null;
        }
    }
}
