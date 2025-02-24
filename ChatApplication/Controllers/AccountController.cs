﻿using ChatApplication.DTOs;
using ChatApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Ecommerce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(UserDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Return validation errors
                return BadRequest(ModelState);
            }

            try
            {
                string res = await _accountService.Register(dto);
                if (res == "success")
                    return RedirectToAction("Login", "Account");
                else if (res == "username already exist")
                {
                    ModelState.AddModelError(string.Empty, "Registration failed. User with the same username already exist");
                    return View(dto);
                }
                else
                {
                    // Registration failed, add an error message to ModelState
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View(dto); // Return the view with the provided DTO to display error message
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View(dto); // Return the view with the provided DTO to display error message
            }
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Return to the login view with validation errors
                return View(dto);
            }

            try
            {
                // Obtain ConnectionId from SignalR Hub
                //var connectionId = HttpContext.Items["ConnectionId"]?.ToString();

                // Authenticate the user using the provided credentials
                string value = await _accountService.Authenticate(dto);

                if (!string.IsNullOrEmpty(value))
                {
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    var isValidJwt = tokenHandler.CanReadToken(value);
                    if (isValidJwt)
                    {
                        // User authenticated successfully, redirect to the home page
                        return RedirectToAction("Index", "User");
                    }
                    else if (value == "Invalid Password" || value == "Invalid Credentials")
                    {
                        // Authentication failed due to invalid username or password
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                        return View(dto);
                    }
                    else
                    {
                        // Other authentication errors
                        ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                        return View(dto);
                    }
                }
                else
                {
                    // Authentication failed, add an error message to ModelState
                    ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    return View(dto); // Return the view with the provided DTO to display error message
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View(dto); // Return the view with the provided DTO to display error message
            }
        }

        public async Task<IActionResult> Logout()
        {
            // Obtain ConnectionId from SignalR Hub
            //var connectionId = HttpContext.Items["ConnectionId"]?.ToString();

            await _accountService.LogoutUser();

            return RedirectToAction("Index", "Home");
        }
    }
}
