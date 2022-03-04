using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using SimpleForm.Models;

namespace SimpleForm.Services
{
    public class FormService
    {
        public FormViewModel CreateAreaInputModel(FormInputModel saveRequest, out int statusCode)
        {
            if (IsEmailValid(saveRequest.Email))
            {
                TextAreaInput inputModel = new TextAreaInput();
                inputModel.Birth = saveRequest.Birth;
                inputModel.Email = saveRequest.Email;
                inputModel.Name = saveRequest.Name;
                if (saveRequest.React is not null && saveRequest.React.Equals("Yes"))
                {
                    inputModel.React = "Yes";
                }
                else if (saveRequest.React is null || !saveRequest.React.Equals("Yes"))
                {
                    inputModel.React = "No";
                }

                inputModel.Region = ListOfRegions()[saveRequest.RegionIndex];
                inputModel.SurName = saveRequest.SurName;
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                statusCode = 200;
                return new FormViewModel()
                    {TextAreaInput = inputModel, TextAreaInputString = JsonSerializer.Serialize(inputModel, options)};
            }

            statusCode = 400;
            return null;
        }

        public FormInputModel CreateFormInputModel(string textAreaInput, out int statusCode)
        {
            if (string.IsNullOrEmpty(textAreaInput) || !textAreaInput.Contains('{'))
            {
                statusCode = 400;
                return null;
            }
            var formInput = new TextAreaInput();
            try
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                formInput = JsonSerializer.Deserialize<TextAreaInput>(textAreaInput, options);
                if (formInput is not null)
                {
                    if (IsEmailValid(formInput.Email) && !string.IsNullOrEmpty(formInput.Email) &&
                        !string.IsNullOrEmpty(formInput.SurName) && !string.IsNullOrEmpty(formInput.Name))
                    {
                        var indexOfRegion = ListOfRegions().IndexOf(formInput.Region);
                        bool react;
                        if (formInput.React.Equals("Yes") || formInput.React.Equals("No"))
                        {
                            if (formInput.React.Equals("Yes"))
                            {
                                statusCode = 200;
                                return new FormInputModel()
                                    {Birth = formInput.Birth, Email = formInput.Email, Name = formInput.Name, React = formInput.React, RegionIndex = indexOfRegion, SurName = formInput.SurName};
                            }
                            statusCode = 200;
                            return new FormInputModel()
                                {Birth = formInput.Birth, Email = formInput.Email, Name = formInput.Name, React = "No", RegionIndex = indexOfRegion, SurName = formInput.SurName};
                        }
                        statusCode = 400;
                        return null;
                    }

                    statusCode = 409;
                    return null;
                }
                else
                {
                    statusCode = 400;
                }
            }
            catch (JsonException f)
            {
                Console.WriteLine(f);
                statusCode = 406;
            }
            

            return null;
        }

        public bool IsEmailValid(string email)
        {
            return email.Contains('@') && email.Contains('.');
        }

        public List<string> ListOfRegions()
        {
            var listOfRegions = new List<string>();
            listOfRegions.Add("Hlavní město Praha");
            listOfRegions.Add("Středočeský kraj");
            listOfRegions.Add("Jihočeský kraj");
            listOfRegions.Add("Plzeňský kraj");
            listOfRegions.Add("Karlovarský kraj");
            listOfRegions.Add("Ústecký kraj");
            listOfRegions.Add("Liberecký kraj");
            listOfRegions.Add("Královéhradecký kraj");
            listOfRegions.Add("Pardubický kraj");
            listOfRegions.Add("Kraj Vysočina");
            listOfRegions.Add("Jihomoravský kraj");
            listOfRegions.Add("Zlínský kraj");
            listOfRegions.Add("Olomoucký kraj");
            listOfRegions.Add("Moravskoslezský kraj");
            return listOfRegions;
        }
    }
}