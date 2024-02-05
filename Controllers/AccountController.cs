
using CustomPageApp.Data;
using CustomPageApp.Models;
using CustomPageApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace CustomPageApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration _config;
        private AppDbContext context;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration config, AppDbContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this._config = config;
            this.context = context;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm login)

        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, lockoutOnFailure: false);


                if (result.Succeeded)
                {
                    var token = GenerateToken(login);
                    // HttpContext.Response.Cookies.Append(
                    HttpContext.Response.Cookies.Append("access_token", token, new CookieOptions { HttpOnly = true, Secure = true });
                    // Redirect to a success page or home page
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Optionally, you can check for specific failure reasons and provide custom error messages
                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "Account locked out. Please try again later.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    }

                    // If you want to show detailed errors, you can iterate through the result.Errors
                    // foreach (var error in result.Errors)
                    // {
                    //     ModelState.AddModelError(string.Empty, error.Description);
                    // }
                }
            }

            // If ModelState is not valid or login fails, return to the login view with the model
            return View(login);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Name = model.Name,
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                        await signInManager.SignInAsync(user, isPersistent: false);

                        var registrationData = new RegisterVm
                        {
                            Name = model.Name,
                            Email = model.Email,
                            Password = model.Password,
                            Address = model.Address
                        };

                        context.registerVm.Add(registrationData);
                        context.SaveChanges();

                        return RedirectToAction("ShowData", "Account");
                    }
                    else
                    // Handle registration failures
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {

                            return BadRequest("Email is already taken. Please choose a different one.");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
            }
            else
            {
                var existingRecord = context.registerVm.Find(model.Id);
                // It's an update, find the existing record by Id

                if (existingRecord != null)
                {
                    // Update the values with the new ones
                    if (model.Name != null || model.Email != null || existingRecord.Address != null)
                    {


                        existingRecord.Name = model.Name;
                        existingRecord.Email = model.Email;
                        existingRecord.Address = model.Address;
                        context.SaveChanges();


                        return RedirectToAction("ShowData", "Account");

                    }
                    else
                    {
                        return BadRequest("Enter required fields");


                    }


                }

                 else if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    // Handle the case where the record with the given Id was not found
                    ModelState.AddModelError(string.Empty, "Record not found for update.");
                }


            }
            return View(model);
        }

        public async Task<IActionResult> Logout()

        {
            await signInManager.SignOutAsync();
            Response.Cookies.Delete("access_token");
            return RedirectToAction("Index", "Home");

        }
        public IActionResult ShowData()

        {

            var data = context.registerVm.ToList();

            return View(data);
        }

        public IActionResult DeleteStudent(int Id)
        {

            if(Id != 0)
            {
                var data = context.registerVm.Find(Id);
                context.registerVm.Remove(data);
                context.SaveChanges();
              return RedirectToAction("ShowData", "Account");
            }
            return Json(new { success = true });

        }


        [HttpGet]
        public IActionResult EditPartial(int id)
        {
            // Retrieve the data for the specified ID and pass it to the partial view
              // Retrieve the model data based on the ID
            var model = context.registerVm.SingleOrDefault(x => x.Id == id);
        return PartialView("_EditPartial", model);
        }
        [HttpGet]
        public IActionResult CreateStudent()

        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateStudent(EmpModel empModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Enter required fields");
            }

            // Map EmpModel to Employee
            var employee = new EmpModel();
            {
                employee.Name = empModel.Name;
                employee.City = empModel.City;
                employee.Address = empModel.Address;
            };

            // Insert into database
            try
            {
                {
                    context.Employees.Add(employee);
                    context.SaveChanges();
                }

                return Json(employee);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to insert data. Error: {ex.Message}");
            }
        }


        //generating aa token here be carefull 
        private string GenerateToken(LoginVm user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Issuer"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
          new Claim(ClaimTypes.NameIdentifier, user.UserName),
          new Claim(ClaimTypes.Name, user.Password)
      };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddSeconds(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
