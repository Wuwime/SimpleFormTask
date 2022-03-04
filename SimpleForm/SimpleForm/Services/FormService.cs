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
            if (IsEmailValid(saveRequest.Email))// checking if email is valid
            {
                TextAreaInput inputModel = new TextAreaInput();
                inputModel.Birth = saveRequest.Birth;
                inputModel.Email = saveRequest.Email;
                inputModel.Name = saveRequest.Name;
                if (saveRequest.React is not null && saveRequest.React.Equals("Yes"))//checks if the REACT checkbox is checked or not
                {
                    inputModel.React = "Yes";
                }
                else if (saveRequest.React is null || !saveRequest.React.Equals("Yes"))
                {
                    inputModel.React = "No";
                }

                inputModel.Region = ListOfRegions()[saveRequest.RegionIndex]; // region is selected base on the index provided
                inputModel.SurName = saveRequest.SurName;
                var options = new JsonSerializerOptions //helps character encoding
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                statusCode = 200;//in case everything is ok, returns code success and view model
                return new FormViewModel()
                    {TextAreaInput = inputModel, TextAreaInputString = JsonSerializer.Serialize(inputModel, options)};
            }
//in case something went wrong, returns null and bad code 4OO 
            statusCode = 400;
            return null;
        }

        public FormInputModel CreateFormInputModel(string textAreaInput, out int statusCode)
        {
            if (string.IsNullOrEmpty(textAreaInput) || !textAreaInput.Contains('{')) //if text area is empty, returns bad request
            {
                statusCode = 400;
                return null;
            }
            var formInput = new TextAreaInput();
            try//in case of bad date format, we'll catch json exception
            {
                var options = new JsonSerializerOptions//helps character encoding again
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                formInput = JsonSerializer.Deserialize<TextAreaInput>(textAreaInput, options); //converts string to object
                if (formInput is not null)
                {
                    if (IsEmailValid(formInput.Email) && !string.IsNullOrEmpty(formInput.Email) &&
                        !string.IsNullOrEmpty(formInput.SurName) && !string.IsNullOrEmpty(formInput.Name))//checks if email is valid and all the fields are filled out
                    {
                        var indexOfRegion = ListOfRegions().IndexOf(formInput.Region); //gets index from a list by checking item provided 
                        
                        if (formInput.React.Equals("Yes") || formInput.React.Equals("No"))//checks if the REACT checkbox is checked or not
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

        public bool IsEmailValid(string email)//checks if email is valic
        {
            return email.Contains('@') && email.Contains('.');
        }

        public List<string> ListOfRegions()//list, so that I don't have to write it down twice
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