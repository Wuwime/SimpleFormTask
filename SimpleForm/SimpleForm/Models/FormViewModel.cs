using System.Collections.Generic;

namespace SimpleForm.Models
{
    public class FormViewModel
    {
        public FormInputModel FormInput { get; set; }
        public TextAreaInput TextAreaInput { get; set; }
        public string TextAreaInputString { get; set; }
        public List<string> ListOfRegions { get; set; }
        public string Alert { get; set; }
    }
}