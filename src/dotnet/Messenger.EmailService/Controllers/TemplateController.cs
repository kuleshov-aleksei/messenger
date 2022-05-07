using Messenger.Common.JWT;
using Messenger.Common.Settings;
using Messenger.EmailService.Models;
using Messenger.EmailService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Threading.Tasks;

namespace Messenger.EmailService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/email/template")]
    public class TemplateController : ControllerBase
    {
        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();
        private readonly VariablesProvider m_variablesProvider;

        public TemplateController(VariablesProvider variablesProvider)
        {
            m_variablesProvider = variablesProvider;
        }

        [HttpGet("register")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRegisterTempalate()
        {
            string welcomeTeplate = await DBSettings.ReadSettingsAsync("email_welcome_template");
            return Ok(new
            {
                template = welcomeTeplate,
            });
        }

        [HttpPost("register")]
        [Consumes("application/json")]
        public async Task<IActionResult> SetRegisterTempalate(SetTemplateModel templateModel)
        {
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Info($"User {userId} changing template \"email_welcome_template\" to \"{templateModel.Template}\"");

            if (string.IsNullOrEmpty(templateModel?.Template))
            {
                return BadRequest();
            }

            await DBSettings.ChangeSettingsAsync("email_welcome_template", templateModel.Template);
            return Ok();
        }

        [HttpGet("password_reset")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPasswordReplaceTempalate()
        {
            string welcomeTeplate = await DBSettings.ReadSettingsAsync("email_password_reset_template");
            return Ok(new
            {
                template = welcomeTeplate,
            });
        }

        [HttpPost("password_reset")]
        [Consumes("application/json")]
        public async Task<IActionResult> SetPasswordReplaceTempalate(SetTemplateModel templateModel)
        {
            int userId = JwtHelper.GetUserId(User.Claims);
            m_logger.Info($"User {userId} changing template \"email_password_reset_template\" to \"{templateModel.Template}\"");

            if (string.IsNullOrEmpty(templateModel?.Template))
            {
                return BadRequest();
            }

            await DBSettings.ChangeSettingsAsync("email_password_reset_template", templateModel.Template);
            return Ok();
        }

        [HttpGet("variables")]
        [Produces("application/json")]
        public IActionResult GetTempalateVariables()
        {
            return Ok(m_variablesProvider.GetExampleData());
        }
    }
}
