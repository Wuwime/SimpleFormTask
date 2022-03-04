using Microsoft.AspNetCore.Mvc;
using SimpleForm.Models;
using SimpleForm.Services;

namespace SimpleForm.Controllers
{
    public class FormController : Controller
    {
        private FormService FormService;

        public FormController(FormService formService)
        {
            FormService = formService;
        }

        [Route("")]
        [HttpGet("home")]
        public IActionResult FormPage()
        {
            return View(new FormViewModel() {ListOfRegions = FormService.ListOfRegions()});
        }

        [HttpPost("save")]
        public IActionResult GetJsonResult([FromForm] FormInputModel saveRequest)// accepts request
        {
            int statusCode = 0;
            var response = FormService.CreateAreaInputModel(saveRequest, out statusCode);//sends request to the service and returns response with json in string
            if (statusCode == 200)//in case everything's alright, returns 200 status code. Otherwise, returns alert.
            {
                return View("FormPage",
                    new FormViewModel()
                    {
                        TextAreaInput = response.TextAreaInput, TextAreaInputString = response.TextAreaInputString,
                        ListOfRegions = FormService.ListOfRegions(), Alert = "Success!"
                    });
            }

            return View("FormPage",
                new FormViewModel()
                {
                    FormInput = saveRequest, Alert = "Email doesn't match requirements!",
                    ListOfRegions = FormService.ListOfRegions()
                });
        }

        [HttpPost("load")]
        public IActionResult FromJsonToForm([FromForm] string textAreaInput)
        {
            var response = FormService.CreateFormInputModel(textAreaInput, out int statusCode);//receives string, sends it to the service and returns json object
            if (statusCode == 200)//just like up above, if everything went well, returns 200, otherwise you should get alert 
            {
                return View("FormPage",
                    new FormViewModel() {FormInput = response, ListOfRegions = FormService.ListOfRegions()});
            }

            if (statusCode == 406)
            {
                return View("FormPage",
                    new FormViewModel()
                    {
                        TextAreaInputString = textAreaInput, Alert = "Wrong date format!",
                        ListOfRegions = FormService.ListOfRegions()
                    });
            }

            if (statusCode == 409)
            {
                return View("FormPage",
                    new FormViewModel()
                    {
                        TextAreaInputString = textAreaInput, Alert = "Fill out the required fields!",
                        ListOfRegions = FormService.ListOfRegions()
                    });
            }

            return View("FormPage",
                new FormViewModel()
                {
                    TextAreaInputString = textAreaInput, Alert = "Input doesn't match requirements!",
                    ListOfRegions = FormService.ListOfRegions()
                });
        }
    }
}