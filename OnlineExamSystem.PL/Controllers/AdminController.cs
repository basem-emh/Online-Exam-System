using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Custom_Models.Exam;
using OnlineExamSystem.BLL.Custom_Models.Questions;
using OnlineExamSystem.BLL.Services.Admin;
using OnlineExamSystem.PL.ViewModels.Exam;

namespace OnlineExamSystem.PL.Controllers
{
    public class AdminController : Controller
    {
        #region Services
        private readonly IAdminServices _adminServices;
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _environment;
        
        public AdminController(IAdminServices adminServices
            , ILogger<AdminController> logger
            , IWebHostEnvironment environment)
        {
            _adminServices = adminServices;
            _logger = logger;
            _environment = environment;
        }
        #endregion

        #region Index
        [HttpGet]
        [Authorize("Admin")]
        public async Task<IActionResult> Index()
        {
            var exams = await _adminServices.GetAllExamsAsync(); 
            return View(exams);
        }
        #endregion

        #region Details
        [HttpGet]
        [Authorize("Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var exam = await _adminServices.GetExamByIdAsync(id.Value);

            if (exam is null)
                return NotFound();

            var result = new ExamQuestionViewModel
            {
                Exam = new ExamViewModel
                {
                    Id = exam.Id,
                    Title = exam.Title
                },
                Questions= exam.Questions.Select(Q=> new QuestionViewModel
                {
                    Id = Q.Id,
                    Title = Q.Title,
                    Choice1 = Q.Choice1,
                    Choice2 = Q.Choice2,
                    Choice3 = Q.Choice3,
                    Choice4 = Q.Choice4,
                    CorrectAnswer = Q.CorrectAnswer,
                }).ToList()
            };

            return View(result);
        }
        #endregion

        #region Create
        [HttpGet]
        [Authorize("Admin")]
        public IActionResult Create()
        {
            var model = new ExamQuestionViewModel
            {
                Exam = new ExamViewModel(),
                Questions = new List<QuestionViewModel> { new QuestionViewModel() }
            };
            ViewBag.Action = "Create";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Admin")]
        public async Task<IActionResult> Create(ExamQuestionViewModel model)
        {
            var message = string.Empty;
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        _logger.LogError(error.ErrorMessage);
                    }
                    return View(model);
                }
                var createdExam = new CreateExamDto()
                {
                    Title = model.Exam.Title,
                    Questions = model.Questions.Select(q => new CreateQuestionsDto
                    {
                        Title = q.Title,
                        Choice1 = q.Choice1,
                        Choice2 = q.Choice2,
                        Choice3 = q.Choice3,
                        Choice4 = q.Choice4,
                        CorrectAnswer = q.CorrectAnswer
                    }).ToList()
                };

                var created = await _adminServices.CreateExamAsync(createdExam) > 0;

                if (created)
                    TempData["Message"] = $"{model.Exam.Title} is created";
                else
                {
                    ModelState.AddModelError(string.Empty, message);
                    TempData["Message"] = $"{model.Exam.Title} is not created";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);

                // 2. Set Message
                message = _environment.IsDevelopment() ? ex.Message : $"An error has occurred during Creating {model.Exam.Title} :(";

                TempData["Message"] = message;

                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region AddNewQuestion
        [HttpGet]
        [Authorize("Admin")]
        public async Task<IActionResult> AddQuestionToExam(int? id)
        {
            if (id is null)
                return NotFound();

            var exam = await _adminServices.GetExamByIdAsync(id.Value);
            if (exam is null)
                return NotFound();

            var model = new ExamQuestionViewModel
            {
                Exam = new ExamViewModel
                {
                    Id = exam.Id,
                    Title = exam.Title
                },
                Questions = new List<QuestionViewModel> { new QuestionViewModel() } // سؤال جديد
            };

            ViewBag.Action = "AddQuestionToExam";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddQuestionToExam(int id, ExamQuestionViewModel model)
        {
            var message = string.Empty;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return View(model);
            }

            try
            {
                var examDto = new UpdateExamDto
                {
                    Id = id,
                    Title = model.Exam.Title,
                    Questions = model.Questions.Select(q => new UpdateQuestionDto
                    {
                        Title = q.Title,
                        Choice1 = q.Choice1,
                        Choice2 = q.Choice2,
                        Choice3 = q.Choice3,
                        Choice4 = q.Choice4,
                        CorrectAnswer = q.CorrectAnswer
                    }).ToList()
                };

                await _adminServices.CreateAnotherAsync(examDto);

                TempData["Message"] = "Question added successfully!";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding question to exam.");

                TempData["Message"] = _environment.IsDevelopment() ? ex.Message : "An error occurred while adding the question.";

                return RedirectToAction(nameof(Index));
            }
        }
        #endregion
       
        #region Exam
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TakeExam(int? id)
        {
            if (id == null)
                return BadRequest();

            var exam = await _adminServices.GetExamByIdAsync(id.Value);
            if (exam is null)
                return NotFound();

            ViewBag.examId = exam.Id;
            var result = new ExamQuestionViewModel
            {
                Exam = new ExamViewModel
                {
                    Id = exam.Id,
                    Title = exam.Title
                },
                Questions = exam.Questions.Select(Q => new QuestionViewModel
                {
                    Id = Q.Id,
                    Title = Q.Title,
                    Choice1 = Q.Choice1,
                    Choice2 = Q.Choice2,
                    Choice3 = Q.Choice3,
                    Choice4 = Q.Choice4,
                    CorrectAnswer = Q.CorrectAnswer
                }).ToList()
            };
            return View(result);
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult SubmitExam(ExamQuestionViewModel model)
        {
            int counter = 0;

            foreach (var question in model.Questions)
            {
                if (question.SelectedChoices != null &&
                    question.SelectedChoices.Count == 1 &&
                    question.SelectedChoices.Contains(question.CorrectAnswer))
                {
                    counter++;
                }
            }

            ViewBag.CorrectAnswers = counter;
            ViewBag.TotalQuestions = model.Questions.Count;

            return View(nameof(ExamResult)); 
        }
        public IActionResult ExamResult()
        {
            return View();
        }
        #endregion

        #region Update
        [HttpGet]
        [Authorize("Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();
            
            var exam = await _adminServices.GetExamByIdAsync(id.Value);
            if (exam is null)
                return NotFound();

            var model = new ExamQuestionViewModel
            {
                Exam = new ExamViewModel()
                {
                    Id = exam.Id,
                    Title = exam.Title
                },
                Questions = exam.Questions.Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    Title = q.Title, 
                    Choice1 = q.Choice1, 
                    Choice2 = q.Choice2, 
                    Choice3 = q.Choice3, 
                    Choice4 = q.Choice4, 
                    CorrectAnswer = q.CorrectAnswer 
                }).ToList()
            };
            ViewBag.Action = "Edit";
            return View(model);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Admin")]
        public async Task<IActionResult> Edit([FromRoute] int id, ExamQuestionViewModel model)
        {
            var message = string.Empty;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    _logger.LogError(error.ErrorMessage);
                }
                return View(model);
            }

            try
            {
                var updatedExam = new UpdateExamDto()
                {
                    Id = id,
                    Title = model.Exam.Title,
                    Questions = model.Questions.Select(q => new UpdateQuestionDto
                    {
                        Id = q.Id, // هنا التصحيح: يتم تعيين Id الخاص بالسؤال
                        Title = q.Title,
                        Choice1 = q.Choice1,
                        Choice2 = q.Choice2,
                        Choice3 = q.Choice3,
                        Choice4 = q.Choice4,
                        CorrectAnswer = q.CorrectAnswer
                    }).ToList()
                };

                var updated = await _adminServices.UpdateExamAsync(updatedExam) > 0;

                if (updated)
                    TempData["Message"] = $"{model.Exam.Title} is updated";
                else
                {
                    ModelState.AddModelError(string.Empty, message);
                    TempData["Message"] = $"{model.Exam.Title} is not updated";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : $"An error has occurred during Updating {model.Exam.Title} :(";
                TempData["Message"] = message;
                return RedirectToAction(nameof(Index));
            }
        }
        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = await _adminServices.DeleteExamAsync(id);
                if (deleted)
                    return RedirectToAction(nameof(Index));
                else
                    message = "An error has occured during deteting Exam :(";
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);

                //2. Set Message
                message = _environment.IsDevelopment() ? ex.Message : "An error has occured during deleting Exam :(";

            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


    }
}
