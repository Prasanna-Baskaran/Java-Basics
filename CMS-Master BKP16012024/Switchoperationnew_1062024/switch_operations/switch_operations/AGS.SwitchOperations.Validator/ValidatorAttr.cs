using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AGS.SwitchOperations.Validator
{
    public class ValidatorAttr
    {
        public string Name { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public bool Isrequired { get; set; }

        public bool Alpabetic { get; set; }
        public bool Numeric { get; set; }
        public bool AlphaNumeric { get; set; }
        public bool AlphaNumericOnly { get; set; }
        public string Regex { get; set; }
        public string Value { get; set; }
        public string CompareName { get; set; }
        public string CompareValue { get; set; }
        public string Message { get; set; }
        public bool isValid { get; set; }
        public bool isChecked { get; set; }
        public string RequiredGroup { get; set; }

    }

    public static class ExtentionMethods
    {
        public static bool Validate(this List<ValidatorAttr> lstAttr, out string errorMsg)
        {
            foreach (ValidatorAttr attr in lstAttr)
            {

                if (!attr.Isrequired && string.IsNullOrEmpty(attr.Value))
                {
                    attr.isValid = true;
                    continue;
                }
                if (attr.Isrequired && string.IsNullOrEmpty(attr.Value))
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field Cannot be Empty").Trim();
                    continue;
                }
                if (attr.MaxLength != null && attr.Value.Length > attr.MaxLength)
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field lenght must be limited to " + attr.MaxLength + " characters").Trim();
                    continue;
                }
                if (attr.MinLength != null && attr.Value.Length < attr.MinLength)
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field lenght must be at least to " + attr.MinLength + " characters").Trim();
                    continue;
                }
                if (attr.Numeric && !Regex.IsMatch(attr.Value, "^[0-9]*$"))
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field value must be Numeric only").Trim();
                    continue;
                }
                if (attr.Alpabetic && !Regex.IsMatch(attr.Value, "^[a-zA-Z]*$"))
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field value must be Alphabetic only").Trim();
                    continue;
                }
                if (attr.AlphaNumeric && !Regex.IsMatch(attr.Value, "^[a-zA-Z]*$") && !Regex.IsMatch(attr.Value, "^[0-9]*$") && !Regex.IsMatch(attr.Value, "^[a-zA-Z0-9]*$"))
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field value must be Alpha-numeric").Trim();
                    continue;
                }
                if (attr.AlphaNumericOnly && (Regex.IsMatch(attr.Value, "^[a-zA-Z]*$") || Regex.IsMatch(attr.Value, "^[0-9]*$")) && Regex.IsMatch(attr.Value, "^[a-zA-Z0-9]*$"))
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field value is not Alpha-numeric only").Trim();
                    continue;
                }
                if (!string.IsNullOrEmpty(attr.Regex) && !Regex.IsMatch(attr.Value, attr.Regex))
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field value is not valid").Trim();
                    continue;
                }
                if (!string.IsNullOrEmpty(attr.CompareValue) && !attr.Value.Equals(attr.CompareValue, StringComparison.Ordinal))
                {
                    if (string.IsNullOrEmpty(attr.Message))
                        attr.Message = string.Concat(attr.Name.Trim(), " Field value is not matching with ", attr.CompareName.Trim(), " Field").Trim();
                    continue;
                }
                attr.isValid = true;
            }

            errorMsg = string.Join(", ", lstAttr.AsEnumerable().Where(a => !a.isValid).Select(m => m.Message).ToArray()).Trim().TrimEnd(',');
            if (errorMsg == "")
                foreach (string grp in lstAttr.Where(x => x.isValid).Where(x => x.RequiredGroup != null).Select(x => x.RequiredGroup).Distinct())
                {
                    List<ValidatorAttr> lstgrpAttr = lstAttr.Where(x => x.RequiredGroup == grp).ToList();

                    if (lstgrpAttr.Count() == lstgrpAttr.Where(x => string.IsNullOrEmpty(x.Value)).Count())
                    {
                        lstgrpAttr.ForEach(x => x.isValid = false);
                        errorMsg = "Please Enter " + string.Join(" or ", lstgrpAttr.Select(m => m.Name).ToArray()).Trim() + "";
                    }
                }

            return (lstAttr.AsEnumerable().Where(a => !a.isValid).Count() == 0);
        }
    }
}
