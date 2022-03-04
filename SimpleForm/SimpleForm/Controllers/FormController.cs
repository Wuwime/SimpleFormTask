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
        public IActionResult GetJsonResult([FromForm] FormInputModel saveRequest)
        {
            int statusCode = 0;
            var response = FormService.CreateAreaInputModel(saveRequest, out statusCode);
            if (statusCode == 200)
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
            var response = FormService.CreateFormInputModel(textAreaInput, out int statusCode);
            if (statusCode == 200)
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