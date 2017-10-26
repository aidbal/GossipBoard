using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GossipBoard.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections;
using JWT;
using JWT.Serializers;
using JWT.Algorithms;
using Microsoft.Extensions.Options;
using System;
using GossipBoard.Dto;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GossipBoard.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _options;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JWTSettings> optionsAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _options = optionsAccessor.Value;
        }
        
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] UserDto credentials)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(credentials.Email);
                if (user == null)
                {
                    return NotFound();
                }

                if (!await _userManager.CheckPasswordAsync(user, credentials.Password))
                {
                    return Error("Wrong password");
                }

                user.Firstname = credentials.Firstname;
                user.Lastname = credentials.Lastname;
                
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new EmptyResult();
                }
                return Errors(result);
            }
            return Error("Unexpected error");
        }

        [Authorize]
        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] UserDto credentials)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(credentials.Email);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userManager.ChangePasswordAsync(user, credentials.OldPassword, credentials.Password);

                if (result.Succeeded)
                {
                    return new EmptyResult();
                }
                return Errors(result);
            }
            return Error("Unexpected error");
        }

        /**
         * Test request: [POST]
         *      Content-Type: applicaion/json
         *      {
         *        "username":
         *        "email":
         *        "firstname":
         *        "lastname":
         *        "password"
         *       }
         *       
         *       Gives you a json object structured like:
         *       {
         *          tokenObject:{
         *             access_token: "",
         *             id_token: ""
         *          },
         *          userObject:{
         *              email: "",
         *              firstname: "",
         *              lastname: ""
         *          }
         *       }
         */
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDto credentials)
        {
            if (ModelState.IsValid)
            {
                credentials.Username = Guid.NewGuid().ToString(); // the user name is not entered in a form, so generating a new Guid
                /*Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Register controller with credentials:" +
                                   "\n\t username: " + credentials.Username+
                                   "\n\t email: " + credentials.Email+
                                   "\n\t firstname: " + credentials.Firstname);
                Console.ResetColor();*/
                var user = new ApplicationUser {UserName = credentials.Username, Email = credentials.Email, Firstname = credentials.Firstname, Lastname = credentials.Lastname};
                var result = await _userManager.CreateAsync(user, credentials.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "tokenObject", new Dictionary<string, object>{
                            { "access_token", GetAccessToken(credentials.Email) },
                            { "id_token", GetIdToken(user) }
                        }},
                        {"userObject", new Dictionary<string, object>{
                            { "email", user.Email },
                            { "firstname", user.Firstname },
                            { "lastname", user.Lastname },
                        }}
                    });
                }
                return Errors(result);

            }
            return Error("Unexpected error");
        }


        /**
         * Example request: [POST]
         *  Content-Type: application/json
         *  {
         *  "email": "mail@mail.com",
         *  "password": "SuperSecurePassword123"
         *  }
         *  
         *  Gives you a json object structured like:
         *       {
         *          tokenObject:{
         *             access_token: "",
         *             id_token: ""
         *          },
         *          userObject:{
         *              email: "",
         *              firstname: "",
         *              lastname: ""
         *          }
         *       }
         */
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UserDto credentials)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(credentials.Email);
                if(user is null) {
                    /*
                     * This should be replaced with the same Auth fail message, to increase security
                     * in production
                     */
                    return new JsonResult("Email not registered in database") { StatusCode = 401 };
                }
                var result = await _signInManager.PasswordSignInAsync(user.UserName, credentials.Password, false, false);
                if (result.Succeeded)
                {
                    /*Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Log in with email and password successful, searching for user by email");
                    Console.ResetColor();*/
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "tokenObject", new Dictionary<string, object>{
                            { "access_token", GetAccessToken(credentials.Email) },
                            { "id_token", GetIdToken(user) }
                        }},
                        {"userObject", new Dictionary<string, object>{
                            { "email", user.Email },
                            { "firstname", user.Firstname },
                            { "lastname", user.Lastname },
                        }}
                    });
                }
                /*Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("Unable to sign in: \n Locked out:" + result.IsLockedOut + " \n IsNotAllowed: " + result.IsNotAllowed + "\n result.succeeded "+result.Succeeded);
                Console.WriteLine("User details:\n email " + user.Email + " \n username " + user.UserName + " \n password " + credentials.Password);
                Console.ResetColor();*/
                return new JsonResult("Unable to sign in") { StatusCode = 401 };
            }
            return Error("Unexpected error");
        }

        private string GetIdToken(ApplicationUser user)
        {
            var payload = new Dictionary<string, object>
            {
                /*
                 * Will be hashed, so doeasnt need much repeating
                 * If i'm not wrong
                 */
                { "id", user.Id },
                //{ "sub", user.Email },
                { "email", user.Email }//,
                //{ "emailConfirmed", user.EmailConfirmed },
            };
            //Console.WriteLine("GetIdToken dict:" + payload.ToString());
            return GetToken(payload);
        }

        private string GetAccessToken(string Email)
        {
            var payload = new Dictionary<string, object>
            {
                //{ "sub", Email },
                { "email", Email }
            };
            //Console.WriteLine("GetAccessToken dict:" + payload.ToString());
            return GetToken(payload);
        }

        private string GetToken(Dictionary<string, object> payload)
        {
            var secret = _options.SecretKey;

            payload.Add("iss", _options.Issuer);
            payload.Add("aud", _options.Audience);
            payload.Add("nbf", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("iat", ConvertToUnixTimestamp(DateTime.Now));
            payload.Add("exp", ConvertToUnixTimestamp(DateTime.Now.AddDays(7)));
            //Console.WriteLine("GetToken dict:" + payload.ToString());

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(payload, secret);
        }

        private JsonResult Errors(IdentityResult result)
        {
            var items = result.Errors
                .Select(x => x.Description)
                .ToArray();
            return new JsonResult(items) { StatusCode = 400 };
        }

        private JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }

        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}


